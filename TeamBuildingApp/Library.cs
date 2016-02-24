using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Collections;
using GAF;
using GAF.Extensions;
using GAF.Operators;
using MoreLinq;


namespace TeamBuildingApp
{
    class Library
    {

        private static Library instance;
        private static DBConnection dbC;
        private static List<Question> questions;

        private static List<Student> students;
        private static List<KeyValuePair<List<KeyValuePair<Chromosome, double>>, double>> elite;
        private static List<KeyValuePair<List<Student>, double>> solutions; 

        private static double red_allowance;
        private static double blue_allowance;
        private static double green_allowance;
        private static double yellow_allowance;

        private static string message = "";
        private static int groupsize = 0;
        private static double solutionAverage = 0;

        private static Student stud;

        public static Library Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Library();

                    instance.connectToDb();

                }

                return instance;
            }
        }

        public Library()
        {
            questions = new List<Question>();

            students = new List<Student>();
            elite = new List<KeyValuePair<List<KeyValuePair<Chromosome, double>>, double>>();
            solutions = new List<KeyValuePair<List<Student>, double>>();
        }
        public String connectToDb()
        {
            try
            {
                dbC = new DBConnection();
                return "Connected";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }



        }

        public void disconnectDB()
        {
            try
            {
                dbC.DBDisconnect();
            }
            catch (Exception) { }
        }


        public Administrator validateLogin(string username, string password)
        {

            MySqlCommand cmdUser = new MySqlCommand("SELECT * FROM ADMINISTRATORS WHERE USERNAME = '" + username + "'", dbC.getConnection());
            cmdUser.Prepare();

            MySqlDataReader read = cmdUser.ExecuteReader();


            while (read.Read())
            {
                Administrator admin = new Administrator(read.GetString("First_Name"), read.GetString("Second_Name"), read.GetString("Email_Address"),
                    read.GetString("Username"), read.GetString("Company_Name"), read.GetString("Salt"), read.GetString("AdminPassword"));

                read.Close();

                if (admin.checkPassword(password))
                {

                    return admin;
                }
                return null;

            }

            read.Close();
            return null;
        }

        public Boolean checkUsername(string username)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM ADMINISTRATORS WHERE USERNAME ='" + username + "'", dbC.getConnection());
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@val1", username);

            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();
            if (read.HasRows)
            {
                //the username already exists
                return false;
            }
            else
            {
                return true;
            }
        }

        public String addAdministrator(string fn, string sn, string emailA, string companyName, string user)
        {
            try
            {

                Administrator admin = new Administrator(fn, sn, emailA, companyName, user, dbC.getConnection());
                return "Administrator: " + fn + " has been successfully added.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public String generateClassCode(Administrator admin, string classTitle)
        {

            string code = Path.GetRandomFileName().Replace(".", "").Substring(0, 10);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM CLASSCODES WHERE CLASS_TITLE = '" + classTitle + "'", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();

            if (read.HasRows)
            {
                return "This class already has a code";
            }


            cmd = new MySqlCommand("SELECT * FROM CLASSCODES WHERE CLASS_CODE = '" + code + "'", dbC.getConnection());


            while (true)
            {
                read = cmd.ExecuteReader();
                read.Close();

                if (read.HasRows)
                {
                    code = Path.GetRandomFileName().Replace(".", "").Substring(0, 10);
                    break;
                }

                break;

            }

            cmd = new MySqlCommand("INSERT INTO CLASSCODES(CLASS_CODE, CLASS_TITLE, COMPANY_NAME, ADMINISTRATOR_USERNAME)VALUES('" + code + "','" + classTitle + "','" + admin.getCompanyName() + "','" + admin.getUsername() + "')", dbC.getConnection());
            Console.WriteLine(cmd.CommandText);

            try
            {
                read = cmd.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                return ex.Message;
            }

            return "The code for " + classTitle + " is: " + code;


        }

        public List<Question> getQuestionSelection()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM QUESTIONS ORDER BY RAND() LIMIT 10", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {

                questions.Add(new Question(read.GetString("Question_ID"), read.GetString("Question"), read.GetString("Red_Answer"), read.GetString("Blue_Answer"), read.GetString("Green_Answer"), read.GetString("Yellow_Answer")));

            }

            read.Close();
            return questions;


        }

        public bool checkCode(string classcode)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM CLASSCODES WHERE CLASS_CODE = '" + classcode + "'", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();

            if (read.HasRows)
            {
                //format new student into type and store on db
                read.Close();
                return true;
            }
            else
            {
                read.Close();
                return false;
            }


        }

        public Student addStudent(string no, string fname, string sname, string classcode)
        {
           return new Student(no, fname, sname, classcode, dbC.getConnection());
        }

        public void completedResults(int red, int blue, int green, int yellow)
        {
            stud.updateResults(red, blue, green, yellow);
        }

        public void logOut()
        {
            instance = null;
        }

        public void setAllowance(double red, double blue, double green, double yellow)
        {
            red_allowance = red;
            blue_allowance = blue;
            green_allowance = green;
            yellow_allowance = yellow;
        }

        public String generateGroups(string classCode, int groupSize)
        {
            groupsize = groupSize;

            if (checkCode(classCode) == false) { return "Class Code Error"; }

            //get students from the db
            if (students.Count == 0) { students = createStudents(classCode);  }

            if (students.Count % groupsize != 0) { return ("Group Size Error"); }
            

            //create population from the available students
            Population pop = createPopulation(students, groupSize);

            //create operators & algorithm
            createOperatorsAndAlgorithm(pop);

            //format groupstructure

            return message;

        }

        public List<KeyValuePair<List<Student>,double>> getSolutionsList()
        {
            return solutions;
        }
        public double getSolutionAverage()
        {
            return solutionAverage;
        }

        private List<Student> createStudents(string classCode)
        {

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM STUDENTS WHERE CLASS_CODE = '" + classCode + "'", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {
                students.Add(new Student(read.GetString("Student_no"), read.GetString("First_Name"), read.GetString("Second_Name"), classCode,
                    read.GetInt32("Red_Results"), read.GetInt32("Blue_Results"), read.GetInt32("Green_Results"), read.GetInt32("Yellow_Results")));

            }

            read.Close();

            return students;
        }

        private Population createPopulation(List<Student> students, int groupSize)
        {
            Population pop = new Population();

            for (int i = 0; i < 100; i++)
            {
                Chromosome chromo = new Chromosome();

                int j = 0;
                var rnd = new Random();
                students = students.OrderBy(x => rnd.Next()).ToList();

                for (int k = 0; k <= students.Count; k++)
                {
                    Console.WriteLine(k);

                    if (j < groupSize)
                    {
                        chromo.Genes.Add(new Gene(students[k].getStudentNum()));
                        j = chromo.Count;
                    }
                    else
                    {
                        pop.Solutions.Add(chromo);
                        chromo = new Chromosome();
                        j = 0;
                        k = (k == 15) ? k : k - 1;

                    }
                }


            }

            return pop;


        }

        private void createOperatorsAndAlgorithm(Population pop)
        {
            Elite el = new Elite(5);

            Crossover cr = new Crossover(0.7, false);
            cr.CrossoverType = CrossoverType.DoublePoint;

            BinaryMutate mutate = new BinaryMutate(0.3, true);

            GeneticAlgorithm genA = new GeneticAlgorithm(pop, calculateFitness);

            genA.OnGenerationComplete += genA_OnGenerationComplete;
            genA.OnRunComplete += genA_OnRunComplete;

            //add the operators
            //genA.Operators.Add(el);
            //genA.Operators.Add(cr);
            //genA.Operators.Add(mutate);

            //run the GA
            genA.Run(genTerminate);

        }


        private static double calculateFitness(Chromosome chromo)
        {

            int red = 0;
            int blue = 0;
            int green = 0;
            int yellow = 0;
            int score = 100;



            //Count the instances of colours
            foreach (Gene g in chromo)
            {
                Student stud = getStudentByNum(g.ObjectValue.ToString());

                switch (stud.getPrimary())
                {
                    case "Red": red += 1; break;
                    case "Blue": blue += 1; break;
                    case "Green": green += 1; break;
                    case "Yellow": yellow += 1; break;
                }




            }

            //balancing formula -- based on idea that one of each is perfect. 


            if (red > System.Math.Round(chromo.Count * (red_allowance / 100)))
            {
                //for every extra red a penalty of 4 - 4 being the worst penalty
                int allowance = Convert.ToInt16(System.Math.Round(chromo.Count * (red_allowance / 100)));
                score = score - (allowance * 4);


            }

            if (yellow > System.Math.Round(chromo.Count * (yellow_allowance / 100)))
            {
                //for every extra red a penalty of 5 - 5 being the worst penalty                
                int allowance = Convert.ToInt16(System.Math.Round(chromo.Count * (yellow_allowance / 100)));
                score = score - (allowance * 3);
            }

            if (blue > System.Math.Round(chromo.Count * (blue_allowance / 100)))
            {
                //for every extra blue a penalty of 2
                int allowance = Convert.ToInt16(System.Math.Round(chromo.Count * (blue_allowance / 100)));
                score = score - (allowance * 2);

            }

            if (green > System.Math.Round(chromo.Count * (green_allowance / 100)))
            {
                //for every extra blue a penalty of 2
                int allowance = Convert.ToInt16(System.Math.Round(chromo.Count * (green_allowance / 100)));
                score = score - (allowance * 2);

            }

            int penalties = ((red + blue) / 2 * 3) + ((red + green) / 2 * 8) + ((red + yellow) / 2 * 3);
            int penaltiesBlue = ((blue + green) / 2 * 5) + ((blue + yellow) / 2 * 6);
            int penaltiesGreen = ((green + yellow) / 2 * 3);
            score = score - (penalties + penaltiesBlue + penaltiesGreen);
            Console.WriteLine(chromo.Id + "Fitness: " + score);
            return score;
        }

        public static bool genTerminate(Population population, int currentGeneration, long currentEvaluation)
        {
            if (currentGeneration > 40)
            {
                message = elite.Count != 0 ? message : "No Structure Found";
                return true;
            }
            return false;
        }


        private static void genA_OnRunComplete(object sender, GaEventArgs e)
        {
            KeyValuePair<List<KeyValuePair<Chromosome, double>>, double> highest = elite.MaxBy(t => t.Value);

            List<Student> groupList = new List<Student>();
            foreach (KeyValuePair<Chromosome, double> kvp in highest.Key)
            {

                foreach (Gene g in kvp.Key)
                {
                    groupList.Add(getStudentByNum(g.ObjectValue.ToString()));

                }

                solutions.Add(new KeyValuePair<List<Student>, double>(groupList, kvp.Key.Fitness));
                groupList = new List<Student>();

            }
            solutionAverage = highest.Value;



        }

        private static void genA_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var studentsQueue = new Queue<Student>(students);
            List<String> existingStudents = new List<String>();
            List<KeyValuePair<List<KeyValuePair<Chromosome, double>>, double>> groupStructures = new List<KeyValuePair<List<KeyValuePair<Chromosome, double>>, double>>();
            List<KeyValuePair<Chromosome, double>> group = new List<KeyValuePair<Chromosome, double>>();

            Chromosome highFitness = null;
            double average = 0;
            List<string> strings = students.Select(s => (string)s.getStudentNum()).ToList();


            for (int j = 0; j < students.Count; j++)
            {

                foreach (Student stud in studentsQueue)
                {
                    if ((students.Count - existingStudents.Count) == groupsize)
                    {

                        List<string> trial = strings.Except(existingStudents).ToList();
                        Chromosome chromo = new Chromosome();
                        foreach (string st in trial)
                        {
                            chromo.Genes.Add(new Gene(st));
                        }
                        chromo.Evaluate(calculateFitness);
                        group.Add(new KeyValuePair<Chromosome,double>(chromo, chromo.Fitness));
                        break;
                    }

                    // if (existingStudents.Count == students.Count) { break; }

                    if (existingStudents.Contains(stud.getStudentNum()) == false)
                    {
                        foreach (Chromosome c in e.Population.Solutions)
                        {
                            List<String> words = c.ToString().Split(' ').ToList();
                            if (existingStudents.Any(item => words.Contains(item)) == false & words.Contains(stud.getStudentNum()) == true)
                            {

                                if (highFitness == null || c.Fitness > highFitness.Fitness)
                                {

                                    highFitness = c;

                                }

                            }
                        }

                        if (highFitness != null)
                        {

                            foreach (Gene s in highFitness)
                            {
                                Student st = getStudentByNum(s.ObjectValue.ToString());
                                existingStudents.Add(st.getStudentNum());

                            }

                            group.Add(new KeyValuePair<Chromosome, double>(highFitness, highFitness.Fitness));
                            highFitness = null;
                        }

                    }

                }

                foreach (KeyValuePair<Chromosome,double> c in group)
                {
                    average += c.Value;
                }

                
                average = average / group.Count;
                


                if (groupStructures.Any(x => x.Key.ToString() == group.Any().ToString()) == false)
                {


                    groupStructures.Add(new KeyValuePair<List<KeyValuePair<Chromosome,double>>,double>(group,average));

                }

                average = 0;
                group = new List<KeyValuePair<Chromosome, double>>();
                existingStudents.Clear();



                studentsQueue.Enqueue(studentsQueue.Dequeue());
            }

            //writeToFile(groupStructures);

            KeyValuePair<List<KeyValuePair<Chromosome,double>>, double> highest = groupStructures.MaxBy(t => t.Value);

            elite.Add(new KeyValuePair<List<KeyValuePair<Chromosome, double>>, double>(highest.Key, highest.Value));


        }



        private static Student getStudentByNum(string sNum)
        {
            Student stud = null;
            foreach (Student s in students)
            {
                if (s.getStudentNum() == sNum)
                {
                    stud = s;
                    break;
                }

            }
            return stud;
        }


        private static void writeToFile(List<KeyValuePair<List<Chromosome>,double>> groups)
        {
            string path = @"MyTest.txt";
            string lines = "\n Group Structure: ";

           
            foreach (KeyValuePair<List<Chromosome>, double> kvp in groups)
            {
                foreach (Chromosome chr in kvp.Key)
                {
                    lines += chr.ToString();
                }

                lines += " Average: " + kvp.Value + '\n'; 
            }

            if (!File.Exists(path))
            {
                
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(lines);
                    sw.Close();
                   
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(lines);
                    sw.Close();
                   
                }	
            }
         

            
            

            
        }

        public int getStudentSize()
        {
            return students.Count;
        }
    }
}

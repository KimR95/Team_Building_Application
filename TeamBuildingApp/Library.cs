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

namespace TeamBuildingApp
{
    class Library
    {

        private static Library instance;
        private static DBConnection dbC;
        private static List<Question> questions;
        
        private static List<Student> students;
        private static SortedList<List<Chromosome>, double> elite;

        private int red_allowance;
        private int blue_allowance;
        private int green_allowance;
        private int yellow_allowance;

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
            elite = new SortedList<List<Chromosome>, double>();
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

        public Student checkCode(string no, string fname, string sname, string classcode)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM CLASSCODES WHERE CLASS_CODE = '" + classcode + "'", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();

            if (read.HasRows)
            {
                //format new student into type and store on db
                read.Close();
                stud = new Student(no, fname, sname, classcode, dbC.getConnection());

                return stud;
            }
            else
            {
                read.Close();
                return null;
            }


        }

        public void completedResults(int red, int blue, int green, int yellow)
        {
            stud.updateResults(red, blue, green, yellow);
        }

        public void logOut()
        {
            instance = null;
        }
        
        public void setAllowance(int red, int blue, int green, int yellow)
        {
            this.red_allowance = red;
            this.blue_allowance = blue;
            this.green_allowance = green;
            this.yellow_allowance = yellow;
        }

        public void generateGroups(string classCode, int groupSize)
        {
            //get students from the db
            List<Student> students = createStudents(classCode);

            //create population from the available students
            Population pop = createPopulation(students, groupSize);

            //create operators & algorithm
            createOperatorsAndAlgorithm(pop);



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
                    if (j <= groupSize)
                    {
                        chromo.Genes.Add(new Gene(students[k]));
                        j = chromo.Count;
                    }
                    else
                    {
                        break;
                    }
                }

                pop.Solutions.Add(chromo);
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
            genA.Operators.Add(el);
            genA.Operators.Add(cr);
            genA.Operators.Add(mutate);

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
            foreach (Student stud in chromo)
            {
                switch (stud.getPrimary())
                {
                    case "Red": red += 1; break;
                    case "Blue": blue += 1; break;
                    case "Green": green += 1; break;
                    case "Yellow": yellow += 1; break;
                }
            }

            //balancing formula -- based on idea that one of each is perfect. 

            if (red > chromo.Count * (red_allowance / 100))
            {
                //for every extra red a penalty of 4 - 4 being the worst penalty
                score = score - (red - chromo.Count * (red_allowance / 100) * 4);
            }

            if (yellow > chromo.Count * (yellow_allowance / 100))
            {
                //for every extra red a penalty of 5 - 5 being the worst penalty
                score = score - (yellow - chromo.Count * (yellow_allowance / 100) * 3);
            }

            if (blue > chromo.Count * (blue_allowance / 100))
            {
                //for every extra blue a penalty of 2
                score = score - (blue - chromo.Count * (blue_allowance / 100) * 2);
            }

            if (green > chromo.Count * (green_allowance / 100))
            {
                //for every extra blue a penalty of 2
                score = score - (green - chromo.Count * (green_allowance / 100) * 2);
            }


            return 2.0;
        }

        public static bool genTerminate(Population population, int currentGeneration, long currentEvaluation)
        {
            if (currentGeneration > 400)
            {
                return true;
            }
            return false;
        }


        private static void genA_OnRunComplete(object sender, GaEventArgs e)
        {

        }

        private static void genA_OnGenerationComplete(object sender, GaEventArgs e)
        {
            List<Student> existingStudents = new List<Student>();
            SortedList<List<Chromosome>, double> groupStructures = new SortedList<List<Chromosome>, double>();
            List<Chromosome> group = new List<Chromosome>();

            Chromosome highFitness = null;


            double average = 0;

            for (int j = 0; j < students.Count - 1; j++)
            {
                for (int i = j; i < students.Count - 1; i++)
                {
                    if (existingStudents.Contains(students[i])) { break; }

                    foreach (Chromosome c in e.Population.Solutions)
                    {
                        foreach (Student g in c)
                        {
                            if (g == students[i])
                            {
                                if (highFitness == null || c.Fitness > highFitness.Fitness)
                                {
                                    highFitness = c;

                                }
                            }

                        }
                    }

                    foreach (Student s in highFitness)
                    {
                        existingStudents.Add(s);
                    }

                    group.Add(highFitness);

                }


                foreach (Chromosome c in group)
                {
                    average += c.Fitness;
                }

                average = average / group.Count;

                groupStructures.Add(group, average);


            }


            var gS = groupStructures.OrderBy(v => v.Value);

            foreach (KeyValuePair<List<Chromosome>, double> pair in gS)
            {
                elite.Add(pair.Key, pair.Value);
            }
        }
    }
}


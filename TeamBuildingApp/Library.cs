using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Collections;

namespace TeamBuildingApp
{
    class Library
    {

        private static Library instance;
        private static DBConnection dbC;
        private static List<Question> questions;
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
        }
        public String connectToDb()
        {
            try
            {
                dbC = new DBConnection();
                return "Connected";
            }
            catch(Exception ex)
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
            catch(Exception){}
        }

       
        public Administrator validateLogin(string username, string password)
        {
            
            MySqlCommand cmdUser = new MySqlCommand("SELECT * FROM ADMINISTRATORS WHERE USERNAME = '" + username + "'", dbC.getConnection());
            cmdUser.Prepare();       
            
            MySqlDataReader read = cmdUser.ExecuteReader();
            

           while(read.Read())
            {
                Administrator admin = new Administrator(read.GetString("First_Name"), read.GetString("Second_Name"), read.GetString("Email_Address"),
                    read.GetString("Username"),read.GetString("Company_Name"),read.GetString("Salt"), read.GetString("AdminPassword"));

                read.Close();
                
                if(admin.checkPassword(password))
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
            if(read.HasRows)
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
            catch(Exception ex)
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

            if(read.HasRows)
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
            catch(MySqlException ex)
            {
                return ex.Message;
            }

            return "The code for " + classTitle + " is: " + code;


        }

        public List<Question> getQuestionSelection()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM QUESTIONS ORDER BY RAND() LIMIT 10", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();
        
            while(read.Read())
            {
                
                questions.Add(new Question(read.GetString("Question_ID"),read.GetString("Question"), read.GetString("Red_Answer"), read.GetString("Blue_Answer"), read.GetString("Green_Answer"), read.GetString("Yellow_Answer")));
                
            }

            read.Close();
            return questions;
           
    
        }

        public Student checkCode(string no, string fname, string sname, string classcode)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM CLASSCODES WHERE CLASS_CODE = '" + classcode + "'", dbC.getConnection());
            MySqlDataReader read = cmd.ExecuteReader();
            
            if(read.HasRows)
            {
                //format new student into type and store on db
                read.Close();
                stud = new Student(no, fname, sname, classcode,dbC.getConnection());
                
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
    }
}

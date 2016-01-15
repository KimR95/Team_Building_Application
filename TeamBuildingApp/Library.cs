using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TeamBuildingApp
{
    class Library
    {

        private static Library instance;
        private static DBConnection dbC;

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
            dbC.DBDisconnect();
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

                //not sure whether this is stable - might break if
                read.Close();
                
                if(admin.checkPassword(password))
                {
                   
                    return admin;
                }
                return null;
                
            }

            return null; 
        }

        public String checkUsername(string username)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM ADMINISTRATORS WHERE USERNAME ='@val1'", dbC.getConnection());
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@val1", username);

            MySqlDataReader read = cmd.ExecuteReader();
            if(read.HasRows)
            {
                
                return "Exists";
            }
            else
            {
                return "Doesn't Exists";
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
    }
}

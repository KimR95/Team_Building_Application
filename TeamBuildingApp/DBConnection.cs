using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using MySql.Data.MySqlClient;


namespace TeamBuildingApp
{
    class DBConnection
    {


        private MySqlConnection connectionDb;

        public DBConnection()
        {                         
            try
            {
                connectionDb = new MySqlConnection("Server = us-cdbr-azure-central-a.cloudapp.net; Port = 3306; Database = TBA_Database; Uid = bd96a30c09f4d0; Pwd = ab148145");
                connectionDb.Open();   
            }
            catch(MySqlException e)
            {
                throw new Exception(e.Message);
            }

        }


        public void DBDisconnect()
        {
            this.connectionDb.Close();
        }

        public MySqlConnection getConnection()
        {
            return this.connectionDb;
        }
    }
}

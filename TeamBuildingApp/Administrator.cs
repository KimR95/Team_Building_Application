using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;


namespace TeamBuildingApp
{
    public class Administrator
    {

        private string fname;
        private string sname;
        private string emailAddress;
        private string username;
        private string password;
        private string companyName;
        private string salt;
        
        private MySqlConnection connect;
        
        
        public Administrator(string fn, string sn, string ea, string cName, string us, MySqlConnection ct)
        {
            //administrators before db store
            this.fname = fn;
            this.sname = sn;
            this.emailAddress = ea;
            this.companyName = cName;
            this.username = us;

            this.password = createPassword();

            this.connect = ct;

            this.storeToDb();

        }

        public Administrator(string fn, string sn, string ea, string us, string cn, string slt, string slthash)
        {
            //administrators from db store
            this.fname = fn;
            this.sname = sn;
            this.emailAddress = ea;
            this.username = us;
            this.salt = slt;
            this.password = slthash;
            this.companyName = cn;
            
        }

        public String createPassword()
        {
            //create password for new admin - default & encrypted
            string pass = fname + sname;
            this.salt = createSalt(30);
            return generateHash(pass,salt);
            
        }
        
        public void changePassword(string newpass)
        {
            //encryption of new password 
            this.salt = createSalt(30);
            this.password = generateHash(newpass, salt);
        }

        public void storeToDb()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO ADMINISTRATORS(FIRST_NAME, SECOND_NAME, EMAIL_ADDRESS, COMPANY_NAME, USERNAME, ADMINPASSWORD, SALT)" +
                "VALUES(@VAL1, @VAL2, @VAL3, @VAL4, @VAL5, @VAL6, @VAL7)", this.connect);
           
            cmd.Parameters.AddWithValue("@val1", this.fname);
            cmd.Parameters.AddWithValue("@val2", this.sname);
            cmd.Parameters.AddWithValue("@val3", this.emailAddress);
            cmd.Parameters.AddWithValue("@val4", this.companyName);
            cmd.Parameters.AddWithValue("@val5", this.username);
            cmd.Parameters.AddWithValue("@val6", this.password);
            cmd.Parameters.AddWithValue("@val7", this.salt);

            
            try
            {
                MySqlDataReader read = cmd.ExecuteReader();
                read.Close();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }
              
        
        public String createSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);


        }

        public String generateHash(String input, String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sha256hashString = new SHA256Managed();
            byte[] hash = sha256hashString.ComputeHash(bytes);

            return ByteArrayToHexString(hash);


        }

        private string ByteArrayToHexString(byte[] hash)
        {
            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public Boolean checkPassword(string pword)
        {
            if (this.password == generateHash(pword, salt))
            {
                return true;
            }
            return false;

        
        }

        public String getUsername()
        {
            return this.username;
        }

        public String getCompanyName()
        {
            return this.companyName;
        }
    }

}

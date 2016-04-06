using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;


namespace TeamBuildingApp
{
    class Student
    {
        string studentNo;
        string firstName;
        string secondName;
        string classCode;
        string primary;
        string secondary;
        SortedList<string, int> assessmentResults;
        MySqlConnection connect;


        public Student(string no, string fn, string sn, string classC, MySqlConnection cnt)
        {
            this.studentNo = no;
            this.firstName = fn;
            this.secondName = sn;
            this.classCode = classC;
            this.connect = cnt;

            this.assessmentResults = new SortedList<string, int>();
            
            
            storeToDb();
        }

        public Student(string no, string fn, string sn, string classC, int red, int blue, int green, int yellow, string pc, string sc)
        {
            this.studentNo = no;
            this.firstName = fn;
            this.secondName = sn;
            this.classCode = classC;
          
            this.assessmentResults = new SortedList<string, int>();

            if (pc == "NULL" || sc == "NULL")
            {
                this.setResults(red, blue, green, yellow);
            }

            else
            {
                this.primary = pc;
                this.secondary = sc;
            }


          
        }


        public void storeToDb()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO STUDENTS (STUDENT_NO, FIRST_NAME, SECOND_NAME, CLASS_CODE) VALUES ('"
                + this.studentNo + "','" + this.firstName + "','" + this.secondName + "','" + this.classCode + "')",this.connect);

            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();

        }

        public void updateResults(int red, int blue, int green, int yellow)
        {
            setResults(red, blue, green, yellow);                           
            

            MySqlCommand cmd = new MySqlCommand("UPDATE STUDENTS SET RED_RESULTS = @val1, BLUE_RESULTS =@val2, GREEN_RESULTS = @val3, YELLOW_RESULTS = @val4 WHERE STUDENT_NO = '@val5'",this.connect);
            cmd.Parameters.AddWithValue("@val1", red);
            cmd.Parameters.AddWithValue("@val2", blue);
            cmd.Parameters.AddWithValue("@val3", green);
            cmd.Parameters.AddWithValue("@val4", yellow);
            cmd.Parameters.AddWithValue("@val5", this.studentNo);
            
            MySqlDataReader read = cmd.ExecuteReader();
            read.Close();


        }

        private void setResults(int red, int blue, int green, int yellow)
        {
            this.assessmentResults.Add("Red", red);
            this.assessmentResults.Add("Blue", blue);
            this.assessmentResults.Add("Green", green);
            this.assessmentResults.Add("Yellow", yellow);

            List<KeyValuePair<string, int>> sorted = this.assessmentResults.OrderBy(v => v.Value).ToList();

            primary = sorted[0].Key;
            secondary = sorted[1].Key;
            

            
        }

        public String getPrimary()
        {
            return this.primary;
        }

        public String getSecondary()
        {
            return this.secondary;
        }

        public String getStudentNum()
        {
            return this.studentNo;
        }

        public String getName()
        {
            return this.firstName + " " + this.secondName; 
        }

      
    
    }
}

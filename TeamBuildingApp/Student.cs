﻿using System;
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

        public Student(string no, string fn, string sn, string classC, int red, int blue, int green, int yellow)
        {
            this.studentNo = no;
            this.firstName = fn;
            this.secondName = sn;
            this.classCode = classC;
          
            this.assessmentResults = new SortedList<string, int>();
            this.setResults(red, blue, green, yellow);


          
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
            

            MySqlCommand cmd = new MySqlCommand("INSERT INTO STUDENTS(RED_RESULTS, BLUE_RESULTS, GREEN_RESULTS, YELLOW_RESULTS) VALUES('"
                + red + "','" + blue + "','" + green + "','" + yellow + "')",this.connect);

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

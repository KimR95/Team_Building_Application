using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBuildingApp
{
    class Question
    {
        string id;
        string question;
        string redAnswer;
        string blueAnswer;
        string greenAnswer;
        string yellowAnswer;

        public Question(string ID,string q, string rA, string bA, string gA, string yA)
        {
            this.question = q;
            this.redAnswer = rA;
            this.blueAnswer = bA;
            this.greenAnswer = gA;
            this.yellowAnswer = yA;
            this.id = ID;
        }

        public String getQuestion()
        {
            return this.question;
        }

        public String getRedAnswer()
        {
            return this.redAnswer;
        }

        public String getBlueAnswer()
        {
            return this.blueAnswer;
        }

        public String getGreenAnswer()
        {
            return this.greenAnswer;
        }

        public String getYellowAnswer()
        {
            return this.yellowAnswer;
        }
    }
}

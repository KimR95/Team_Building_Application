using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web;

namespace TeamBuildingApp
{
    /// <summary>
    /// Interaction logic for winQuestionaire.xaml
    /// </summary>
    public partial class winQuestionaire : Window
    {
        Library lib;        
        List<Question> questions = new List<Question>();
        List<KeyValuePair<String, String>> answers;

        //loop increment
        int i;
        
        //Results
        int red;
        int blue;
        int green;
        int yellow;
        string selectedAnswer;


        public winQuestionaire()
        {
            InitializeComponent();
            
            lib = Library.Instance;
            questions = lib.getQuestionSelection();
            answers = new List<KeyValuePair<string, string>>();
            i = -1;

                        
        }



        private void getNextQuestion()
        {
            
           
            if (questions != null)
            {
                i += 1;
                answers.Clear();
                tbQuestion.Text = "";
                tbAnswerA.Text = "";
                tbAnswerB.Text = "";
                tbAnswerC.Text = "";
                tbAnswerD.Text = ""; 

                //get the corresponding answers
                answers.Add(new KeyValuePair<string, string>("Red", questions[i].getRedAnswer()));                
                answers.Add(new KeyValuePair<string,string>("Blue", questions[i].getBlueAnswer()));
                answers.Add(new KeyValuePair<string, string>("Green", questions[i].getGreenAnswer()));
                answers.Add(new KeyValuePair<string,string>("Yellow", questions[i].getYellowAnswer()));

                Random rnd = new Random();                
                answers = answers.OrderBy(x => rnd.Next()).ToList();

                tbQuestion.Text = questions[i].getQuestion();
                tbAnswerA.Text = answers[0].Value;
                tbAnswerB.Text = answers[1].Value;
                tbAnswerC.Text = answers[2].Value;
                tbAnswerD.Text = answers[3].Value;

                progBarQuestions.Value = progBarQuestions.Value + 1;
                
            }


        }
       

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            //hide components 
            lblTitle.Visibility = System.Windows.Visibility.Hidden;
            lblRules.Visibility = System.Windows.Visibility.Hidden;
            btnBegin.Visibility = System.Windows.Visibility.Hidden;

            //show components for questions
            progBarQuestions.Visibility = System.Windows.Visibility.Visible;
            tbQuestion.Visibility = System.Windows.Visibility.Visible;
            btnAnswerA.Visibility = System.Windows.Visibility.Visible;
            tbAnswerA.Visibility = System.Windows.Visibility.Visible;
            btnAnswerB.Visibility = System.Windows.Visibility.Visible;
            tbAnswerB.Visibility = System.Windows.Visibility.Visible;
            btnAnswerC.Visibility = System.Windows.Visibility.Visible;
            tbAnswerC.Visibility = System.Windows.Visibility.Visible;
            btnAnswerD.Visibility = System.Windows.Visibility.Visible;
            tbAnswerD.Visibility = System.Windows.Visibility.Visible;
            btnConfirm.Visibility = System.Windows.Visibility.Visible;

            getNextQuestion();            
        }

        

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            
            TextBlock tb = this.FindName(selectedAnswer) as TextBlock;
            //save result
            foreach(KeyValuePair<string, string> kvp in answers)
            {
                if(kvp.Value == tb.Text)
                {
                    if (kvp.Key == "Red")
                    {
                        this.red += 1;
                    }
                    else if(kvp.Key == "Blue")
                    {
                        this.blue += 1;
                    }
                    else if (kvp.Key == "Green")
                    {
                        this.green += 1;
                    }
                    else
                    {
                        this.yellow += 1;
                    }
                }


            }
            
            if (progBarQuestions.Value != progBarQuestions.Maximum)
            {
                //Clear the previous answer 
                clearSelection();             

                //display the next question     

                getNextQuestion();
            }
            else
            {
                lib.completedResults(this.red, this.blue, this.green, this.yellow);

                MessageBox.Show("Completed. Results have been sent to your administrator", "Completion", MessageBoxButton.OK, MessageBoxImage.Information);

                lib.logOut();
                winLogin wl = new winLogin();
                this.Close();
                wl.Show();
                
    
            }
        }


       
        
        //Selection Focus Events
        private void btnAnswerA_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.selectedAnswer = "tbAnswerA";
            var bc = new BrushConverter();

            tbAnswerA.Background = (Brush)bc.ConvertFrom("#FFC5D7F8");
            tbAnswerA.FontWeight = FontWeights.Bold;

        }

        private void btnAnswerB_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.selectedAnswer = "tbAnswerB";
            var bc = new BrushConverter();

            tbAnswerB.Background = (Brush)bc.ConvertFrom("#FFC5D7F8");
            tbAnswerB.FontWeight = FontWeights.Bold;
        }

        private void btnAnswerC_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.selectedAnswer = "tbAnswerC";
            var bc = new BrushConverter();

            tbAnswerC.Background = (Brush)bc.ConvertFrom("#FFC5D7F8");
            tbAnswerC.FontWeight = FontWeights.Bold;
        }

        private void btnAnswerD_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.selectedAnswer = "tbAnswerD";
            var bc = new BrushConverter();

            tbAnswerD.Background = (Brush)bc.ConvertFrom("#FFC5D7F8");
            tbAnswerD.FontWeight = FontWeights.Bold;
        }
        

             private void clearSelection()
        {

            tbAnswerA.Background = new SolidColorBrush(Colors.White);
            tbAnswerA.FontWeight = FontWeights.Normal;
            tbAnswerB.Background = new SolidColorBrush(Colors.White);
            tbAnswerB.FontWeight = FontWeights.Normal;
            tbAnswerC.Background = new SolidColorBrush(Colors.White);
            tbAnswerC.FontWeight = FontWeights.Normal;
            tbAnswerD.Background = new SolidColorBrush(Colors.White);
            tbAnswerD.FontWeight = FontWeights.Normal;
        }

    }
}

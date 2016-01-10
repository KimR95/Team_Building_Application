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

namespace TeamBuildingApp
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {
        public winLogin()
        {
            InitializeComponent();
        }

       

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            //hide buttons
            btnAdmin.Visibility = System.Windows.Visibility.Hidden;
            btnStudent.Visibility = System.Windows.Visibility.Hidden;

            //show log in components 
            txtAdminUsername.Visibility = System.Windows.Visibility.Visible;
            passPassword.Visibility = System.Windows.Visibility.Visible;
            btnEnter.Visibility = System.Windows.Visibility.Visible;
            btnBack.Visibility = System.Windows.Visibility.Visible;
            lblPass.Visibility = System.Windows.Visibility.Visible;
           

            lblDesc.Content = "Administrator Login";
            lblDesc.Visibility = System.Windows.Visibility.Visible;

        }

        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            //hide buttons
            btnAdmin.Visibility = System.Windows.Visibility.Hidden;
            btnStudent.Visibility = System.Windows.Visibility.Hidden;

            //show student components
            lblStudentLogin.Visibility = System.Windows.Visibility.Visible;
            txtClassCode.Visibility = System.Windows.Visibility.Visible;
            btnEnterS.Visibility = System.Windows.Visibility.Visible;
            btnBack.Visibility = System.Windows.Visibility.Visible;
            txtStudentName.Visibility = System.Windows.Visibility.Visible;
            txtStudentNo.Visibility = System.Windows.Visibility.Visible;
            txtStudentSecondName.Visibility = System.Windows.Visibility.Visible;

           
            
            lblDesc.Content = "Student Login";
            lblDesc.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            winLogin wl = new winLogin();
            wl.Show();
            this.Close();
            

        }

        private void btnEnterS_Click(object sender, RoutedEventArgs e)
        {
           
            winQuestionaire wq = new winQuestionaire();
            this.Close();
            wq.Show();
        }

       

      

        private void txtStudentName_MouseEnter(object sender, MouseEventArgs e)
        {

            txtStudentName.Clear();
            txtStudentName.FontStyle = FontStyles.Normal;
            txtStudentName.Foreground.Opacity = 100;


        }

        private void txtAdminUsername_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            txtAdminUsername.Clear();
            txtAdminUsername.FontStyle = FontStyles.Normal;
            txtAdminUsername.Foreground.Opacity = 100;
        }

        private void txtStudentNo_MouseEnter(object sender, MouseEventArgs e)
        {
            txtStudentNo.Clear();
            txtStudentNo.FontStyle = FontStyles.Normal;
            txtStudentNo.Foreground.Opacity = 100;
        }

        private void txtStudentSecondName_MouseEnter(object sender, MouseEventArgs e)
        {
            txtStudentSecondName.Clear();
            txtStudentSecondName.FontStyle = FontStyles.Normal;
            txtStudentSecondName.Foreground.Opacity = 100;
        }

        private void lblPass_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            lblPass.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            //Login Validation 

            //Navigation 
            winAdministrator wa = new winAdministrator();
            this.Close();
            wa.Show();
        }

       
    }
}

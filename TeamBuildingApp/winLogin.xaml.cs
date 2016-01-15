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
        Library lib;
        Administrator admin;
        string checkNo;
        string checkfn;
        string checksn;

        public winLogin()
        {
            InitializeComponent();
            lib = Library.Instance;
            admin = null;
            
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
            if(checkNo != "check" && txtStudentNo.Text != "" 
                && checkfn != "check" && txtStudentName.Text != "" 
                    && checksn != "check" && txtStudentSecondName.Text != "" && txtClassCode.Text != "")
            {
                if(lib.checkCode(txtStudentNo.Text, txtStudentName.Text, txtStudentSecondName.Text, txtClassCode.Text)!= null)
                {
                    winQuestionaire wq = new winQuestionaire();
                    this.Close();
                    wq.Show();
                }
                else
                {
                    MessageBox.Show("Invalid class code, contact administrator.", "Invalid Code", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ensure all details have been entered", "Input Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                checkNo = "";
                checkfn = "";
                checksn = "";
            }
           
           
        }

       

      

        private void txtStudentName_MouseEnter(object sender, MouseEventArgs e)
        {

            checkfn = "check";
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
            checkNo = "check";
            txtStudentNo.Clear();
            txtStudentNo.FontStyle = FontStyles.Normal;
            txtStudentNo.Foreground.Opacity = 100;
        }

        private void txtStudentSecondName_MouseEnter(object sender, MouseEventArgs e)
        {
            checksn = "check";
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
            admin = lib.validateLogin(txtAdminUsername.Text, passPassword.Password);

            if (admin != null)
            {
                //Navigation 
                winAdministrator wa = new winAdministrator(admin);
                this.Close();
                wa.Show();
            }
            else
            {
                MessageBox.Show("Crendentials are incorrect", "Invalid Login", MessageBoxButton.OK, MessageBoxImage.Error);
                txtAdminUsername.Clear();
                passPassword.Clear();
            }
        }

       
    }
}

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
    /// Interaction logic for winNewAdmin.xaml
    /// </summary>
    public partial class winNewAdmin : Window
    {
        Library lib;
        Boolean usernameCheck;

        public winNewAdmin()
        {
            InitializeComponent();
            lib = Library.Instance;
            
        }

  

        private void txtUsername_MouseLeave(object sender, MouseEventArgs e)
        {
            
            if(txtUsername.Text != "")
            {
                usernameCheck = lib.checkUsername(txtUsername.Text);
                
                //if username is free
                if (usernameCheck == false)
                {
                    imgTick.Visibility = System.Windows.Visibility.Visible;
                    imgCross.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    imgTick.Visibility = System.Windows.Visibility.Hidden;
                    imgCross.Visibility = System.Windows.Visibility.Visible;
                    
                }
            }
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text != "")
            {
                usernameCheck = lib.checkUsername(txtUsername.Text);

                //if username is free
                if (usernameCheck == false)
                {
                    imgTick.Visibility = System.Windows.Visibility.Visible;
                    imgCross.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    imgTick.Visibility = System.Windows.Visibility.Hidden;
                    imgCross.Visibility = System.Windows.Visibility.Visible;

                }
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string message = ""; 
            if (usernameCheck == false)
            {
                message = lib.addAdministrator(txtFName.Text, txtSName.Text, txtEmailAddress.Text, txtCompanyName.Text, txtUsername.Text);
            }

            MessageBox.Show(message);
        }
    }
}

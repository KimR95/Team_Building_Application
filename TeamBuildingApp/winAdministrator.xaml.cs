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
    /// Interaction logic for winAdministrator.xaml
    /// </summary>
    public partial class winAdministrator : Window
    {
        private Administrator admin;
        Library lib;
        
        public winAdministrator(Administrator ad)
        {
            InitializeComponent();
            this.admin = ad;
            lib = Library.Instance;
           
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            winLogin wl = new winLogin();
            this.Close();
            wl.Show();
        }

        private void btnNewAdmin_Click(object sender, RoutedEventArgs e)
        {
            winNewAdmin wNA = new winNewAdmin();
            this.Close();
            wNA.Show();
        }

        private void btnGenerateCode_Click(object sender, RoutedEventArgs e)
        {
            winGenerateCode wGA = new winGenerateCode(this.admin);
            wGA.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            lib.setAllowance(25, 25, 25, 25);
            lib.generateGroups("k52zoxolck", 5);

        }
    }
}

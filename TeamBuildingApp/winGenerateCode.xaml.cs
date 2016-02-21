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
    /// Interaction logic for winGenerateCode.xaml
    /// </summary>
    public partial class winGenerateCode : Window
    {
        Library lib;
        Administrator admin;

        public winGenerateCode(Administrator ad)
        {
            InitializeComponent();
            lib = Library.Instance;
            this.admin = ad;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (txtClassName.Text != "")
            {
                string message = lib.generateClassCode(this.admin, txtClassName.Text);

                MessageBox.Show(message, "Generate Class Code", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Must Enter A Class Name", "Invalid Class Name", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

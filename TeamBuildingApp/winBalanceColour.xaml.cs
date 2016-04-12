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
    /// Interaction logic for winBalanceColour.xaml
    /// </summary>
    public partial class winBalanceColour : Window
    {
        Library lib;
        
        public winBalanceColour()
        {
           
            InitializeComponent();
            lib = Library.Instance;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            lib.setAllowance(sliderRed.Value, sliderBlue.Value, sliderGreen.Value, sliderYellow.Value);
            MessageBox.Show("The colour allowances have been set.");
            this.Close();
        }

        private void lblHelp_Click(object sender, RoutedEventArgs e)
        {
            winHelp help = new winHelp();
            help.Show();

        }

        
    }
}

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
    /// Interaction logic for winGroupManagement.xaml
    /// </summary>
    public partial class winGroupManagement : Window
    {
        Library lib;
        
        public winGroupManagement()
        {
            InitializeComponent();
            lib = Library.Instance;
        }

        private void btnGenerateStructures_Click(object sender, RoutedEventArgs e)
        {
            winInputDialogBox winInput = new winInputDialogBox(this);
            winInput.Show();
        }

           
    }
}

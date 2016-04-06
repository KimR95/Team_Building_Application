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
    /// Interaction logic for winViewAdmin.xaml
    /// </summary>
    public partial class winViewAdmin : Window
    {
        Administrator admin;
        Library lib;

        public winViewAdmin(Administrator a)
        {
            InitializeComponent();
            admin = a;
            lib = Library.Instance;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Administrator> adminL = lib.getAdministrators(admin);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(adminL);


            this.lstViewAdmin.Items.Clear();
            this.lstViewAdmin.ItemsSource = adminL;
        }
    }
}

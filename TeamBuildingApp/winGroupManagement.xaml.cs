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
using System.ComponentModel;

namespace TeamBuildingApp
{
    /// <summary>
    /// Interaction logic for winGroupManagement.xaml
    /// </summary>
    public partial class winGroupManagement : Window
    {
        Library lib;
        string mode;
        List<SolutionItem> solutionItems;
        CollectionView view;
        
        public winGroupManagement()
        {
            InitializeComponent();
            lib = Library.Instance;
            solutionItems = null;
           
          
        }

        private void btnGenerateStructures_Click(object sender, RoutedEventArgs e)
        {
            winInputDialogBox winInput = new winInputDialogBox(this);
            winInput.Show();
        }

        private void lstViewStudents_Selected(object sender, SelectionChangedEventArgs e)
        {

            if (mode == "Swap")
            {
                if (lstViewStudents.SelectedItems.Count != 1)
                {
                    if (lstViewStudents.SelectedItems.Count > 2)
                    {

                        System.Windows.MessageBox.Show("Only two items can be swapped at a time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        lstViewStudents.SelectedItems.Clear();
                    }
                    else if (lstViewStudents.SelectedItems.Count == 2)
                    {

                        SolutionItem v1 = (SolutionItem)lstViewStudents.SelectedItems[0];
                        SolutionItem v2 = (SolutionItem)lstViewStudents.SelectedItems[1];

                        string messageBox = "Do you wish to swap " + v1.StudentID + " " + v1.StudentName + " and " + v2.StudentID + " " + v2.StudentName + " ?";

                        if (MessageBox.Show(messageBox, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (v1.PColour == v2.PColour && v1.SColour == v2.SColour)
                            {
                                string v1GroupTitle = v1.GroupTitle;
                                string v2GroupTitle = v2.GroupTitle;

                                SolutionItem result1 = solutionItems.Single(s => s.StudentID == v1.StudentID);
                                SolutionItem result2 = solutionItems.Single(s => s.StudentID == v2.StudentID);

                                result2.GroupTitle = v1GroupTitle;
                                result1.GroupTitle = v2GroupTitle;

                                view.Refresh();
                                btnSwap_Click(sender, e);
                            }
                        }
                    }


                    lstViewStudents.UnselectAll();

                }
            }
        }

        public void displayList(List<SolutionItem> sItems)
        {
            this.solutionItems = sItems;

            view = (CollectionView)CollectionViewSource.GetDefaultView(solutionItems);

            view.GroupDescriptions.Add(new PropertyGroupDescription("GroupTitle"));
            view.SortDescriptions.Add(new SortDescription("GroupTitle", ListSortDirection.Ascending));
            
            this.lstViewStudents.Items.Clear();
            this.lstViewStudents.ItemsSource = solutionItems;

            this.Topmost = true;
        }

        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            if (this.mode != "Swap")
            {
                this.mode = "Swap";
                this.btnSwap.BorderBrush = Brushes.Crimson;
            }
            else
            {
                this.mode = "";
                btnSwap.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC5D7F8"));
            }
        }

        private void btnSetColourBalance_Click(object sender, RoutedEventArgs e)
        {
            winBalanceColour wBC = new winBalanceColour();
            wBC.Show();
            btnSetColourBalance.BorderBrush = Brushes.Crimson;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Would you like to save this group solution?", "Saving", MessageBoxButton.YesNo)== MessageBoxResult.Yes)
            {
                //Write to file - possible excel file.
                //Conditional Formatted template.
            }

            this.Close();
        }

     



     

       

           
    }
}

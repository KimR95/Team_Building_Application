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
    /// Interaction logic for winInputDialogBox.xaml
    /// </summary>
    public partial class winInputDialogBox : Window
    {
        Library lib;
        winGroupManagement winGroup;
        public winInputDialogBox(winGroupManagement wGM)
        {
            
            InitializeComponent();
            lib = Library.Instance;
            winGroup = wGM;
        }

        public string getClassCode()
        {
            return txtClassCode.Text;
        }
        public int getGroupSize()
        {
            return int.Parse(txtGroupSize.Text);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            

           string result = lib.generateGroups(this.getClassCode(), this.getGroupSize());
           lib.setAllowance(25, 25, 25, 25);
           //winGroup.Topmost = true;

           if (result == "Group Size Error")
           {
               MessageBox.Show("There are too many students for group size. Enter a group size suitable for " + lib.getStudentSize() + "students.");
               this.Topmost = true;
               this.txtGroupSize.Clear();
           }
           else if (result == "Class Code Error")
           {
               MessageBox.Show("The class code is invalid.");
               this.Topmost = true;
               this.txtClassCode.Clear();
           }
           else
           {

               List<KeyValuePair<List<Student>,double>> solved = lib.getSolutionsList();
               List<SolutionItem> sitems = new List<SolutionItem>();              


               int i =0;

               foreach (KeyValuePair<List<Student>, double> kvp in solved)
               {

                   i += 1;


                   foreach (Student stud in kvp.Key)
                   {

                       sitems.Add(new SolutionItem { StudentID = stud.getStudentNum(), StudentName = stud.getName(), PColour = stud.getPrimary(), SColour = stud.getSecondary(), GroupTitle = "Group " + i + "\t  Fitness Average: " + kvp.Value });
                   }

               }

         
               
               CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(sitems);
                         
               view.GroupDescriptions.Add(new PropertyGroupDescription("GroupTitle"));

               winGroup.lstViewTry.Items.Clear();
               winGroup.lstViewTry.ItemsSource = sitems;

               

               winGroup.Topmost = true;
               this.Close();

           }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TeamBuildingApp
{
    public class SolutionItem : INotifyPropertyChanged
    {
        public string StudentID { get; set; }

        public string StudentName { get; set; }

        public string PColour { get; set; }

        public string SColour { get; set; }

        public string GroupTitle { get; set; }
      

        public void changeGroupTitle(string cGT)
        {
            this.GroupTitle = cGT;
            NotifyPropertyChanged("GroupTitle");

        }
        public event PropertyChangedEventHandler PropertyChanged;




        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            System.Diagnostics.Debug.WriteLine("Update!"); //ok
            if (PropertyChanged != null)
            {
                //PropertyChanged is always null and shouldn't.
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
     



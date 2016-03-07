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
using System.Windows.Threading;
using System.ComponentModel;

namespace TeamBuildingApp
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {

       

        public Loading()
        {
            InitializeComponent();
            this.Show();
            progTick();
        }

        public void progTick()
        {
           
            if (progLoad.Value < progLoad.Maximum)
            {

                progLoad.Dispatcher.Invoke(() => progLoad.Value += 1, DispatcherPriority.Background);
                System.Threading.Thread.Sleep(2000);                

            }
            else
            {
              
                this.Close();
            }
        }

  

       
    }
}
        
                
             
            
            




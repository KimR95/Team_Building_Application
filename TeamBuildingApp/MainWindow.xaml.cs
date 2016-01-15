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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace TeamBuildingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Library lib;

        public MainWindow()
        {
            lib = Library.Instance;
            InitializeComponent();
            lib.disconnectDB();

            
            startUp();
            
            
        }

        private void startUp()
        {
            
            this.Show(); 
            //System.Threading.Thread.

            string connected = lib.connectToDb();
            if (connected == "Connected")
            {
                //progBar.Value += (progBar.Maximum / 4);
                Application.Current.Exit += new ExitEventHandler(applicationClose);

                //slow down progress bar for ui reasons
                pbTimings();
            }
            else
            {
                
                
                System.Threading.Thread.Sleep(3000);
                MessageBox.Show(connected, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
            
        

       

        private void pbTimings()
        {
            
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();     

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            progBar.Value += (progBar.Maximum / 4);

            if (progBar.Value == progBar.Maximum)
            {
                (sender as DispatcherTimer).Stop();
                
                winLogin wl = new winLogin();
                this.Close();
                wl.Show();
               
            }
        }

        private void applicationClose(object sender, EventArgs e)
        {
            lib.disconnectDB();
          
 
        }
 
       
           
        
    }
}

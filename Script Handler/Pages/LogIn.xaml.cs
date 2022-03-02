using Microsoft.Win32;
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

namespace Script_Handler.Pages
{
    /// <summary>
    /// Logica di interazione per LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        MainWindow mainwindow;
        public LogIn(MainWindow m)
        {
            mainwindow = m;
            InitializeComponent();
        }

        private void open_button_Click(object sender, RoutedEventArgs e)
        {
            string file = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                file = openFileDialog.FileName;
            }
            if(file != "")
            {
                mainwindow.mainframe.Content = new Pages.EditorPage(file, mainwindow);
            }
        }

        private void new_button_Click(object sender, RoutedEventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            bool exists = System.IO.Directory.Exists(workingDirectory + "\\repos");

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(workingDirectory + "\\repos");
            }

            mainwindow.mainframe.Content = new Pages.EditorPage(workingDirectory + "\\repos\\test.txt", mainwindow);

        }
    }
}

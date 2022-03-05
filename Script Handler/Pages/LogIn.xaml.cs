using Microsoft.Win32;
using Script_Handler.Objs;
using System;
using System.Collections.Generic;
using System.IO;
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
        const string ext = ".mike";
        MainWindow mainwindow;
        string new_file_name;
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
            else
            {
                MessageBox.Show("File not existing or wrong formatting");
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


            input_name_box.Visibility = Visibility.Visible;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            File.WriteAllText(workingDirectory + "\\repos\\" + new_file_name + ext, "row;null;Start writing!;#00afec");
            mainwindow.mainframe.Content = new Pages.EditorPage(workingDirectory + "\\repos\\" + new_file_name + ext, mainwindow);
        }

        private void name_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            new_file_name = name_box.Text;
        }
    }
}

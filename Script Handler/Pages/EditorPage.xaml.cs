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
    /// Logica di interazione per EditorPage.xaml
    /// </summary>
    public partial class EditorPage : Page
    {
        string file;
        MainWindow mainWindow;
        Objs.IO io;
        string[] script;
        string[] script_index;
        int pointer_to_script = -1;


        public EditorPage(string f, MainWindow m)
        {
            mainWindow = m;
            InitializeComponent();
            file = f;
            m.Title = file.Split("\\")[file.Split("\\").Length - 1];
            mainWindow.Height = 1000;
            mainWindow.Width = 1500;
            io = new Objs.IO(f);
            Update();
        }

        private async void ScriptBodyUpdate()
        {
            int chars = 0;
            double approx_len;
            for(int i = 0; i < script.Length; i++)
            {
                    chars += script[i].Length;
            }
            script_data.Text = "lines: " + script.Length + "\nChars: " + chars.ToString();
            help.Text = "\\{ name} --> name of person talking \n \\del {index} --> delete row index \n {index} - {text} --> substitute line index with text";
        }

        private async void Update()
        {
            (script_index, script) = await io.ReadAllFileAsync();
            ScriptBodyUpdate();
            script_view.ItemsSource = script_index;
        }

        private void input_box_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && input_box.Text.Length > 0)
            {
                if (isNumber(input_box.Text.Split(" ")[0]))
                {
                    int index = int.Parse(input_box.Text.Split(" ")[0]) - 1;
                    if (index > -1 && index < script.Length)
                    {
                        script[index + 1] = input_box.Text.Split((index + 1).ToString() + " - ")[1];
                    }
                    input_box.Text = "";
                    io.writeToFile(script);
                }else if(input_box.Text[0] == '\\')
                {
                    if(input_box.Text.Substring(1, 3) == "del" && isNumber(input_box.Text.Split(" ")[1]))
                    {
                        string[] script_temp = new string[script.Length - 1];

                        int pos0 = 0;
                        int pos1 = 0;
                        int pos2 = int.Parse(input_box.Text.Split(" ")[1]);

                        while (pos1 < script.Length)
                        {
                            if(pos1 != pos2)
                            {
                                script_temp[pos0] = script[pos1];
                                pos0++;
                                pos1++;
                            }
                            else
                            {
                                pos1++;
                            }
                        }
                        script = script_temp;
                        input_box.Text = "";
                        io.writeToFile(script);
                    }else if (isAChar(input_box.Text.Split(" ")[0].Substring(1, input_box.Text.Split(" ")[0].Length - 1)))
                    {
                        string name = input_box.Text.Split(" ")[0].Substring(1, input_box.Text.Split(" ")[0].Length - 1);
                        string res = name.ToUpper() + " >> " + input_box.Text.Substring(name.Length + 2, input_box.Text.Length - (name.Length + 2));
                        Task temp = io.writeToFileAsync(res);
                        input_box.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Your command \"" + input_box.Text + "\" was not recognized");
                    }
                }
                else
                {
                    Task temp = io.writeToFileAsync(input_box.Text);
                    input_box.Text = "";
                }
                Update();
                pointer_to_script = -1;
            }
            else if(e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if(pointer_to_script < 0 || pointer_to_script > script.Length)
                {
                    pointer_to_script = script.Length - 1;
                }
                input_box.Text = script[pointer_to_script--];
            }else if(e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (pointer_to_script < 0 || pointer_to_script > script.Length)
                {
                    pointer_to_script = script.Length - 1;
                }
                input_box.Text = script[pointer_to_script++];
            }
        }

        private bool isAChar(string value)
        {
            string[] charachters = char_box.Text.Split("\r\n");
            foreach(var c in charachters)
            {
                if(c == value)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isNumber(string v)
        {
            try
            {
                int.Parse(v);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void script_view_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                input_box.Text = (string)((ListViewItem)item).Content;
            }

        }
    }
}

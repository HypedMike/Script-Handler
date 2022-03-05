using Script_Handler.Objs;
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

        Script script;


        public EditorPage(string f, MainWindow m)
        {
            mainWindow = m;
            InitializeComponent();
            file = f;
            m.Title = file.Split("\\")[file.Split("\\").Length - 1];
            mainWindow.Height = 1000;
            mainWindow.Width = 1500;


            io = new Objs.IO(f);
            script = new Script(f);
            Update();
        }

        private void ScriptBodyUpdate()
        {
            /*int chars = 0;
            double approx_len;
            for(int i = 0; i < script.Length; i++)
            {
                    chars += script[i].Length;
            }
            script_data.Text = "lines: " + script.Length + "\nChars: " + chars.ToString();
            help.Text = "\\{ name} --> name of person talking \n \\del {index} --> delete row index \n {index} - {text} --> substitute line index with text";
            */
        }

        private void Update()
        {
            ScriptBodyUpdate();
            scriptviewupdater();
        }

        private void scriptviewupdater()
        {
            /* List<TextBlock> list = new List<TextBlock>();
            int char_len = 0;
            if (script.Length > 0)
            {
                char_len = int.Parse(script[0]);
            }
            script_index = Tools.SubArray(script_index, char_len + 1, script.Length - (char_len + 1));
            foreach (string si in script_index)
            {
                TextBlock temp = new TextBlock();
                temp.Text = si;
                string name = si.Split(" ")[2];
                if (isAChar(name))
                {
                    var converter = new System.Windows.Media.BrushConverter();
                    temp.Foreground = (Brush)converter.ConvertFromString("#00afec");
                    temp.Background = (Brush)converter.ConvertFromString("#FFFFFF90");
                }
                list.Add(temp);
            }*/

            script_view.ItemsSource = script.getTextBlocks();
        }

        private void input_box_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && input_box.Text.Length > 0)
            {

                // operations over the submitted input
                //
                // parser
                //


                // EDIT A ROW
                if (input_box.Text.Split(" ").Length > 2 && input_box.Text.Split(" ")[1] == "-" && isNumber(input_box.Text.Split(" ")[0]))
                {

                    // if 3 parts are detected
                    // if the second part is a '-'
                    // if the first part is a number

                    bool res = script.EditARow(int.Parse(input_box.Text.Split(" ")[0]), input_box.Text.Substring(4));

                    if (!res)
                    {
                        MessageBox.Show("Index is out of bounds");
                    }


                    // parser 2° lvl
                }else if(input_box.Text[0] == '\\' && input_box.Text.Length > 1)
                {

                    // if the first char is '\\'
                    // if the first char is not the only char


                    // DELETE A ROW
                    if(input_box.Text.Substring(1, 3).ToLower() == "del" && isNumber(input_box.Text.Split(" ")[1]))
                    {

                        // if the first part is 'del'
                        // if the second part is a number
                        // if the selected row is present in the script

                        bool res = script.DeleteARow(int.Parse(input_box.Text.Split(" ")[1]));
                        if (!res)
                        {
                            MessageBox.Show("Index out of limits", "Not cool, didnt laugh");
                        }
                        else
                        {
                            input_box.Text = "";
                        }

                    }
                    else
                    {

                        // it's a character

                        string name = input_box.Text.Substring(1).Split(" ")[0];
                        string[] output = {name, input_box.Text.Substring(name.Length + 1)};
                        script.AddARow(output);
                        input_box.Text = "";
                    }
                }
                else
                {

                    // it's a normal row

                    script.AddARow(input_box.Text);
                    input_box.Text = "";
                }
                Update();
            }
            else if(e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                input_box.Text = script.PointerUp();
            }else if(e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                input_box.Text = script.PointerDown();
            }
        }

        private bool isAChar(string value)
        {
            /*
            string[] charachters = char_box.Text.Split("\r\n");
            foreach(var c in charachters)
            {
                if(c.ToLower() == value.ToLower())
                {
                    return true;
                }
            }
            charachters = char_box.Text.Split("\n");
            foreach (var c in charachters)
            {
                if (c.ToLower() == value.ToLower())
                {
                    return true;
                }
            }
            */
            return true;
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

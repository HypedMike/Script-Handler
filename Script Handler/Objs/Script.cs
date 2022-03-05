using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Script_Handler.Objs
{
    public struct Line
    {
        public string actor;
        public string color;
        public string text;
        public int index;
    }

    public struct ActorBlock
    {
        public string name;
        public string color;
        public StackPanel item;
    }

    // type {act, row};name{name, null};text;color;

    public class Script
    {
        List<Line> lines;
        IO io;
        List<ActorBlock> actorBlocks;
        int pointer_to_script = 0;

        public int length = 0;


        public Script(string path)
        {
            lines = new List<Line>();
            io = new IO(path);
            getScript();
        }
        private void getScript()
        {
            lines = new List<Line>();
            actorBlocks = new List<ActorBlock>();
            string[] temp = io.ReadAllFile();
            int index = 0;
            foreach (string line in temp)
            {
                if (line != "" && line != "\n" && line != "\r" && line != "\r\n")
                {
                    lines.Add(lineBuilder(line, index));
                    index++;
                }
            }
            length = index + 1;
        }
        public List<TextBlock> getTextBlocks()
        {

            // gets the TextBlocks ready for the ListView source item

            getScript();
            List<TextBlock> textBlocks = new List<TextBlock>();
            var converter = new System.Windows.Media.BrushConverter();
            foreach (Line l in lines)
            {
                TextBlock temp = new TextBlock();
                if(l.actor != "null")
                {
                    temp.Text = l.index.ToString() + " - " + l.actor + " >> " + l.text;
                    temp.Background = (Brush)converter.ConvertFromString("#FFFFFF90");
                }
                else
                {
                    temp.Text = l.index.ToString() + " - " + l.text;
                }
                textBlocks.Add(temp);
            }
            return textBlocks;
        }
        public TextBlock getTextBlock(int i)
        {

            // gets the TextBlocks ready for the ListView source item

            getScript();
            Line l = lines[i];
            var converter = new System.Windows.Media.BrushConverter();
            TextBlock temp = new TextBlock();
            if (l.actor != "null")
            {
                temp.Text = l.index.ToString() + " - " + l.actor + " >> " + l.text;
                temp.Background = (Brush)converter.ConvertFromString("#FFFFFF90");
            }
            return temp;
        }

        public bool EditARow(int i, string value)
        {
            // edit row based on index

            if(i > length)
            {
                return false;
            }

            lines[i] = lineBuilder(value, i);

            return true;
        }
        public bool AddARow(params string[] args)
        {
            if(args.Length == 1)
            {
                // simple line
                io.writeToFile(string.Format("row;null;{0};DEFAULT", args[0]));
            }
            else if(args.Length > 3)
            {
                return false;
            }
            else
            {
                // 1 - name, 2 - text
                io.writeToFile(string.Format("act;{0};{1};{2}", args[0], args[1], "#00afec"));
            }
            getScript();
            return true;
        }
        public bool DeleteARow(int i)
        {
            if(i > lines.Count || i < 0)
            {
                return false;
            }
            lines.RemoveAt(i);
            io.writeToFile(LinesToString());
            getScript();
            return true;
        }


        public string PointerUp()
        {
            if(pointer_to_script > lines.Count)
            {
                pointer_to_script = 0;
            }
            return tostring(lines[pointer_to_script++]);
        }
        public string PointerDown()
        {
            if (pointer_to_script < 0)
            {
                pointer_to_script = lines.Count - 1;
            }
            return tostring(lines[pointer_to_script--]);
        }


        public List<StackPanel> getActors()
        {
            List<StackPanel> list = new List<StackPanel>();
            foreach (ActorBlock ab in actorBlocks)
            {
                list.Add(ab.item);
            }
            return list;
        }
        private string toTXT(Line line)
        {
            string type = "null";
            if(line.actor != "null") { type = line.actor; }
            return string.Format("{0};{1};{2};{3}", type, line.actor, line.text, line.color);
        }
        private string tostring(Line line)
        {
            string type = "null";
            if (line.actor != "null") { type = line.actor; }
            return string.Format("{0} - {1} >> {2}", line.index, line.actor, line.text);
        }
        private Line lineBuilder(string line, int i)
        {
            Line l = new Line();

            string[] lparsed = line.Split(";");

            if (lparsed[0] == "act")
            {
                l.color = lparsed[3];
                l.actor = lparsed[1];
            }
            else
            {
                l.actor = "null";
            }

            l.text = lparsed[2];
            l.index = i;


            return l;
        }
        private int actorBuilder(string line)
        {

            // creates an actorblock with name, color and stackpanel

            string[] lparsed = line.Split(";");
            foreach (string s in lparsed)
            {
                if (lparsed[1] == s)
                {
                    return 0;
                }
            }

            ActorBlock ab = new ActorBlock();
            ab.name = lparsed[1];
            ab.color = lparsed[3];
            StackPanel sp = new StackPanel();
            TextBlock tbl = new TextBlock();
            TextBox tb = new TextBox();

            sp.Orientation = Orientation.Horizontal;
            tbl.Text = lparsed[1];
            tb.Name = lparsed[1] + "ID";
            tb.Text = lparsed[3];

            sp.Children.Add(tbl);
            sp.Children.Add(tb);

            ab.item = sp;


            return 0;
        }
        private string[] LinesToString()
        {
            List<string> listring = new List<string>();
            foreach(Line l in lines)
            {
                listring.Add(toTXT(l));
            }
            return listring.ToArray();
        }
    }
}

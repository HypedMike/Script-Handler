using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script_Handler.Objs
{
    internal class IO
    {
        string path;
        int init = 0;
        public IO(string f)
        {
            path = f;
        }
        
        public void writeToFile(string row)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                if(init == 0)
                {
                    sw.WriteLine(row);
                    init++;
                }
                else
                {
                    sw.WriteLine(row + "\n");
                }
            }
        }
        public void writeToFile(string[] script)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach(string s in script)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public string[] ReadAllFile()
        {
            List<string> res_without_index = new List<string>();
            string[] temp = File.ReadAllLines(path);
            for (int i = 0; i < temp.Length; i++)
            {
                res_without_index.Add(temp[i]);
            }
            return res_without_index.ToArray();
        }

        public async Task<(string[], string[])> ReadAllFileAsync()
        {
            List<string> res = new List<string>();
            List<string> res_without_index = new List<string>();
            string[] temp = await File.ReadAllLinesAsync(path);
            for(int i = 0; i < temp.Length; i++)
            {
                res.Add(i.ToString() + " - " + temp[i]);
                res_without_index.Add(temp[i]);
            }
            return (res.ToArray(), res_without_index.ToArray());
        }
    }
}

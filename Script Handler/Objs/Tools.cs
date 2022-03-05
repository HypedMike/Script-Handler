using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script_Handler.Objs
{
    public static class Tools
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static bool checkFileFormat(string path)
        {
            string[] script = File.ReadAllLines(path);
            int length = 0;
            try
            {
                length = int.Parse(script[0]);
            }
            catch
            {
                return false;
            }
            for(int i = 1; i < length; i++)
            {
                if(script[i] == "\n")
                {
                    return false;
                }
            }
            if(script[length] != "\n")
            {
                return false;
            }
            return true;
        }
    }
}

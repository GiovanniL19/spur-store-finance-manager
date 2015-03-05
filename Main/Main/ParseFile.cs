using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Main
{
    class ParseFile
    {
        public static List<string[]> parseCSVFile(string path)
        {
            List<string[]> output = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    
                    while ((line = readFile.ReadLine()) != null)
                    {
                        output.Add(line.Split(','));
                    }
                }
            }
            catch (FileFormatException e)
            {
                Console.WriteLine(e);
            }

            return output;
        }
    }
}

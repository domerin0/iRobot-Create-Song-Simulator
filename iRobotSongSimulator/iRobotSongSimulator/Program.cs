using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace iRobotSongSimulator
{
    class Program
    {

        private static void playNote(double frequency, int duration)
        {
            //convert 1/64th seconds to millis
            int length = Convert.ToInt32(Math.Floor(duration * (1.0 / 64) * 1000));
            Console.Beep(Convert.ToInt32(Math.Round(frequency, 0, MidpointRounding.ToEven)), length);
        }

        static void Main(string[] args)
        {
            var section = (Hashtable)ConfigurationManager.GetSection("notes");
            Dictionary<int, double> dictionary = section.Cast<DictionaryEntry>().ToDictionary(d => Int32.Parse((string)d.Key), d => double.Parse((string)d.Value));
            while (true)
            {
                Console.WriteLine(section["Reset"]);
                string input = Console.ReadLine();
                string[] cmd = input.Split(' ');
                switch (cmd[0])
                {
                    case "song":
                        if (cmd.Length % 2 == 1)
                            fromConsole(cmd, dictionary);
                        else
                            Console.WriteLine("Incorrect number of arguments (need even number).");
                        break;
                    case "file":
                        if (cmd.Length == 2)
                            fromFile(cmd[1], dictionary);
                        else
                            Console.WriteLine("Incorrect number of arguments (need 1).");
                        break;
                    case "help":
                        Console.WriteLine("Commands are: song [note(32-127)] [duration(in 1/64th seconds)]\n and file [filename(relative or absolute)]");
                        break;
                    default:
                        Console.WriteLine("Incorrect command type 'help' for commmand list.");
                        break;
                }

            }

        }

        private static void fromFile(string filename, Dictionary<int, double> dic)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] splitLines = line.Split(' ');
                double freq = dic[Int32.Parse(splitLines[0])];
                int length = Int32.Parse(splitLines[1]);
                playNote(freq, length);
            }
            Console.Write("Finished.");
        }

        private static void fromConsole(string[] args, Dictionary<int, double> dic)
        {
            for (int i = 1; i < args.Length - 1; i += 2)
            {
                int key = Int32.Parse(args[i]);
                if (dic.ContainsKey(key))
                {
                    double freq = dic[key];
                    int duration = Int32.Parse(args[i + 1]);
                    playNote(freq, duration);
                }
            }
            Console.Write("Finished.");
        }

        /* string outerTag = "<DeviceSettings>\n{0}</DeviceSettings>";
 string innerTag = "<notes>\n{0}</notes>";
 string item = "<add key=\"{0}\" value=\"{1}\"/>\n";
 StringBuilder items = new StringBuilder();
 string[] lines = System.IO.File.ReadAllLines("test.txt");
 foreach (var line in lines)
 {
     string[] splitLines = line.Split(' ');
     string key = splitLines[0];
     string freq = splitLines[2];
     string dicItem = string.Format(item, key, freq);
     items.Append(dicItem);
 }
 string final = string.Format(outerTag, string.Format(innerTag, items.ToString()));
 File.WriteAllText("final.txt", final);
*/

    }
}

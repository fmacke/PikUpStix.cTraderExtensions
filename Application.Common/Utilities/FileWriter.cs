using System.Diagnostics;

namespace Application.Common.Utilities
{
    public class FileWriter
    {
        public static void Write(string[] lines, string path)
        {
            if (!File.Exists(path))
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (var line in lines)
                    sw.WriteLine(line);
            }            
        }
        public static void Read(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Debug.WriteLine(s);
                    Console.WriteLine(s);
                }
            }
        }
    }
}

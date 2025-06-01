using System.Diagnostics;
namespace Application.Common.Utilities
{
    public interface IPythonRunner
    {
        void RunScript(string scriptPath, string args);
    }
    public class PythonRunner : IPythonRunner
    {
        public void RunScript(string scriptPath, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            start.Arguments = $"{scriptPath} {args}";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }
    }
}
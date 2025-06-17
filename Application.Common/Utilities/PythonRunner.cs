using System.Diagnostics;
using static Application.Common.Constants.Permissions;
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
            //args = "16770 3 strategy 20/2/2025 C:/Users/finn/OneDrive/Desktop/ss";
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
                    string error = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.WriteLine($"Python Error: {error}");
                    }
                }
            }
        }
    }
}
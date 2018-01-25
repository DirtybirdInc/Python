using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PythonIntegration
{
    /// <summary> 
    /// Simple C# and Python interprocess communication 
    /// Author      : Ozcan ILIKHAN (modified by Dirtybird123)
    /// Created     : 02/26/2015 
    /// Last Update : 01/25/2018 
    /// </summary> 
    public static class CallPython
    {
        /// <summary>
        /// Executes a .py file 
        /// </summary>
        /// <param name="python">Full path of python interpreter.</param>
        /// <param name="pythonApp">Filename of the python application.</param>
        /// <param name="parameter">Parameter for the python app.</param>
        /// <param name="standardOutput">Standard output</param>
        /// <param name="standardError">Standard error</param>
        public static void Call(string python, string pythonApp, IEnumerable<object> parameter, out string standardOutput, out string standardError)
        {
            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.RedirectStandardError = true;

            // start python app with arguments  
            myProcessStartInfo.Arguments = pythonApp + (parameter != null ? " " + parameter.Aggregate((a, ars) => a.ToString() + " " + ars) : "");

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;

            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            //string myString = myStreamReader.ReadLine();

            // if you need to read multiple lines, you might use: 
            standardOutput = myProcess.StandardOutput.ReadToEnd();
            standardError = myProcess.StandardError.ReadToEnd();

            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();
        }
    }
}

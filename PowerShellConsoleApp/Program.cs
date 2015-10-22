using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.IO;

namespace PowerShellConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RunPowerShell("Test01.ps1"));
            Console.ReadLine();
        }

        #region Run PowerShell
        private static string RunPowerShell(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string scriptText = sr.ReadToEnd();
            sr.Close();
            return RunScript(scriptText);
        }


        /// <summary>
        /// Sample code from: 
        /// http://www.codeproject.com/Articles/18229/How-to-run-PowerShell-scripts-from-C
        /// </summary>
        /// <param name="scriptText"></param>
        /// <returns></returns>
        private static string RunScript(string scriptText)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            pipeline.Commands.Add("Out-String");

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}

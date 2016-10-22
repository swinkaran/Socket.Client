using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Configuration;
using System.Security.Permissions;
using System.IO;
using NLog.Internal;

namespace Socket_Demo.Client
{
    class Program
    {
        private int threadWait = 500;

        static void Main(string[] args)
        {
            Console.Title = "Socket Client";

            // Execute the client application
            Program p = new Program();
            p.OnStart();
            p.OnStop();
            
            //AsynchronousClient.StartClient();
        }

        private void OnStart()
        {
            this.Execute();
        }
    
        private void OnStop()
        {
            Console.WriteLine("LoyLap Client Stopped");
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void Execute()
        {
            // Read the file as one string.
            string strFileLocation = @"C:\loylap\data";
            //threadWait = Convert.ToInt16(ConfigurationManager.AppSettings["ThreadWait"]);

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = strFileLocation;

            // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Created += new FileSystemEventHandler(OnCreated);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Equals("response.txt"))
            {
                System.Threading.Thread.Sleep(threadWait);
                
                string[] requests = System.IO.File.ReadAllLines(e.FullPath);
                //string request = "\u0002PCVB7329I6P,cbivend00@loylap.com,-0.11\u0003";
                string request = "\u0002CCVB7329I6P\u0003";
                //string request = requests[0];
                Console.WriteLine("New file found here");
                //delete the file

                File.Delete(@"C:\loylap\data\response.txt");
                AsynchronousClient.StartClient(request);
            }
        }
    }
}


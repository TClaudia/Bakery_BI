using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BakeryBI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Register assembly resolve handler to redirect office.dll version 16.0.0.0 to 15.0.0.0
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Handles assembly resolution, specifically redirecting office.dll version 16.0.0.0 to 15.0.0.0
        /// </summary>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Check if this is a request for office.dll version 16.0.0.0
            if (args.Name.Contains("office") && args.Name.Contains("Version=16.0.0.0"))
            {
                try
                {
                    // Try to load version 15.0.0.0 from GAC
                    string gacPath15 = @"C:\WINDOWS\assembly\GAC_MSIL\office\15.0.0.0__71e9bce111e9429c\office.dll";
                    if (File.Exists(gacPath15))
                    {
                        return Assembly.LoadFrom(gacPath15);
                    }

                    // Try loading by name with version 15.0.0.0
                    string assemblyName = args.Name.Replace("Version=16.0.0.0", "Version=15.0.0.0");
                    return Assembly.Load(assemblyName);
                }
                catch
                {
                    // If loading fails, return null to let the default resolver try
                }
            }

            return null;
        }
    }
}
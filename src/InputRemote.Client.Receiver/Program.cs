using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InputRemote.Client.Receiver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool result;
            // Mutex is uesd for instance check
            var mutex = new System.Threading.Mutex(true, "inputremote-client-receiver", out result);

            if (!result)
            {
                MessageBox.Show("Another instance is already running.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

            // mutex shouldn't be released - important line
            GC.KeepAlive(mutex);                
            
        }
    }
}

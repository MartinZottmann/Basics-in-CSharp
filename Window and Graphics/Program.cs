using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MartinZottmann
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }
    }
}
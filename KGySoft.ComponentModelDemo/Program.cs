﻿#region Usings

#region Used Namespaces

using System.Threading;

using KGySoft.ComponentModelDemo.ViewModel;
using KGySoft.ComponentModelDemo.ViewWinForms.Forms;
using KGySoft.ComponentModelDemo.ViewWpf.Windows;

#endregion

#region Used Aliases

using WinFormsApp = System.Windows.Forms.Application;
using WpfApp = KGySoft.ComponentModelDemo.ViewWpf.App;

#endregion

#endregion

namespace KGySoft.ComponentModelDemo
{
    static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            LaunchStaThread(StartWpf);
            LaunchStaThread(StartWinForms);
        }

        private static void LaunchStaThread(ThreadStart start)
        {
            var thread = new Thread(start);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private static void StartWpf()
        {
            var application = new WpfApp();
            application.InitializeComponent();
            application.Run(new MainWindow(new MainViewModel()));
        }

        private static void StartWinForms()
        {
            WinFormsApp.EnableVisualStyles();
            WinFormsApp.SetCompatibleTextRenderingDefault(false);
            WinFormsApp.Run(new MainForm(new MainViewModel()));
        }

        #endregion
    }
}

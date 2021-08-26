using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MusicPlayerOnline.Common;

namespace MusicPlayerOnline
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "MusicPlayerOnlineOnlyRun");
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);
            }
            else
            {
                Messages.ShowError("程序已经运行");
                this.Shutdown();
            }
        }
        private static System.Threading.Mutex mutex;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var window = new MainWindow();
            window.ShowDialog();
        }
        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("程序差点挂了，你厉害");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("程序挂了，你赢了");
        }
    }
}

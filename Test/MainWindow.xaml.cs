using Microsoft.Win32;
using PanelSW.WPF.Controls;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SecurePassword = new SecureString();
            SecurePassword.AppendChar('p');
            SecurePassword.AppendChar('a');
            SecurePassword.AppendChar('s');
            SecurePassword.AppendChar('s');
            SecurePassword.AppendChar('w');
            SecurePassword.AppendChar('o');
            SecurePassword.AppendChar('r');
            SecurePassword.AppendChar('d');
            SecurePassword.AppendChar('!');

            DataContext = this;
            InitializeComponent();

            IsVisibleChanged += MainWindow_IsVisibleChanged;
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
                string psw = Marshal.PtrToStringUni(valuePtr);
                System.Diagnostics.Debugger.Log(0, "Debug", $"SecurePassword='{psw}'{Environment.NewLine}");
            }
            finally
            {
                if (valuePtr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                }
            }
        }

        public SecureString SecurePassword { get; set; }

        private void WaitableButton_Click(object sender, RoutedEventArgs e)
        {
            WaitableButton btn = sender as WaitableButton;
            if (btn.IsWaiting)
            {
                return;
            }

            btn.IsWaiting = true;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                Dispatcher.Invoke(new Action(() => btn.IsWaiting = false));
            });
        }


        public string FilePath { get; set; } = "Some File";
        public string FolderPath { get; set; } = "C:\\Some Folder";

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBoxWithButton tb = e.Source as TextBoxWithButton;
            if (tb != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = tb.Text;
                ofd.Filter = e.Parameter?.ToString();
                if (ofd.ShowDialog() == true)
                {
                    tb.Text = ofd.FileName;
                }
            }
        }
    }
}

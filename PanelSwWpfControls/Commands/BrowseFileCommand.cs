using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Commands
{
    /// <summary>
    /// Command to open a <see cref="OpenFileDialog"/> and use <see cref="File"/> binding to get and set selected path.
    /// </summary>
    public class BrowseFileCommand : MarkupExtension, ICommand
    {

        /// <summary>
        /// <see cref="OpenFileDialog"/> Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// <see cref="OpenFileDialog"/> Filter. Defaults to "All files (*.*)|*.*"
        /// </summary>
        public string Filter { get; set; } = "All files (*.*)|*.*";

        /// <summary>
        /// <see cref="OpenFileDialog"/> CheckFileExists. Defaults to true
        /// </summary>
        public bool CheckFileExists { get; set; } = true;

        /// <summary>
        /// <see cref="OpenFileDialog"/> CheckPathExists. Defaults to true
        /// </summary>
        public bool CheckPathExists { get; set; } = true;

        private static DependencyProperty FileBindingSinkProperty = DependencyProperty.RegisterAttached("FileBindingSink", typeof(string), typeof(BrowseFileCommand), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
     
        /// <summary>
        /// String binding to selected file
        /// </summary>
        public BindingBase File { get; set; }
        DependencyObject fileBinding_;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            string file = fileBinding_?.GetValue(FileBindingSinkProperty) as string;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Filter;
            dlg.CheckFileExists = CheckFileExists;
            dlg.CheckPathExists = CheckPathExists;
            dlg.Multiselect = false;
            dlg.Title = Title;

            if (!string.IsNullOrEmpty(file))
            {
                dlg.FileName = file;
            }

            if (dlg.ShowDialog() == true)
            {
                fileBinding_.SetValue(FileBindingSinkProperty, dlg.FileName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if ((target != null) && (target.TargetObject is DependencyObject) && (target.TargetProperty is DependencyProperty))
            {
                fileBinding_ = (DependencyObject)target.TargetObject;
            }
            else
            {
                return this;
            }

            BindingOperations.SetBinding(fileBinding_, BrowseFileCommand.FileBindingSinkProperty, File);

            return this;
        }
    }
}
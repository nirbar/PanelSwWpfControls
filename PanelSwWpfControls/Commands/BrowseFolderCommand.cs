using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

namespace PanelSW.WPF.Controls.Commands
{
    /// <summary>
    /// Command to open a <see cref="FolderBrowserDialog"/> and use <see cref="Folder"/> binding to get and set selected path.
    /// </summary>
    public class BrowseFolderCommand : MarkupExtension, ICommand
    {
        /// <summary>
        /// <see cref="FolderBrowserDialog"/> Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// <see cref="FolderBrowserDialog"/> ShowNewFolderButton. Defaults to true
        /// </summary>
        public bool ShowNewFolderButton { get; set; } = true;

        private static DependencyProperty FolderBindingSinkProperty = DependencyProperty.RegisterAttached("FolderBindingSink", typeof(string), typeof(BrowseFolderCommand), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
     
        /// <summary>
        /// String binding to selected folder
        /// </summary>
        public BindingBase Folder { get; set; }
        DependencyObject folderBinding_;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            string folder = folderBinding_?.GetValue(FolderBindingSinkProperty) as string;

            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = ShowNewFolderButton;
                dlg.Description = Description;

                if (!string.IsNullOrEmpty(folder))
                {
                    dlg.SelectedPath = folder;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    folderBinding_.SetValue(FolderBindingSinkProperty, dlg.SelectedPath);
                }
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
                folderBinding_ = (DependencyObject)target.TargetObject;
            }
            else
            {
                return this;
            }

            BindingOperations.SetBinding(folderBinding_, BrowseFolderCommand.FolderBindingSinkProperty, Folder);

            return this;
        }
    }
}
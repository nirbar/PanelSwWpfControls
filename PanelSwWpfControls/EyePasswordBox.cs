using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace PanelSW.WPF.Controls
{
    /// <summary>
    /// Password control with a button that shows plain password when pressed.
    /// </summary>
    [TemplatePart(Name = "PART_WatermarkPasswordBox", Type = typeof(WatermarkPasswordBox))]
    [TemplatePart(Name = "PART_ShowPasswordButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_PlainTextBox", Type = typeof(TextBox))]
    [TemplateVisualState(Name = "HiddenPassword", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "PlainPassword", GroupName = "ValueStates")]
    public partial class EyePasswordBox : Control, IDisposable
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public EyePasswordBox()
        {
            Focusable = false;
        }

        static EyePasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EyePasswordBox), new FrameworkPropertyMetadata(typeof(EyePasswordBox)));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            WatermarkPasswordBox = GetTemplateChild("PART_WatermarkPasswordBox") as WatermarkPasswordBox;
            ShowPasswordButton = GetTemplateChild("PART_ShowPasswordButton") as ButtonBase;
            PlainTextBox = GetTemplateChild("PART_PlainTextBox") as TextBox;

            CopyPasswordToBox();
        }

        void IDisposable.Dispose()
        {
            SecurePassword.Dispose();
            WatermarkPasswordBox?.SecurePassword?.Dispose();
        }

        private WatermarkPasswordBox watermarkPasswordBox_ = null;
        private WatermarkPasswordBox WatermarkPasswordBox
        {
            get => watermarkPasswordBox_;
            set
            {
                if (watermarkPasswordBox_ != null)
                {
                    watermarkPasswordBox_.PasswordChanged -= PasswordBox__PasswordChanged;
                }

                watermarkPasswordBox_ = value;
                if (watermarkPasswordBox_ != null)
                {
                    watermarkPasswordBox_.PasswordChanged += PasswordBox__PasswordChanged;
                }
            }
        }

        private ButtonBase showPasswordButton_ = null;
        private ButtonBase ShowPasswordButton
        {
            get => showPasswordButton_;
            set
            {
                if (showPasswordButton_ != null)
                {
                    showPasswordButton_.RemoveHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(btnShowPassword__Mouse));
                    showPasswordButton_.RemoveHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(btnShowPassword__Mouse));
                }

                showPasswordButton_ = value;
                if (showPasswordButton_ != null)
                {
                    showPasswordButton_.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(btnShowPassword__Mouse), true);
                    showPasswordButton_.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(btnShowPassword__Mouse), true);
                }
            }
        }

        private TextBox PlainTextBox { get; set; } = null;

        #region SecurePassword

        /// <summary>
        /// DependencyProperty for SecurePassword
        /// </summary>
        public static readonly DependencyProperty SecurePasswordProperty = DependencyProperty.Register("SecurePassword", typeof(SecureString), typeof(EyePasswordBox), new FrameworkPropertyMetadata(new SecureString(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSecurePasswordChanged));
        private static void OnSecurePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me?.CopyPasswordToBox();
        }

        private void CopyPasswordToBox()
        {
            if (WatermarkPasswordBox == null)
            {
                return;
            }

            if (WatermarkPasswordBox.SecurePassword != SecurePassword)
            {
                IntPtr valuePtr = IntPtr.Zero;
                try
                {
                    valuePtr = Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
                    WatermarkPasswordBox.SecurePassword.Clear();
                    for (int i = 0; i < SecurePassword.Length; ++i)
                    {
                        char c = (char)Marshal.ReadInt16(valuePtr, 2 * i);
                        WatermarkPasswordBox.SecurePassword.AppendChar(c);
                    }
                    // Workaround since WatermarkPasswordBox doesn't automatically update the text.
                    WatermarkPasswordBox.Text = new string(WatermarkPasswordBox.PasswordChar, SecurePassword.Length);
                }
                finally
                {
                    if (valuePtr != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                    }
                }
            }
        }

        private void btnShowPassword__Mouse(object sender, MouseButtonEventArgs e)
        {
            IsShowingPlainPassword = ShowPasswordButton?.IsPressed ?? false;
            if (IsShowingPlainPassword)
            {
                VisualStateManager.GoToState(this, "PlainPassword", true);
                if (PlainTextBox != null)
                {
                    PlainTextBox.Text = WatermarkPasswordBox?.Password ?? string.Empty;
                    PlainTextBox.Visibility = Visibility.Visible;
                }
                if (WatermarkPasswordBox != null)
                {
                    WatermarkPasswordBox.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "HiddenPassword", true);
                if (PlainTextBox != null)
                {
                    PlainTextBox.Text = "";
                    PlainTextBox.Visibility = Visibility.Collapsed;
                }
                if (WatermarkPasswordBox != null)
                {
                    WatermarkPasswordBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void PasswordBox__PasswordChanged(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                if (WatermarkPasswordBox.SecurePassword != SecurePassword)
                {
                    SecurePassword = WatermarkPasswordBox.SecurePassword;
                }
            });
        }

        /// <summary>
        /// Bindable SecureString
        /// </summary>
        public SecureString SecurePassword
        {
            get
            {
                return (SecureString)GetValue(SecurePasswordProperty);
            }
            set
            {
                SetValue(SecurePasswordProperty, value);
            }
        }

        #endregion

        #region IsShowingPlainPassword

        /// <summary>
        /// DependencyProperty for IsShowingPlainPassword
        /// </summary>
        public static readonly DependencyProperty IsShowingPlainPasswordProperty = DependencyProperty.Register("IsShowingPlainPassword", typeof(bool), typeof(EyePasswordBox), new PropertyMetadata(false, new PropertyChangedCallback(OnIsShowingPlainPasswordChanged)));
        private static void OnIsShowingPlainPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me.RaiseEvent(new RoutedEventArgs(IsShowingPlainPasswordChangedEvent));
        }

        /// <summary>
        /// Gets whether or not passord is currently shown in plain
        /// </summary>
        public bool IsShowingPlainPassword
        {
            get
            {
                return (bool)GetValue(IsShowingPlainPasswordProperty);
            }
            private set
            {
                SetValue(IsShowingPlainPasswordProperty, value);
            }
        }

        /// <summary>
        /// RoutedEvent for IsShowingPlainPassword
        /// </summary>
        public static readonly RoutedEvent IsShowingPlainPasswordChangedEvent = EventManager.RegisterRoutedEvent("IsShowingPlainPassword", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EyePasswordBox));

        /// <summary>
        /// Event triggered when IsShowingPlainPassword changes
        /// </summary>
        public event RoutedEventHandler IsShowingPlainPasswordChanged
        {
            add { AddHandler(IsShowingPlainPasswordChangedEvent, value); }
            remove { RemoveHandler(IsShowingPlainPasswordChangedEvent, value); }
        }

        #endregion

        #region Watermark

        /// <summary>
        /// DependencyProperty for Watermark
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(EyePasswordBox), new PropertyMetadata(""));

        /// <summary>
        /// Watermark to show when password is empty
        /// </summary>
        public string Watermark
        {
            get
            {
                return (string)GetValue(WatermarkProperty);
            }
            set
            {
                SetValue(WatermarkProperty, value);
            }
        }

        #endregion
    }
}
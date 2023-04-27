using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PanelSW.WPF.Controls
{
    /// <summary>
    /// Password control with a button that shows plain password when pressed.
    /// </summary>
    [TemplatePart(Name = "PART_PasswordBox", Type = typeof(PasswordBox))]
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

            PasswordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;
            ShowPasswordButton = GetTemplateChild("PART_ShowPasswordButton") as ButtonBase;
            PlainTextBox = GetTemplateChild("PART_PlainTextBox") as TextBox;

            CopyPasswordToBox();
        }

        void IDisposable.Dispose()
        {
            SecurePassword.Dispose();
            PasswordBox?.SecurePassword?.Dispose();
        }

        private PasswordBox PasswordBox_ = null;
        private PasswordBox PasswordBox
        {
            get => PasswordBox_;
            set
            {
                if (PasswordBox_ != null)
                {
                    PasswordBox_.PasswordChanged -= PasswordBox__PasswordChanged;
                }

                PasswordBox_ = value;
                if (PasswordBox_ != null)
                {
                    PasswordBox_.PasswordChanged += PasswordBox__PasswordChanged;
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
            if (PasswordBox == null)
            {
                return;
            }

            if (PasswordBox.SecurePassword != SecurePassword)
            {
                IntPtr valuePtr = IntPtr.Zero;
                try
                {
                    PasswordBox.SecurePassword.Clear();
                    if (SecurePassword == null)
                    {
                        return;
                    }

                    valuePtr = Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
                    for (int i = 0; i < SecurePassword.Length; ++i)
                    {
                        char c = (char)Marshal.ReadInt16(valuePtr, 2 * i);
                        PasswordBox.SecurePassword.AppendChar(c);
                    }
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
                    PlainTextBox.Text = PasswordBox?.Password ?? string.Empty;
                    PlainTextBox.Visibility = Visibility.Visible;
                }
                if (PasswordBox != null)
                {
                    PasswordBox.Visibility = Visibility.Collapsed;
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
                if (PasswordBox != null)
                {
                    PasswordBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void PasswordBox__PasswordChanged(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                if (PasswordBox.SecurePassword != SecurePassword)
                {
                    SecurePassword = PasswordBox.SecurePassword;
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
    }
}

﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace PanelSW.WPF.Controls
{
    /// <summary>
    /// Interaction logic for EyePasswordBox.xaml
    /// </summary>
    [TemplatePart(Name = "PART_WatermarkPasswordBox", Type = typeof(WatermarkPasswordBox))]
    [TemplatePart(Name = "PART_ShowPasswordButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_PlainTextBox", Type = typeof(TextBox))]
    [TemplateVisualState(Name = "HiddenPassword", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "PlainPassword", GroupName = "ValueStates")]
    public partial class EyePasswordBox : Control, IDisposable
    {
        public EyePasswordBox()
        {
        }

        static EyePasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EyePasswordBox), new FrameworkPropertyMetadata(typeof(EyePasswordBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            WatermarkPasswordBox = GetTemplateChild("PART_WatermarkPasswordBox") as WatermarkPasswordBox;
            ShowPasswordButton = GetTemplateChild("PART_ShowPasswordButton") as ButtonBase;
            PlainTextBox = GetTemplateChild("PART_PlainTextBox") as TextBox;
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

        public static readonly DependencyProperty SecurePasswordProperty = DependencyProperty.Register("SecurePassword", typeof(SecureString), typeof(EyePasswordBox), new FrameworkPropertyMetadata(new SecureString(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSecurePasswordChanged));
        private static void OnSecurePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            if (me.WatermarkPasswordBox == null)
            {
                return;
            }

            if (me.WatermarkPasswordBox.SecurePassword != me.SecurePassword)
            {
                IntPtr valuePtr = IntPtr.Zero;
                try
                {
                    valuePtr = Marshal.SecureStringToGlobalAllocUnicode(me.SecurePassword);
                    me.WatermarkPasswordBox.SecurePassword.Clear();
                    for (int i = 0; i < me.SecurePassword.Length; ++i)
                    {
                        char c = (char)Marshal.ReadInt16(valuePtr, 2 * i);
                        me.WatermarkPasswordBox.SecurePassword.AppendChar(c);
                    }
                    // Workaround since WatermarkPasswordBox doesn't automatically update the text.
                    me.WatermarkPasswordBox.Text = new string(me.WatermarkPasswordBox.PasswordChar, me.SecurePassword.Length);
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

        public static readonly DependencyProperty IsShowingPlainPasswordProperty = DependencyProperty.Register("IsShowingPlainPassword", typeof(bool), typeof(EyePasswordBox), new PropertyMetadata(false, new PropertyChangedCallback(OnIsShowingPlainPasswordChanged)));
        private static void OnIsShowingPlainPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me.RaiseEvent(new RoutedEventArgs(IsShowingPlainPasswordChangedEvent));
        }

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

        public static readonly RoutedEvent IsShowingPlainPasswordChangedEvent = EventManager.RegisterRoutedEvent("IsShowingPlainPassword", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EyePasswordBox));
        public event RoutedEventHandler IsShowingPlainPasswordChanged
        {
            add { AddHandler(IsShowingPlainPasswordChangedEvent, value); }
            remove { RemoveHandler(IsShowingPlainPasswordChangedEvent, value); }
        }

        #endregion

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(EyePasswordBox), new PropertyMetadata(""));
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

        #region Text Padding

        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register("TextPadding", typeof(Thickness), typeof(EyePasswordBox), new PropertyMetadata(new Thickness(0, 0, 0, 0)));
        public Thickness TextPadding
        {
            get
            {
                return (Thickness)GetValue(TextPaddingProperty);
            }
            set
            {
                SetValue(TextPaddingProperty, value);
            }
        }

        #endregion

        #region Eye Image

        public static readonly DependencyProperty EyeImageProperty = DependencyProperty.Register("EyeImage", typeof(ImageSource), typeof(EyePasswordBox)
            , new PropertyMetadata(
                new BitmapImage(
                    // Eye image with Apache 2.0 license downloaded from http://www.iconarchive.com/show/noto-emoji-people-clothing-objects-icons-by-google/12125-eye-icon.html
                    new Uri("pack://application:,,,/PanelSwWpfControls;component/Resources/Eye.png"))));

        public ImageSource EyeImage
        {
            get
            {
                return (ImageSource)GetValue(EyeImageProperty);
            }
            set
            {
                SetValue(EyeImageProperty, value);
            }
        }

        #endregion

        #region Eye Image Height

        public static readonly DependencyProperty EyeHeightProperty = DependencyProperty.Register("EyeHeight", typeof(double), typeof(EyePasswordBox), new FrameworkPropertyMetadata(32d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double EyeHeight
        {
            get
            {
                return (double)GetValue(EyeHeightProperty);
            }
            set
            {
                SetValue(EyeHeightProperty, value);
            }
        }

        #endregion

        #region Eye Image Width

        public static readonly DependencyProperty EyeWidthProperty = DependencyProperty.Register("EyeWidth", typeof(double), typeof(EyePasswordBox), new FrameworkPropertyMetadata(32d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double EyeWidth
        {
            get
            {
                return (double)GetValue(EyeWidthProperty);
            }
            set
            {
                SetValue(EyeWidthProperty, value);
            }
        }

        #endregion
    }
}
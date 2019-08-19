﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace PanelSW.WPF.Controls
{
    /// <summary>
    /// Interaction logic for EyePasswordBox.xaml
    /// </summary>
    public partial class EyePasswordBox : UserControl
    {
        public EyePasswordBox()
        {
            InitializeComponent();
            passwordBox_.PasswordChanged += PasswordBox__PasswordChanged;
        }

        #region Password

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(EyePasswordBox), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPasswordChanged)));
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            if (me.passwordBox_.Password != me.Password)
            {
                me.passwordBox_.Password = me.Password;
            }
        }

        private void PasswordBox__PasswordChanged(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
               if (passwordBox_.Password != Password)
               {
                   Password = passwordBox_.Password;
               }
           });
        }

        public string Password
        {
            get
            {
                return (string)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        #endregion

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(EyePasswordBox), new PropertyMetadata("", new PropertyChangedCallback(OnWatermarkChanged)));
        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me.Watermark = e.NewValue as string;
        }

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

        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register("TextPadding", typeof(Thickness), typeof(EyePasswordBox), new PropertyMetadata(new Thickness(0, 0, 0, 0), new PropertyChangedCallback(OnTextPaddingChanged)));
        private static void OnTextPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me.TextPadding = (Thickness)e.NewValue;
        }

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

        #region Eye Image Height

        public static readonly DependencyProperty EyeHeightProperty = DependencyProperty.Register("EyeHeight", typeof(int), typeof(EyePasswordBox), new PropertyMetadata(32, new PropertyChangedCallback(OnEyeHeightChanged)));
        private static void OnEyeHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me.EyeHeight = (int)e.NewValue;
        }

        public int EyeHeight
        {
            get
            {
                return (int)GetValue(EyeHeightProperty);
            }
            set
            {
                SetValue(EyeHeightProperty, value);
            }
        }

        #endregion

        #region Eye Image Width

        public static readonly DependencyProperty EyeWidthProperty = DependencyProperty.Register("EyeWidth", typeof(int), typeof(EyePasswordBox), new PropertyMetadata(32, new PropertyChangedCallback(OnEyeWidthChanged)));
        private static void OnEyeWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EyePasswordBox me = d as EyePasswordBox;
            me.EyeWidth = (int)e.NewValue;
        }

        public int EyeWidth
        {
            get
            {
                return (int)GetValue(EyeWidthProperty);
            }
            set
            {
                SetValue(EyeWidthProperty, value);
            }
        }

        #endregion
    }
}
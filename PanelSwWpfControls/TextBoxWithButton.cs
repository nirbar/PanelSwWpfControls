using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PanelSW.WPF.Controls
{
    /// <summary>
    /// TextBox control with a button on the right hand side
    /// </summary>
    [TemplatePart(Name = "PART_Button", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplateVisualState(Name = "ButtonPressed", GroupName = "ValueStates")]
    public partial class TextBoxWithButton : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public TextBoxWithButton()
        {
        }

        static TextBoxWithButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWithButton), new FrameworkPropertyMetadata(typeof(TextBoxWithButton)));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button = GetTemplateChild("PART_Button") as ButtonBase;
            TextBox = GetTemplateChild("PART_TextBox") as TextBox;
        }

        private ButtonBase button_ = null;
        private ButtonBase Button
        {
            get => button_;
            set
            {
                if (button_ != null)
                {
                    button_.RemoveHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(btn_Mouse));
                    button_.RemoveHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(btn_Mouse));
                }

                button_ = value;
                if (button_ != null)
                {
                    button_.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(btn_Mouse), true);
                    button_.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(btn_Mouse), true);
                }
            }
        }

        private void btn_Mouse(object sender, MouseButtonEventArgs e)
        {
            ButtonPressed = Button?.IsPressed ?? false;
            if (ButtonPressed)
            {
                VisualStateManager.GoToState(this, "ButtonPressed", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        private TextBox TextBox { get; set; } = null;

        #region Text

        /// <summary>
        /// DependencyProperty for Text
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithButton), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxWithButton me = d as TextBoxWithButton;
            me.RaiseEvent(new RoutedEventArgs(TextChangedEvent));
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// RoutedEvent for TextChanged
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxWithButton));

        /// <summary>
        /// Event triggered when text changes
        /// </summary>
        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        #endregion

        #region ButtonPressed

        /// <summary>
        /// DependencyProperty for ButtonPressed
        /// </summary>
        public static readonly DependencyProperty ButtonPressedProperty = DependencyProperty.Register("ButtonPressed", typeof(bool), typeof(TextBoxWithButton), new PropertyMetadata(false, new PropertyChangedCallback(OnButtonPressedChanged)));
        private static void OnButtonPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxWithButton me = d as TextBoxWithButton;
            me.RaiseEvent(new RoutedEventArgs(ButtonPressedChangedEvent));
        }

        /// <summary>
        /// Gets whether or not button is pressed
        /// </summary>
        public bool ButtonPressed
        {
            get
            {
                return (bool)GetValue(ButtonPressedProperty);
            }
            private set
            {
                SetValue(ButtonPressedProperty, value);
            }
        }

        /// <summary>
        /// RoutedEvent for ButtonPressedChanged
        /// </summary>
        public static readonly RoutedEvent ButtonPressedChangedEvent = EventManager.RegisterRoutedEvent("ButtonPressed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxWithButton));

        /// <summary>
        /// Event triggered when ButtonPressed changes. Equivalent to button click events
        /// </summary>
        public event RoutedEventHandler ButtonPressedChanged
        {
            add { AddHandler(ButtonPressedChangedEvent, value); }
            remove { RemoveHandler(ButtonPressedChangedEvent, value); }
        }

        #endregion

        #region Command

        /// <summary>
        /// DependencyProperty for Command
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TextBoxWithButton), new PropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxWithButton me = d as TextBoxWithButton;
            if (me.button_ != null)
            {
                me.button_.Command = me.Command;
            }
        }

        /// <summary>
        /// Button Command
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        #endregion

        #region CommandParameter

        /// <summary>
        /// DependencyProperty for CommandParameter
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TextBoxWithButton), new PropertyMetadata(null));

        /// <summary>
        /// Button CommandParameter
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        #endregion
    }
}
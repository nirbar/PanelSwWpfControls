using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PanelSW.WPF.Controls
{
    /// <summary>
    /// Button with a visual state to indicate length operations
    /// </summary>
    [TemplatePart(Name = "PART_Waiting", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_Button", Type = typeof(ButtonBase))]
    [TemplateVisualState(Name = "Waiting", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Ready", GroupName = "ValueStates")]
    public class WaitableButton : Button
    {
        static WaitableButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitableButton), new FrameworkPropertyMetadata(typeof(WaitableButton)));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ButtonPart = GetTemplateChild("PART_Button") as ButtonBase;
            WaitingPart = GetTemplateChild("PART_Waiting") as FrameworkElement;
        }

        private ButtonBase ButtonPart { get; set; } = null;
        private FrameworkElement WaitingPart { get; set; } = null;

        #region IsWaiting

        /// <summary>
        /// DependencyProperty for IsWaiting
        /// </summary>
        public static readonly DependencyProperty IsWaitingProperty = DependencyProperty.Register("IsWaiting", typeof(bool), typeof(WaitableButton), new PropertyMetadata(false, OnIsWaitingChanged));
        private static void OnIsWaitingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WaitableButton me = d as WaitableButton;
            if (me != null)
            {
                if (me.IsWaiting)
                {
                    if ((me.ButtonPart != null) && (me.WaitingPart != null))
                    {
                        me.ButtonPart.Visibility = Visibility.Collapsed;
                        me.WaitingPart.Visibility = Visibility.Visible;
                    }
                    VisualStateManager.GoToState(me, "Waiting", true);
                }
                else
                {
                    if ((me.ButtonPart != null) && (me.WaitingPart != null))
                    {
                        me.ButtonPart.Visibility = Visibility.Visible;
                        me.WaitingPart.Visibility = Visibility.Collapsed;
                    }
                    VisualStateManager.GoToState(me, "Ready", true);
                }
            }
        }

        /// <summary>
        /// Gets or set value to indicate length operation is in process
        /// </summary>
        public bool IsWaiting
        {
            get
            {
                return (bool)GetValue(IsWaitingProperty);
            }
            set
            {
                SetValue(IsWaitingProperty, value);
            }
        }

        /// <summary>
        /// RoutedEvent for IsWaitingChanged
        /// </summary>
        public static readonly RoutedEvent IsWaitingChangedEvent = EventManager.RegisterRoutedEvent("IsWaiting", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WaitableButton));

        /// <summary>
        /// Event triggered on changes to IsWaiting
        /// </summary>
        public event RoutedEventHandler IsWaitingChanged
        {
            add { AddHandler(IsWaitingChangedEvent, value); }
            remove { RemoveHandler(IsWaitingChangedEvent, value); }
        }

        #endregion
    }
}
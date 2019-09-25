using System.Windows;
using System.Windows.Controls;

namespace PanelSW.WPF.Controls
{
    [TemplatePart(Name = "PART_Button", Type = typeof(Button))]
    [TemplateVisualState(Name = "Waiting", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Ready", GroupName = "ValueStates")]
    public class WaitableButton : Button
    {
        public override void OnApplyTemplate()
        {
            button_ = GetTemplateChild("PART_Button") as Button;
        }

        private Button button_ = null;

        #region IsWaiting

        public static readonly DependencyProperty IsWaitingProperty = DependencyProperty.Register("IsWaiting", typeof(bool), typeof(WaitableButton), new PropertyMetadata(false));
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

        public static readonly RoutedEvent IsWaitingChangedEvent = EventManager.RegisterRoutedEvent("IsWaiting", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WaitableButton));
        public event RoutedEventHandler IsWaitingChanged
        {
            add { AddHandler(IsWaitingChangedEvent, value); }
            remove { RemoveHandler(IsWaitingChangedEvent, value); }
        }

        #endregion
    }
}
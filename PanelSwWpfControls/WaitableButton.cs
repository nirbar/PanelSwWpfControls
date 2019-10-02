using System.Windows;
using System.Windows.Controls;

namespace PanelSW.WPF.Controls
{
    [TemplateVisualState(Name = "Waiting", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Ready", GroupName = "ValueStates")]
    public class WaitableButton : Button
    {
        #region IsWaiting

        public static readonly DependencyProperty IsWaitingProperty = DependencyProperty.Register("IsWaiting", typeof(bool), typeof(WaitableButton), new PropertyMetadata(false, OnIsWaitingChanged));
        private static void OnIsWaitingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WaitableButton me = d as WaitableButton;
            if (me != null)
            {
                VisualStateManager.GoToState(me, me.IsWaiting ? "Waiting" : "Ready", true);
            }
        }

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
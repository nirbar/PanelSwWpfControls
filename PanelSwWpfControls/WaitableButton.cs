using System.Windows;
using System.Windows.Controls;

namespace PanelSW.WPF.Controls
{
    public class WaitableButton : Button
    {
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
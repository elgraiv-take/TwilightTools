using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Elgraiv.TwilightTools.Views;
using Microsoft.Xaml.Behaviors;


namespace Elgraiv.TwilightTools.Mvvm.Interactivity
{
    public class ShowDialogBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty MessengerProperty =
              DependencyProperty.Register(
                  "Messenger",
                  typeof(INotifyMessageSent),
                  typeof(ShowDialogBehavior),
                  new PropertyMetadata(null, MessengerChanged));

        public INotifyMessageSent Messenger
        {
            get => (INotifyMessageSent)GetValue(MessengerProperty);
            set => SetValue(MessengerProperty, value);
        }

        private static void MessengerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ShowDialogBehavior behavior)
            {
                if (e.OldValue is INotifyMessageSent oldMessagner)
                {
                    oldMessagner.NotifyDialogMessageSent -= behavior.DialogMessageReceived;
                }
                if (e.NewValue is INotifyMessageSent newMessagner)
                {
                    newMessagner.NotifyDialogMessageSent += behavior.DialogMessageReceived;
                }
            }
        }

        private Window? _orner;

        protected override void OnAttached()
        {
            base.OnAttached();
            _orner = AssociatedObject as Window;
            if (_orner == null)
            {
                _orner = Window.GetWindow(AssociatedObject);
            }
        }

        private void DialogMessageReceived(object? sender, DialogMessageEventArgs e)
        {
            var viewModel = e.ViewModel;
            if (viewModel == null)
            {
                return;
            }
            if (viewModel.DialogResult != null)
            {
                return;
            }
            var window = AssociatedObject as Window;
            if (window == null)
            {

            }
            var dialog = new DialogWindow()
            {
                DataContext = viewModel,
                Owner = _orner,
            };
            dialog.ShowDialog();
        }
    }
}

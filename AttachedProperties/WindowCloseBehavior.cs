using System.Windows;
using System.Windows.Input;
namespace RecomendMovie.AttachedProperties
{
    public static class WindowCloseBehavior
    {
        public static readonly DependencyProperty ClosingCommandProperty =
            DependencyProperty.RegisterAttached(
                "ClosingCommand",
                typeof(ICommand),
                typeof(WindowCloseBehavior),
                new PropertyMetadata(null, OnClosingCommandChanged));

        public static ICommand GetClosingCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosingCommandProperty);
        }

        public static void SetClosingCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosingCommandProperty, value);
        }

        private static void OnClosingCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                if (e.NewValue is ICommand command)
                {
                    window.Closing += (sender, args) =>
                    {
                        if (command.CanExecute(null))
                        {
                            command.Execute(null);
                        }
                    };
                }
            }
        }
    }
}

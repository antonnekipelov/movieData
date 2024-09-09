using System.Windows;
using System.Windows.Controls;

namespace RecomendMovie.AttachedProperties
{
    public static class PasswordBoxHelper
    {
        // Флаг для предотвращения зацикливания при изменении пароля
        private static bool _isUpdating;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnPasswordChanged));

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (_isUpdating)
                    return;

                var passwordBox = d as PasswordBox;
                if (passwordBox == null)
                    throw new InvalidOperationException("Attached property 'Password' can only be used with PasswordBox.");

                if (e.NewValue is string newPassword)
                {
                    _isUpdating = true;
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                    passwordBox.Password = newPassword;
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in PasswordBoxHelper.OnPasswordChanged: {ex.Message}");
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isUpdating)
                    return;

                var passwordBox = sender as PasswordBox;
                if (passwordBox == null)
                    throw new InvalidOperationException("Event handler 'PasswordChanged' can only be attached to PasswordBox.");

                _isUpdating = true;
                SetPassword(passwordBox, passwordBox.Password);
            }
            catch (Exception ex)
            {
                // Логирование исключения или другие действия по обработке ошибок
                MessageBox.Show($"Error in PasswordBoxHelper.PasswordBox_PasswordChanged: {ex.Message}");
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }
}

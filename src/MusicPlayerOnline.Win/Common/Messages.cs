using System.Windows;

namespace MusicPlayerOnline.Win.Common
{
    public class Messages
    {
        private const string Title = "在线音乐助手";
        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowError(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

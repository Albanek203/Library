using System.Windows;
using Library.Data.Enumeration;
using Library.View.AdditionalView;

namespace Library.Data.Models {
    public static class MessageWindow {
        private static void ApplyEffect(Window window, int radius) {
            if (window == null) return;
            window.Effect = new System.Windows.Media.Effects.BlurEffect { Radius = radius };
        }
        public static bool Show(Window ownWindow, string message, TypeWindow type, MessageButton messageButton) {
            var result = false;
            if (type == TypeWindow.ErrorWindow) {
                ApplyEffect(ownWindow, 100);
                var errorWindow = new ErrorWindow(message, messageButton) { Owner = ownWindow };
                if (ownWindow == null) errorWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                // ReSharper disable once PossibleInvalidOperationException
                result = (bool)errorWindow.ShowDialog();
                ApplyEffect(ownWindow, 0);
            }
            else if (type == TypeWindow.Information) {
                ApplyEffect(ownWindow, 100);
                var informationWindow = new InformationWindow(message, messageButton) { Owner = ownWindow };
                if (ownWindow == null) informationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                // ReSharper disable once PossibleInvalidOperationException
                result = (bool)informationWindow.ShowDialog();
                ApplyEffect(ownWindow, 0);
            }
            return result;
        }
    }
}
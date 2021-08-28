using System.Windows;
using Library.Data.Enumeration;
using Library.View.AdditionalView;

namespace Library.Data.Models {
    public static class MessageWindow {
        private static void ApplyEffect(Window window, int radius) {
            var objBlur = new System.Windows.Media.Effects.BlurEffect { Radius = radius };
            window.Effect = objBlur;
        }
        public static bool Show(Window ownWindow, string message, TypeWindow type, MessageButton messageButton) {
            var result = false;
            if (type == TypeWindow.ErrorWindow) {
                ApplyEffect(ownWindow, 100);
                var errorWindow = new ErrorWindow(message, messageButton) { Owner = ownWindow };
                // ReSharper disable once PossibleInvalidOperationException
                result = (bool)errorWindow.ShowDialog();
                ApplyEffect(ownWindow, 0);
            }
            else if (type == TypeWindow.Information) {
                ApplyEffect(ownWindow, 100);
                var informationWindow = new InformationWindow(message, messageButton) { Owner = ownWindow };
                // ReSharper disable once PossibleInvalidOperationException
                result = (bool)informationWindow.ShowDialog();
                ApplyEffect(ownWindow, 0);
            }
            return result;
        }
    }
}
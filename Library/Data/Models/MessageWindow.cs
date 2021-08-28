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
            switch (type) {
                case TypeWindow.ErrorWindow:
                    ApplyEffect(ownWindow, 100);
                    var window = new ErrorWindow(message, messageButton) { Owner = ownWindow };
                    var result = window.ShowDialog();
                    ApplyEffect(ownWindow, 0);
                    return result == true;
            }

            return false;
        }
    }
}
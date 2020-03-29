using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace JournalingGUI.Hellpers
{
    public class MessageBoxHellpers
    {
        /// <summary>
        /// Выводит сообщение об ошибке.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="body">Сообщение пользователю.</param>
        public static void Error(string title, string body) => MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

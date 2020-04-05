using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace JournalingGUI.Hellpers
{
    public class MessageBoxHellpers
    {
        /// <summary>
        /// Заголовок для окна с вопросом.
        /// </summary>
        private const string question_title = "Предупреждение";

        /// <summary>
        /// Выводит сообщение об ошибке.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="body">Сообщение пользователю.</param>
        public static void Error(string title, string body) => MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);

        /// <summary>
        /// Задает вопрос пользователю и возвращает true в случаи нажатия на кнопку "Yes".
        /// </summary>
        /// <param name="body">Сообщение пользоваетлю.</param>
        /// <returns></returns>
        public static MessageBoxResult Questions(string body) => MessageBox.Show(body, question_title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
    }
}

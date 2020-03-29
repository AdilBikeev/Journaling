using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JournalingGUI.Hellpers
{
    public static class FileSystemHellpers
    {
        /// <summary>
        /// Возвращает содержимое файла.
        /// </summary>
        /// <param name="path">Полный путь к файлу.</param>
        /// <returns></returns>
        public static string GetBodyFile(string path)
        {
            var body = string.Empty;

            using(StreamReader sr = new StreamReader(path))
            {
                body = sr.ReadToEnd();
            }

            return body;
        }
    }
}

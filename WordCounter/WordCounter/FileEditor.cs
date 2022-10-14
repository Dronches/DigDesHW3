using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace WordCounter
{
    // класс взаимодействия с файлом
    class FileEditor
    {
        /// <summary>
        /// Поиск наибольшего по количеству символов ключа - для красивого табулированного вывода в файлик
        /// </summary>
        /// <param name="uniqueWords">Список слов и их сопоставлений количества</param>
        /// <returns>Значение количества символов в наибольшем ключе</returns>
        protected static int FindLargestKeyLength(Dictionary<string, int> uniqueWords)
        {
            int maxLen = 0;
            foreach (var word in uniqueWords)
                if (word.Key.Length > maxLen)
                    maxLen = word.Key.Length;
            return maxLen;
        }

        /// <summary>
        /// отсортировать список в порядке убывания
        /// </summary>
        /// <param name="uniqueWords"></param>
        /// <returns>Список в порядке убывания по количеству вхождений (value)</returns>
        private static IOrderedEnumerable<KeyValuePair<string, int>> sortAlgorithm(Dictionary<string, int> uniqueWords)
        {
            // производим сортировку с помощью query в c#
            return (from word in uniqueWords
                    orderby word.Value descending
                    select word);
        }


        /// <summary>
        /// Запись списка в файлик
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="uniqueWords">Список соответствий слов и количества совпадений</param>
        /// <returns>true - удалось записать список; false - не удлаось открыть файл, не удалось записать список</returns>
        public static bool PrintToFile(string path, string name, Dictionary<string, int> uniqueWords)
        {
            // преобразуем путь к нужному для записи
            path = PathParser.ConvertPath(path, name);

            bool isAppend = false; // вспомогательная переменная определяющая, что производится перезапись
            try
            {
                // Запись в файл
                using (StreamWriter writer = new StreamWriter(path, isAppend, Encoding.Default))
                {
                    // производим сортировку с помощью query в c#
                    IOrderedEnumerable<KeyValuePair<string, int>> sortedUniqueWords = sortAlgorithm(uniqueWords);

                    // ищем максимальный размер
                    int maxLen = FindLargestKeyLength(uniqueWords);

                    // производим запись в файл
                    foreach (KeyValuePair<string, int> word in sortedUniqueWords)
                        writer.WriteLine(word.Key.PadRight(maxLen) + "\t" + word.Value);
                    // выводим сообщение об успешной записи:
                    Console.WriteLine("Информация была успешно записана в файл со следующим расположением: " + path);
                }
            }
            catch
            {
                Console.WriteLine("Не удалось открыть файл для записи по пути: " + path);
                return false;
            }

            return true;
        }

        /// Получить массив строк данных из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>массив строк, считанных из файла</returns>
        public static string[] GetInfoFromPath(string path)
        {
            // если файл существует
            if (File.Exists(path))
            {
                // считываем все строки
                string[] fileInfo = File.ReadAllLines(path, Encoding.Default);
                if (fileInfo.Length == 0)
                {
                    Console.WriteLine("В файле по пути '" + PathParser.ToCurrentDirection(path) + "' не содержатся данные.");
                    return null;
                }
                return fileInfo;
            }
            else
            {
                Console.WriteLine("Не удалось открыть файл по пути '" + PathParser.ToCurrentDirection(path) + "' для чтения.");
                return null;
            }
        }


    }
}
using System;
using System.Collections.Generic;

namespace WordCounterLib
{
    /// <summary>
    /// класс взаимодействия со словарем информации
    /// </summary>
    public class DictionaryWords : IDictionaryWords
    {
        /// <summary>
        /// основной словарь сопоставления слов и их количества
        /// </summary>
        private Dictionary<string, int> uniqueWords;

        /// <summary>
        /// конструктор, НЕ выделяющий память под uniqueWords
        /// </summary>
        public DictionaryWords() => uniqueWords = new Dictionary<string, int>();

        /// <summary>
        /// Добавить слова в словарь или увеличение счетчика, если слово уже определено
        /// </summary>
        /// <param name="word">строка, которую пытаемся добавить в словарь</param>
        public void AddToDictionary(string word)
        {
            // обрезка дефиса справа
            word = WordParser.CutHyphen(word);
            // привести к нижнему регистру
            word = word.ToLower();
            // пытаемся добавить - ели не удалось добавить, то производим увеличение
            if (!uniqueWords.ContainsKey(word))
                uniqueWords.Add(word, 1);
            else
                // увеличиваем значение счетчика
                uniqueWords[word] += 1;
        }


        /// <summary>
        /// Метод получения словаря из класса (специально оформлен не как getter)
        /// </summary>
        /// <returns>Хранимый в классе словарь</returns>
        public Dictionary<string, int> getDictionary()
        {
            return uniqueWords;
        }

        /// <summary>
        /// Очистка данных из словаря
        /// </summary>
        public void clearDictionary()
        {
            uniqueWords.Clear();
        }

        /// <summary>
        /// Необязательная составляющая - помощь для garbage collector
        /// </summary>
        ~DictionaryWords()
        {
            this.clearDictionary();
        }

    }
}

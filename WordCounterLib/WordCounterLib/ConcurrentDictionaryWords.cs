using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace WordCounterLib
{
    /// <summary>
    /// класс взаимодействия с потокозащищенным словарем информации
    /// </summary>
    class ConcurrentDictionaryWords : IDictionaryWords
    {
        /// <summary>
        /// основной словарь сопоставления слов и их количества
        /// </summary>
        private ConcurrentDictionary<string, int> uniqueWords;

        /// <summary>
        /// конструктор, НЕ выделяющий память под uniqueWords
        /// </summary>
        public ConcurrentDictionaryWords() => uniqueWords = new ConcurrentDictionary<string, int>();

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
            if (!uniqueWords.TryAdd(word, 1))
                // увеличиваем значение счетчика
                uniqueWords[word] += 1;
        }


        /// <summary>
        /// Метод получения словаря из класса (специально оформлен не как getter)
        /// </summary>
        /// <returns>Хранимый в классе словарь</returns>
        public Dictionary<string, int> getDictionary()
        {
            return uniqueWords.ToDictionary(entry => entry.Key, entry => entry.Value);
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
        ~ConcurrentDictionaryWords()
        {
            this.clearDictionary();
        }
    }
}

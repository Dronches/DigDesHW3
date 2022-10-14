using System;
using System.Collections.Generic;

namespace WordCounterLib
{
    interface IDictionaryWords
    {
        /// <summary>
        /// Добавить слова в словарь или увеличение счетчика, если слово уже определено
        /// </summary>
        /// <param name="word">строка, которую пытаемся добавить в словарь</param>
        void AddToDictionary(string word);

        /// <summary>
        /// Метод получения словаря из класса (специально оформлен не как getter)
        /// </summary>
        /// <returns>Хранимый в классе словарь</returns>
        Dictionary<string, int> getDictionary();

        /// <summary>
        /// Очистка данных из словаря
        /// </summary>
        void clearDictionary();
    }
}

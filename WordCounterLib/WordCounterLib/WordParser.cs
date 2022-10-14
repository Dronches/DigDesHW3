using System;

namespace WordCounterLib
{
    /// <summary>
    /// Вспомогательный класс взаимодействия с словом и его преобразования
    /// </summary>
    static class WordParser
    {

        /// <summary>
        /// обрезка боковых дефисов в конечном слове справа по ссылке
        /// Example: "жизнь-" -> "жизнь"
        /// </summary>
        /// <param name="word">Входная строка с возможным наличием дефисоф</param>
        /// <returns>Возврат строки без дефисов</returns>
        public static string CutHyphen(string word)
        {
            int posEnd = word.Length - 1;
            while (word[posEnd] == '-' && posEnd >= 0)
                --posEnd;
            if (posEnd != word.Length - 1)
                word = word.Substring(0, posEnd + 1);
            return word;
        }
    }
}

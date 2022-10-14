using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordCounterLib
{

    /// <summary>
    /// Класс считывания данных и их обработки - подсчет количества слов в тексте
    /// </summary>
    public class LineParser
    {
        /// <summary>
        /// Раздление строки по словам - запись их в словар
        /// </summary>
        /// <param name="line">строка, которая разделяется на слова</param>
        private void ParseLine(string line, IDictionaryWords dict)
        {
            bool wasStart = false; // вспомогательная переменная, определяющая - было ли начало
            int posStart = 0; // переменная, запоминающая позицию начала слова
            int posSaver = 0; // вспомогательная переменная, позволяющая определить принадлежность к сноске - является ли значение в квадратных скобках числом
            // пробег по всем буквам
            for (int posEnd = 0; posEnd < line.Length; ++posEnd)
            {
                // Если буква или число (считаем их за возможный элемент слова. Например, "Юпитер-1", "1T"  "1999")
                if (Char.IsLetter(line[posEnd]) || Char.IsDigit(line[posEnd]))
                {
                    // если дошли до начала - начало слова, сохраняем позиции
                    if (!wasStart)
                    {
                        posStart = posEnd;
                        wasStart = true;
                    }
                }
                // если символы, которые могут быть в слове
                else if (line[posEnd] == '\'' || line[posEnd] == '-')
                {
                    // продолжаем движение
                    continue;
                }
                // если [54] - специальное обозначение сноски, однакое если [49 год], то считываются слова 49 и год
                else if (line[posEnd] == '[')
                {
                    // если мы считывали слово, то добавляем его
                    if (wasStart)
                    {
                        dict.AddToDictionary(line.Substring(posStart, posEnd - posStart));
                        wasStart = false; // обнуляем флаг начала
                    }

                    posSaver = posEnd + 1;
                    // движемся до конца строки или до ']'
                    while (posSaver < line.Length && line[posSaver] != ']')
                        ++posSaver;

                    // еcли дошли до ']', если имеем число внутри скобок - значит сноска
                    if (posSaver < line.Length && line[posSaver] == ']' && line.Substring(posEnd + 1, posSaver - posEnd - 1).Trim().All(char.IsDigit))
                        // производим переход на позицию после сноски
                        posEnd = posSaver;
                    // иначе продолжаем идти по циклу
                }
                // если символ, который не относится к слову
                else
                {
                    // если мы считывали слово, то добавляем его
                    if (wasStart)
                    {
                        dict.AddToDictionary(line.Substring(posStart, posEnd - posStart));
                        wasStart = false; // обнуляем флаг начала
                    }
                }
            }
            // если закончили словом, то добавляем его
            if (wasStart)
                dict.AddToDictionary(line.Substring(posStart, line.Length - posStart));
        }

        // Public по условию
        /// <summary>
        /// Парсим имеющуюся информацию в dictionary с помощью Parallel - ускоряя работу, распределяя ее на потоки
        /// Модификатор Public стоит по условию ДЗ 3
        /// </summary>
        /// <param name="lines">считываемая строка</param>
        /// <returns></returns>
        public Dictionary<string, int> ParseInfoToDictionaryParallel(string[] lines)
        {
            // потокозащещенный словарь
            ConcurrentDictionaryWords dict = new ConcurrentDictionaryWords();
            // поизводим цикл для каждой строкиfc
            Parallel.ForEach(lines, line => ParseLine(line, dict));
            // возвращаем словарик
            return dict.getDictionary();
        }

        /// <summary>
        /// Парсим имеющуюся информацию в dictionary стандартно, использую 1 поток
        /// Модификатор Private стоит по условию ДЗ 2
        /// </summary>
        /// <param name="lines">считываемая строка</param>
        /// <returns></returns>
        private Dictionary<string, int> ParseInfoToDictionary(string[] lines)
        {
            // потокозащещенный словарь - влияет на скорость выполнения работы, т.к. нужно чистить флаги занятости
            DictionaryWords dict = new DictionaryWords();
            // поизводим цикл для каждой строки
            foreach (string line in lines)
                ParseLine(line, dict);
            return dict.getDictionary();
        }
    }
}

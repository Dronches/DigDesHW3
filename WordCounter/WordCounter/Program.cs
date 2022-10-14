using System;
using System.Collections.Generic;
using System.Diagnostics;
using WordCounterLib; //  - позволяет избежать необходимость упоминать пространство имен

namespace WordCounter
{

    
    /// <summary>
    /// Основной класс программы
    /// </summary>
    class Program
    {
        /// <summary>
        /// Название функции либы без распараллеливания
        /// </summary>
        private static string nameStandartFunc = "ParseInfoToDictionary";
        /// <summary>
        /// Название функции либы с распараллеливанием на потоки
        /// </summary>
        private static string nameParallelFunc = "ParseInfoToDictionaryParallel";

        /// <summary>
        /// Рефлексивно получаем из либы метод
        /// Обрабатываем массив строк
        /// Возвращаем список (результат обработки) 
        /// Записываем список по результирующему пути в удобночитаемом виде
        /// </summary>
        /// <param name="path">путь к файлу начальному</param>
        /// <param name="info">данные, передаваемые в функцию</param>
        /// <param name="nameFunc">название функции, которую будем рефлексивно получать из либы</param>
        /// <param name="publicFlag">фалг модификатора - какой уровень доступа к методу в либе</param>
        /// <param name="resultName">название файла результата</param>
        private static void parseAndSaveInfo(string path, string[] info, string nameFunc, System.Reflection.BindingFlags publicFlag, string resultName)
        {

            // получаем нужный нам метод
            var searchMethod = typeof(LineParser).GetMethod(nameFunc, System.Reflection.BindingFlags.Instance | publicFlag);
            //searchMethod = typeof(LineParser).GetMethod("ParseInfoToDictionary", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            // если полученный объект метода пустой 
            if (searchMethod is null)
            {
                Console.WriteLine("Ошибка. Не удалось получить объект библиотеки, проверьте наличие библиотек...");
                return;
            }
            else
            {
                // Создаем объект класса (он доступен в стандартном варианте, в отличие от метода)
                var lineParser = new LineParser();

                // ---- Начинаем отсчет ---
                var watch = Stopwatch.StartNew();
                // выполняем извлеченный метод - передаем данные
                var objDictionary = searchMethod?.Invoke(lineParser, parameters: new object[] { info }); // слово parameters - поясняющая мнемоника, может быть опущена          
                                                                                                         // ---- заканчиваем отсчет ---
                watch.Stop();
                // выводим результат независимо от содержания выполнения
                TimeSpan ts = watch.Elapsed;
                Console.WriteLine(String.Format("{0} сек, {1} мс", ts.Seconds, ts.Milliseconds));

                // если объект пустой 
                if (objDictionary is not null)
                    if (objDictionary.GetType() == typeof(Dictionary<string, int>))
                       FileEditor.PrintToFile(path, resultName, objDictionary as Dictionary<string, int>);
                    // если объект не пустой -> не тот тип
                    else
                        Console.WriteLine("Не удалось распознать тип результата - системная ошибка...");
                // иначе объект пустой - будет выведена ошибка, втроенная в lib
            }
        }


        /// <summary>
        /// Точка входа в программу
        /// </summary>
        /// <param name="args">Начальные аргументы (командной строки) - не используются</param>
        static void Main(string[] args)
        {
            // вывод информации о программе
            Console.Write(PathParser.info);
            // считываем путь
            Console.Write("Введите путь к файлу: ");
            string path = Console.ReadLine();

            // получаем данные
            string[] info = FileEditor.GetInfoFromPath(path);
            // выходим, если не получили данные (ошибки записаны внутри)
            if (info == null)
                return;

            /// ---------------
            /// ЗАДАНИЕ 3
            /// Реализовать двумя способами получение результата обхода словаря  
            /// С распараллеливанием задач на потоки - parallel
            /// и без распараллеливания
            /// сравнить время выполнения с помощью StopWatch()
            /// Класс - WordCounterLib.LineParser
            /// Метод - WordCounterLib.LineParser.ParseInfoToDictionaryParallel(path)
            /// --------------

            /// ---------------
            /// ЗАДАНИЕ 2
            /// Получить доступ к приватному методу из dll.
            /// Класс - WordCounterLib.LineParser
            /// Метод - WordCounterLib.LineParser.ParseInfoToDictionaryParallel(path)
            /// --------------
            /// 

            // Использование 1 потока
            Console.WriteLine("Стандартное выполнение с использованием 1 потока...");
            parseAndSaveInfo(path, info, nameStandartFunc, System.Reflection.BindingFlags.NonPublic, PathParser.resultName);
            // Использование многопоточности
            Console.WriteLine("Параллельное выполнение с использованием нескольких потоков...");
            parseAndSaveInfo(path, info, nameParallelFunc, System.Reflection.BindingFlags.Public, PathParser.resultNameParallel);
           
            // ожидаем любых действий пользователя
            Console.WriteLine("Для продолжения нажмите на любую клавишу...");
            Console.ReadKey();
        }

      

    }
}

using System;

namespace WordCounter
{
    /// <summary>
    /// класс преобразования пути
    /// </summary>
    static class PathParser
    {
        /// <summary>
        /// Переменная названия файла результата 
        /// <optimization>
        /// 1. Считывать название файла результата у пользователя
        /// 2. Считывать путь к результату (включая название) у пользователя
        /// </optimization>
        /// </summary>
        public static string resultName { get; set; } = "result.txt";
        public static string resultNameParallel { get; set; } = "resultParallel.txt";

        /// <summary>
        /// Переменная информации о программе
        /// <optimization>
        /// 1. Считывать данные с файла - не реализовано для более четкой демонстрации кода
        /// </optimization>
        /// </summary>
        public static string info = "Программа считает количество уникальных слов в заданном файле и записывает результат в файл.\n" +
            "Будьте внимательны, результат будет записан в ту же директорию c наименованими файлов: " + PathParser.resultName +
            ", " + PathParser.resultNameParallel + "!\n";

        /// <summary>
        /// Проверяем, относится ли путь к полному
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>true- путь полный, false - путь относительно директории </returns>
        public static bool IsProgramDirection(string path)
        {
            return path.Contains(":") ? false : true;
        }

        /// <summary>
        /// Преобразуем путь к пути относительно расположения программы
        /// </summary>
        /// <param name="path">Строка пути</param>
        /// <returns>Путь относительно программного расположения</returns>
        public static string ToProgramDirection(string path)
        {
            return AppDomain.CurrentDomain.BaseDirectory + path;
        }


        public static string ToCurrentDirection(string path)
        {
            return IsProgramDirection(path) ? ToProgramDirection(path) : path;
        }

        /// <summary>
        /// Преобразовать путь к файлу относительно директории в полному пути
        /// </summary>
        /// <param name="path">Входной путь к файлу (относительно директории проекта)</param>
        /// <returns>Полный путь к файлу относительно наименований носителей (дисков)</returns>
        public static string ConvertPath(string path, string name)
        {
            int posEnd = path.Length - 1;
            // идем до начала имени файла
            while (posEnd >= 0 && path[posEnd] != '/' && path[posEnd] != '\\')
                --posEnd;
            // проверяем - дериктория проекта или дериктория пользователя... в зависимости от этого строим результат
            return IsProgramDirection(path) ? ToProgramDirection(name) : path.Substring(0, posEnd + 1) + name;
        }
    }
}


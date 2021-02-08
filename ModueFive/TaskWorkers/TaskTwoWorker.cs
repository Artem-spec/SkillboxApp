using SkillBox.Infrastructure.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModueFive.TaskWorkers
{
    internal class TaskTwoWorker : TaskWork
    {
        public override void DoTask()
        {
            SetConfigTask();
            Console.WriteLine();
            NextStep();
            Console.WriteLine();
        }

        protected override void SetConfigTask()
        {
            Console.WriteLine();
            Console.WriteLine("input line:");
            var str = Console.ReadLine().Trim();

            var mergedLine = MergeLine(str);
            var check = CheckLine(mergedLine);
            if (!(check is null))
            {
                DisplayError(check);
                SetConfigTask();
            }
            else
            {
                var listMin = CheckWordsWithMinLength(mergedLine);
                var listMax = CheckWordsWithMaxLength(mergedLine);

                Console.WriteLine("\nline after merge:");
                ViewList(mergedLine, null);

                Console.WriteLine("\nlist min words:");
                ViewList(listMin, ',');

                Console.WriteLine("\nlist max words:");
                ViewList(listMax, ',');
            }
        }

        /// <summary>
        /// отображение в консоли листа с разделительным символом
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separate"></param>
        private void ViewList(List<string> list, char? separate)
        {
            var viewStr = string.Empty;

            if (separate is null)
                list.ForEach(x =>
                {
                    viewStr += $"{x} ";
                });
            else
                list.ForEach(x =>
                {
                    viewStr += $"{x}{separate} ";
                });

            viewStr = viewStr.TrimEnd(new char[] { ',', ' ' });
            Console.WriteLine($"{viewStr}");
        }

        /// <summary>
        /// преобразование введённйо строки - удаление лишних символов
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<string> MergeLine(string line)
        {
            var result = new List<string>();

            result = new List<string>(
                line.Split(new char[] { ',', ' ', '.' }))
               .Where(x => x.Length != 0)
                .OrderBy(x => x.Length)
                .ToList();

            return result;
        }

        /// <summary>
        /// поиск слов с наименьшей длиной
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<string> CheckWordsWithMinLength(List<string> list)
        {
            var result = new List<string>();

            var orderedList = list.OrderBy(x => x.Length).ToList();

            var minForFilter = orderedList.First().Length;
            result = orderedList.FindAll(x => x.Length == minForFilter);

            return result;
        }

        /// <summary>
        /// поиск слов с наибольшей длиной
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<string> CheckWordsWithMaxLength(List<string> list)
        {
            var result = new List<string>();

            var orderedList = list.OrderByDescending(x => x.Length).ToList();

            var minForFilter = orderedList.First().Length;
            result = orderedList.FindAll(x => x.Length == minForFilter);

            return result;
        }

        /// <summary>
        /// проверка на соответствие строки формату
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string CheckLine(List<string> line)
        {
            if (line.Count == 0)
                return "found empty line";

            return null;
        }
    }
}

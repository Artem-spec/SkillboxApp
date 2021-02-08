using ModueFive.Model;
using SkillBox.Infrastructure.Builder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ModueFive.TaskWorkers
{
    internal class TaskFoureWorker : TaskWork
    {

        private NumberFormatInfo Provider { get; set; }

        public TaskFoureWorker()
        {
            Provider = new NumberFormatInfo();
            Provider.NumberDecimalSeparator = ".";
        }

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
            Console.WriteLine("input of numbers separated ' '(space):");
            Console.WriteLine("the fractional part is written after the dot*");
            Console.WriteLine();
            var str = Console.ReadLine().Trim();

            var mergedLine = MergeLine(str);
            var check = CheckLine(mergedLine);
            if (!(check is null))
            {
                DisplayError(check);
                SetConfigTask();
            }
            else
                DisplayResultCheck(CheckType(mergedLine));
        }

        /// <summary>
        /// преобразование введённйо строки - получение списка
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<string> MergeLine(string line)
        {
            var result = new List<string>();

            result = new List<string>(
                line.Split(new char[] { ' ' }))
               .Where(x => x.Length != 0)
                .ToList();

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

            try
            {
                foreach (var word in line)
                {
                    Convert.ToDouble(word, Provider);
                }
            }
            catch (Exception)
            {
                return "not number found or invalid format";
            }

            if (line.Count <= 2)
                return "impossible to determine. Add numbers";

            return null;
        }

        private void DisplayResultCheck(ResultProgressionCheck result)
        {
            switch ((result.IsArithmetic, result.IsGeometric))
            {
                case (true, true):
                    {
                        Console.WriteLine($"is arephmetic progression with step: {result.DifArithmetic}");
                        Console.WriteLine("or");
                        Console.WriteLine($"is geometric progression with denominator: {result.DifGeometric}");
                        break;
                    }
                case (true, false):
                    {
                        Console.WriteLine($"is arephmetic progression with step: {result.DifArithmetic}");
                        break;
                    }
                case (false, true):
                    {
                        Console.WriteLine($"is geometric progression with denominator: {result.DifGeometric}");
                        break;
                    }
                case (false, false):
                    {
                        Console.WriteLine("it is not progression");
                        break;
                    }
            }
        }

        /// <summary>
        /// определение типа последовательности
        /// </summary>
        private ResultProgressionCheck CheckType(List<string> line)
        {
            List<double> listNumbe = new();

            foreach (var word in line)
            {
                listNumbe.Add(Convert.ToDouble(word, Provider));
            }

            var orderedNUmbers = listNumbe.ToArray<double>();

            bool isArithmetic = true;
            bool isGeometric = true;

            var ff1 = orderedNUmbers[1];
            var ff = orderedNUmbers[0];

            var gg = ff1 - ff;

            double difArithmetic = Math.Round(orderedNUmbers[1] - orderedNUmbers[0], 4);
            double difGeometric = Math.Round(orderedNUmbers[1] / orderedNUmbers[0], 4);

            for (int i = 1; i < orderedNUmbers.Length - 1; i++)
            {
                if (isArithmetic == true)
                    if (Math.Round(orderedNUmbers[i + 1] - orderedNUmbers[i], 4) != difArithmetic)
                        isArithmetic = false;

                if (isGeometric == true)
                    if (Math.Round(orderedNUmbers[i + 1] / orderedNUmbers[i], 4) != difGeometric)
                        isGeometric = false;
            }

            return new ResultProgressionCheck(isArithmetic, isGeometric, difArithmetic, difGeometric);
        }
    }
}

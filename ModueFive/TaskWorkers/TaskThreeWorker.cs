using SkillBox.Infrastructure.Builder;
using System;

namespace ModueFive.TaskWorkers
{
    internal class TaskThreeWorker : TaskWork
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

            if (str.Length == 0)
            {
                DisplayError("found empty line");
                SetConfigTask();
            }
            else
            {
                var mergedLine = MergeLine(str);
                Console.WriteLine("\nline after merge:");
                Console.WriteLine(mergedLine);

            }
        }

        /// <summary>
        /// преобразование введённйо строки
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string MergeLine(string line)
        {
            var result = " ";

            var counterResult = 0;
            foreach (var symbol in line)
            {
                if (!result[counterResult].Equals(symbol))
                {
                    result += symbol;
                    counterResult++;
                }
            }

            result = result.Trim();
            return result;
        }
    }
}

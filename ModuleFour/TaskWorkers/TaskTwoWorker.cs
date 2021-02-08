using SkillBox.Infrastructure.Builder;
using System;

namespace ModuleFour.TaskWorkers
{
    internal class TaskTwoWorker : TaskWork
    {
        private int CountRow { get; set; }

        /// <summary>
        /// выполнение задачи
        /// </summary>
        public override void DoTask()
        {
            SetConfigTask();
            Console.WriteLine();
            ConstrucrPascal();
            Console.WriteLine();
            NextStep();
            Console.WriteLine();
        }

        /// <summary>
        /// получение информации по заданию данных для выполнения
        /// </summary>
        protected override void SetConfigTask()
        {
            try
            {
                Console.WriteLine("input count row:");
                var countRow = Convert.ToInt32(Console.ReadLine());
                if (countRow > 25)
                    throw new ArgumentException();
                CountRow = countRow;
            }
            catch (Exception)
            {
                DisplayError("incorrect input - only number <= 25");
                SetConfigTask();
            }
        }

        /// <summary>
        /// создание и отображение треугольника паскаля
        /// </summary>
        private void ConstrucrPascal()
        {
            for (var i = 0; i < CountRow; i++)
            {
                for (var c = 0; c <= (CountRow - i); c++)
                {
                    Console.Write("{0,-3}", " ");
                }
                for (var c = 0; c <= i; c++)
                {
                    Console.Write("{0,-3}", " ");
                    Console.Write("{0,-3}", SetFactorial(i) / (SetFactorial(c) * SetFactorial(i - c)));
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// расчёт факториала
        /// </summary>
        private float SetFactorial(int countRow)
        {
            float fact = 1;
            for (var i = 1; i <= countRow; i++)
            {
                fact *= i;
            }
            return fact;
        }
    }
}

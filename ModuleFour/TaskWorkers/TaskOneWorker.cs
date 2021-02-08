using ModuleFour.Model;
using SkillBox.Infrastructure.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleFour.TaskWorkers
{
    internal class TaskOneWorker : TaskWork
    {
        private List<MonthMoneyData> MonthData { get; set; }

        public TaskOneWorker()
        {
            MonthData = new List<MonthMoneyData>();
        }

        /// <summary>
        /// выполнение задачи
        /// </summary>
        public override void DoTask()
        {
            SetConfigTask();
            Console.WriteLine();
            ViewDataOnConsole();
            Console.WriteLine();
            ViewPositiveDiff();
            ViewBestNegativeDiff();
            Console.WriteLine();
            NextStep();
            Console.WriteLine();
        }

        /// <summary>
        /// получение информации по заданию данных для выполнения
        /// </summary>
        protected override void SetConfigTask()
        {
            Console.WriteLine("1 - manual data filling");
            Console.WriteLine("2 - automatic data filling");
            Console.WriteLine("chose:");

            var numberTask = Console.ReadLine();
            switch (numberTask)
            {
                case "1":
                    {
                        ManualMonthDataFilling();
                        break;
                    }
                case "2":
                    {
                        AutoMonthDataFilling();
                        break;
                    }
                default:
                    {
                        DisplayError("incorrect - chose 1 or 2");
                        SetConfigTask();
                        break;
                    }
            }
        }

        /// <summary>
        /// автомотическое заполнение данных по месяцам
        /// </summary>
        private void AutoMonthDataFilling()
        {
            Random rnd = new();
            foreach (var month in Enum.GetValues(typeof(TypeMonth)))
            {
                var incom = rnd.Next(10000);
                var consumption = rnd.Next(10000);
                MonthMoneyData newMonthData = new((TypeMonth)month, incom, consumption);
                MonthData.Add(newMonthData);
            }
        }

        /// <summary>
        /// ручное заплнение данных по месяцам
        /// </summary>
        private void ManualMonthDataFilling()
        {
            foreach (var month in Enum.GetValues(typeof(TypeMonth)))
            {
                InputOneMonth((TypeMonth)month);
            }
        }

        /// <summary>
        /// ввод данных по каждому месяцу
        /// </summary>
        private void InputOneMonth(TypeMonth month)
        {
            try
            {
                Console.WriteLine($"Month: {Enum.GetName(typeof(TypeMonth), month)}");
                Console.WriteLine("Income value: ");
                var incom = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Consumption value:");
                var consumption = Convert.ToInt32(Console.ReadLine());

                MonthMoneyData newMonthData = new((TypeMonth)month, incom, consumption);
                MonthData.Add(newMonthData);

            }
            catch (Exception)
            {
                DisplayError("incorrect input - only number - try again");
                InputOneMonth(month);
            }
        }

        /// <summary>
        /// отображение в консоле полученных данных по месяцам
        /// </summary>
        private void ViewDataOnConsole()
        {
            if (MonthData.Any())
            {
                var pattern = "{0,-20} {1,-20} {2,-20} {3,-20}";
                Console.WriteLine(pattern, "Месяц ", "Доход,тыс.руб.", " Расход,тыс.руб.", " Прибыль,тыс.руб.");
                foreach (var data in MonthData)
                {
                    Console.WriteLine(pattern,
                        Enum.GetName(typeof(TypeMonth), data.Month) + $"({(int)data.Month + 1})",
                        data.Income,
                        data.Consumption,
                        data.Diff);
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("month data is empty");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        /// <summary>
        /// отображение кол-ва месяцев с положительной прибылью
        /// </summary>
        private void ViewPositiveDiff()
        {
            var count = 0;
            MonthData.ForEach(month =>
            {
                if (month.Diff > 0) count++;
            });

            Console.WriteLine($"Месяцев с положительной прибылью: {count}");
        }

        /// <summary>
        /// вывод 3-х худших показателей
        /// </summary>
        private void ViewBestNegativeDiff()
        {
            var result = string.Empty;

            var orderedByDiffListMonth = MonthData.OrderBy(x => x.Diff);

            var countSelectedMonth = 0;
            var lastDif = 0;
            foreach (var month in orderedByDiffListMonth)
            {
                if (countSelectedMonth >= 3)
                    break;

                result += $"{Enum.GetName(typeof(TypeMonth), month.Month)}({(int)month.Month + 1}), ";

                if (lastDif != month.Diff || countSelectedMonth == 0)
                {
                    countSelectedMonth++;
                }
                lastDif = month.Diff;
            }

            result = result.TrimEnd(new char[] { ',', ' ' });
            Console.WriteLine($"Худшая прибыль в месяцах: {result}");
        }


    }
}

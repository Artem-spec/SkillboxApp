using System;

namespace SkillBox.Infrastructure.Builder
{
    public abstract class TaskWork
    {
        /// <summary>
        /// основной метод выполнение задачи
        /// </summary>
        public abstract void DoTask();

        /// <summary>
        /// получение необходимых данных для выполнения
        /// </summary>
        protected abstract void SetConfigTask();

        /// <summary>
        /// переход из задачи при завершении работы
        /// </summary>
        protected virtual void NextStep()
        {
            Console.WriteLine("for continue press any key...");
            Console.ReadKey();
        }

        /// <summary>
        /// вывод в консоль ошибки
        /// </summary>
        protected virtual void DisplayError(string message)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }

        /// <summary>
        /// ввод числа с указание сообщения и огранчиением на мин.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected int InputWholeNumber(int? min, string text)
        {
            try
            {
                Console.WriteLine(text);
                var number = Convert.ToInt32(Console.ReadLine());
                if (!(min is null))
                    if (number < min)
                        throw new ArgumentException();

                return number;
            }
            catch (Exception)
            {
                if (min is null)
                    DisplayError("incorrect - only number");
                else
                    DisplayError($"incorrect - only whole number >= {min}");
                return InputWholeNumber(min, text);
            }

        }

    }
}

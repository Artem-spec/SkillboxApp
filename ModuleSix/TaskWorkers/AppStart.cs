using ModuleSix.Builder;
using ModuleSix.Model;
using System;
using System.Diagnostics;

namespace ModuleSix.TaskWorkers
{
    internal class AppStart
    {
        /// <summary>
        /// выбор заания на выполнение
        /// </summary>
        public static void ChoseTask()
        {
            try
            {
                Console.WriteLine("1 - task 1");
                Console.WriteLine("2 - exit");
                Console.WriteLine("Chose number task:");

                var numberTask = Console.ReadLine();

                ModuleFiveTaskWorkerCreator taskCreator = new();
                switch (numberTask)
                {
                    case "1":
                        {
                            taskCreator.CreateWorkerForTask(TypeTask.TaskOne)
                                .DoTask();
                            ChoseTask();
                            break;
                        }
                    case "2":
                        {
                            Process.GetCurrentProcess().Kill();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine();
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("incorrect - chose one of 1, 2");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.WriteLine();
                            ChoseTask();
                            break;
                        }
                }
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("found exception - try again");
                Console.BackgroundColor = ConsoleColor.Black;
                ChoseTask();
            }

        }
    }
}

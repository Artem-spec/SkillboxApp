using ModuleFour.Builder;
using ModuleFour.Model;
using System;
using System.Diagnostics;

namespace ModuleFour.TaskWorkers
{
    internal static class AppStart
    {
        /// <summary>
        /// выбор заания на выполнение
        /// </summary>
        public static void ChoseTask()
        {
            try
            {
                Console.WriteLine("1 - task 1");
                Console.WriteLine("2 - task 2");
                Console.WriteLine("3 - task 3");
                Console.WriteLine("4 - exit");
                Console.WriteLine("Chose number task:");

                var numberTask = Console.ReadLine();

                ModuleFoureTaskWorkerCreator taskCreator = new();
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
                            taskCreator.CreateWorkerForTask(TypeTask.TaskTwo)
                                .DoTask();
                            ChoseTask();
                            break;
                        }
                    case "3":
                        {
                            taskCreator.CreateWorkerForTask(TypeTask.TaskThree)
                                .DoTask();
                            ChoseTask();
                            break;
                        }
                    case "4":
                        {
                            Process.GetCurrentProcess().Kill();
                            break;
                        }
                    default:
                        {

                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("incorrect - chose one of 1, 2, 3, 4");
                            Console.BackgroundColor = ConsoleColor.Black;
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

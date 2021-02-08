using ModueFive.Builder;
using ModueFive.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ModueFive.TaskWorkers
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
                Console.WriteLine("2 - task 2");
                Console.WriteLine("3 - task 3");
                Console.WriteLine("4 - task 4");
                Console.WriteLine("5 - task 5");
                Console.WriteLine("6 - exit");
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
                            taskCreator.CreateWorkerForTask(TypeTask.TaskFoure)
                                .DoTask();
                            ChoseTask();
                            break;
                        }
                    case "5":
                        {
                            taskCreator.CreateWorkerForTask(TypeTask.TaskFive)
                               .DoTask();
                            ChoseTask();
                            break;
                        }
                    case "6":
                        {
                            Process.GetCurrentProcess().Kill();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine();
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("incorrect - chose one of 1, 2, 3, 4, 5, 6");
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

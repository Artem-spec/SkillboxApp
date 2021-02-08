using System;

namespace Homework_01
{
    class Program
    {


        static void Main(string[] args)
        {
            Repository repository = null;

            ChooseSelection(repository);

            Console.ReadKey();

        }

        private static void ChooseSelection(Repository repository)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Задание 1 - введите 1");
            Console.WriteLine("Задание 2 - введите 2");
            Console.WriteLine("Задание 3 - введите 3");
            Console.Write("Введите пункт меню: ");
            string chooseTask = Console.ReadLine();
            switch (chooseTask)
            {
                case "1":
                    Console.WriteLine("Задание 1");
                    TaskOne(repository);
                    break;
                case "2":
                    Console.WriteLine("Задание 2");
                    TaskTwo(repository);
                    break;
                case "3":
                    Console.WriteLine("Задание 3");
                    TaskThree(repository);
                    break;
                default:
                    Console.WriteLine("Такого пункта нет");
                    ChooseSelection(repository);
                    break;
            }
        }


        private static void TaskOne(Repository repository)
        {
            repository = new Repository(20);
            repository.Print("База данных c 20 сотрудниками");
            ChooseSelection(repository);
        }
        private static void TaskTwo(Repository repository)
        {
            repository = new Repository(40);
            repository.Print("База данных c 40 сотрудниками");
            repository.DeleteRandomWorker(14);
            repository.Print("База данных после увольнений");
            ChooseSelection(repository);
        }
        private static void TaskThree(Repository repository)
        {
            repository = new Repository(50);
            repository.Print("База данных c 50 сотрудниками");
            repository.DeleteWorkerBySalary(30000);
            repository.Print("База данных после увольнений");
            ChooseSelection(repository);
        }
    }
}

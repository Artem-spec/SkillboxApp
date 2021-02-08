using ModuleThree.Service;
using System;
using System.Collections.Generic;

namespace ModuleThree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("if one player is selected - the opponent will be the computer");

            StartGame();
        }

        /// <summary>
        /// запуск игры
        /// </summary>
        private static void StartGame()
        {
            Console.WriteLine($"change game settings? (Y/N)");
            if (Console.ReadLine().Trim().ToLower().Equals("y"))
                Game.ChangeSettings();
            else
                Game.SetDefaoultSettings();

            Console.WriteLine("\nGame started");
            Console.WriteLine("input count player");
            var countPlayer = 0;
            do
            {
                countPlayer = InputCountPlayer();
            } while (countPlayer == 0);

            var nicks = InputNickName(countPlayer);

            var winner = Game.StartGame(nicks);
            Console.WriteLine($"win {winner.NickName}!!\n");

            Console.WriteLine("");
            Console.WriteLine($"view statistics? (Y/N)");
            if (Console.ReadLine().Trim().ToLower().Equals("y"))
                Game.ViewStatistic();

            Console.WriteLine("");
            Console.WriteLine($"play again? (Y/N)");
            if (Console.ReadLine().Trim().ToLower().Equals("y"))
                StartGame();
        }

        /// <summary>
        /// ввод кол-ва играков
        /// </summary>
        /// <returns></returns>
        private static int InputCountPlayer()
        {
            var countStr = Console.ReadLine();
            try
            {
                var count = Convert.ToInt32(countStr);
                if (count == 0)
                    throw new Exception("incorrect count player");
                return count;
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("incorrect - try again - only number > 0");
                Console.BackgroundColor = ConsoleColor.Black;
                return 0;
            }

        }

        /// <summary>
        /// заполнение никнеймов
        /// </summary>
        /// <returns></returns>
        private static List<string> InputNickName(int countPlayer)
        {
            var result = new List<string>();

            for (var i = 0; i < countPlayer; i++)
            {
                Console.WriteLine($"input nickname {i + 1} player");
                var nickname = Input(result);
                result.Add(nickname);
            }

            return result;
        }

        /// <summary>
        /// ввод никнеймов
        /// </summary>
        /// <returns></returns>
        private static string Input(List<string> list)
        {
            var nickname = Console.ReadLine();
            if (list.Find(x => x.Trim().ToLower().Equals(nickname.Trim().ToLower())) != null)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("incorrect - nickname already used");
                Console.BackgroundColor = ConsoleColor.Black;
                Input(list);
            }
            if (nickname.Trim().ToLower().Equals("comp"))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("incorrect - nickname already used - comp player");
                Console.BackgroundColor = ConsoleColor.Black;
                Input(list);
            }

            return nickname;
        }
    }
}

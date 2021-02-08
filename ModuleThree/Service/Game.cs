using ModuleThree.Model;
using System;
using System.Collections.Generic;

namespace ModuleThree.Service
{
    internal static class Game
    {
        private static int GameNumber { get; set; }
        private static LinkedListNode<Player> NowMove { get; set; }

        private static int MaxUserTry { get; set; }
        private static int MaxGameNumber { get; set; }
        private static int MinGameNumber { get; set; }

        private static LinkedList<Player> Players { get; set; }

        /// <summary>
        /// старт игры
        /// </summary>
        /// <param name="nicks"></param>
        public static Player StartGame(List<string> nicks)
        {
            InitGame(nicks);
            Console.WriteLine($"\nnow GameNumber: {GameNumber}\n");

            while (GameNumber > 0)
            {
                switch (NowMove.Value.NickName)
                {
                    case "comp":
                        {
                            CompMove();
                            break;
                        }
                    default:
                        {
                            PlayerMove();
                            break;
                        }
                }
                Console.WriteLine($"\nnow GameNumber: {GameNumber}\n");
                NowMove = NowMove.Next != null ? NowMove.Next : Players.First;
            };
            return NowMove.Previous?.Value ?? Players.Last.Value;
        }

        /// <summary>
        /// ход компьютера
        /// </summary>
        private static void CompMove()
        {
            Random rnd = new Random();
            var turn = rnd.Next(1, MaxUserTry + 1);
            Console.WriteLine($"computer turn:\n{turn}");
            NowMove.Value.Moves.Add(turn);
            GameNumber -= turn;
        }

        /// <summary>
        /// ход игрока
        /// </summary>
        private static void PlayerMove()
        {
            Console.WriteLine($"player {NowMove.Value.NickName} turn:");
            try
            {
                var turn = Convert.ToInt32(Console.ReadLine());
                if (turn < 1 || turn > MaxUserTry)
                    throw new Exception("incorrect turn - try again");

                NowMove.Value.Moves.Add(turn);
                GameNumber -= turn;
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("incorrect - try again");
                Console.BackgroundColor = ConsoleColor.Black;
                PlayerMove();
            }

        }

        /// <summary>
        /// инициализация игры
        /// </summary>
        /// <param name="nicks"></param>
        private static void InitGame(List<string> nicks)
        {
            Random rnd = new Random();
            GameNumber = rnd.Next(MinGameNumber, MaxGameNumber + 1);

            Players = new LinkedList<Player>();

            nicks.ForEach(x =>
            {
                Player newPlayer = new Player(x);
                Players.AddLast(newPlayer);
            });

            if (Players.Count == 1)
            {
                Players.AddLast(new Player("comp"));
            }

            NowMove = Players.First;
        }

        /// <summary>
        /// отображение статистики по игре
        /// </summary>
        public static void ViewStatistic()
        {
            Console.WriteLine($"");
            foreach (var player in Players)
            {
                Console.WriteLine($"Player: {player.NickName}");
                Console.WriteLine($"Moves:");
                foreach (var move in player.Moves)
                {
                    Console.Write($"{move},");
                }
                Console.WriteLine($"");
            }
        }

        /// <summary>
        /// установка параметров по умолчанию
        /// </summary>
        public static void SetDefaoultSettings()
        {
            MaxUserTry = 4;
            MaxGameNumber = 120;
            MinGameNumber = 12;
        }

        /// <summary>
        /// изменение настроек
        /// </summary>
        public static void ChangeSettings()
        {
            Console.WriteLine("");
            Console.WriteLine("1 - change MaxUserTry");
            Console.WriteLine("2 - change GameNumber");
            Console.WriteLine("3 - exit");

            var chose = Console.ReadLine();

            switch (chose)
            {
                case "1":
                    {
                        InputMaxUserTry();
                        ChangeSettings();
                        break;
                    }
                case "2":
                    {
                        InputMinGameNumber();
                        InputMaxGameNumber();
                        ChangeSettings();
                        break;
                    }
                default:
                    {
                        ChangeSettings();
                        break;
                    }
            }
        }

        /// <summary>
        /// ввод MaxUserTry
        /// </summary>
        private static void InputMaxUserTry()
        {
            Console.WriteLine("input new max userTry:");
            try
            {
                var numb = Convert.ToInt32(Console.ReadLine());
                if (numb <= 0)
                    throw new Exception();
                MaxUserTry = numb;
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("incorrect - try again - only number > 0");
                Console.BackgroundColor = ConsoleColor.Black;
                InputMaxUserTry();
            }
        }

        /// <summary>
        /// ввод MaxGameNumber
        /// </summary>
        private static void InputMaxGameNumber()
        {
            Console.WriteLine("input new max GameNumber:");
            try
            {
                var numb = Convert.ToInt32(Console.ReadLine());
                if (numb <= 0)
                    throw new Exception();
                if (numb <= MinGameNumber)
                    throw new Exception();
                MaxGameNumber = numb;
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"incorrect - try again - only number > 0 and > {MinGameNumber}");
                Console.BackgroundColor = ConsoleColor.Black;
                InputMaxGameNumber();
            }
        }

        /// <summary>
        /// ввод MinGameNumber
        /// </summary>
        private static void InputMinGameNumber()
        {
            Console.WriteLine("input new min GameNumber:");
            try
            {
                var numb = Convert.ToInt32(Console.ReadLine());
                if (numb <= 0)
                    throw new Exception();
                MinGameNumber = numb;
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("incorrect - try again - only number > 0");
                Console.BackgroundColor = ConsoleColor.Black;
                InputMinGameNumber();
            }
        }
    }
}

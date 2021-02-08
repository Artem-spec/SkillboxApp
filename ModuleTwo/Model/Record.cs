using System;
using System.Collections.Generic;

namespace ModuleTwo.Model
{
    public class Record
    {
        public string FName { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public List<EducationBall> EducationBalls { get; set; }

        private readonly string[] RndLetter = new string[]
        { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м" };

        public Record(string fName, int age, int height, List<EducationBall> educationBalls)
        {
            FName = fName;
            Age = age;
            Height = height;
            EducationBalls = educationBalls;
        }

        public Record()
        {
            var rnd = new Random();

            Age = rnd.Next(1, 89);
            Height = rnd.Next(100, 199);
            FName = GetRandomName();
            EducationBalls = setRandomMarks();
        }

        /// <summary>
        /// задание имени из случайных букв
        /// </summary>
        /// <returns></returns>
        private string GetRandomName()
        {
            var rnd = new Random();
            var result = string.Empty;

            for (var i = 0; i < 8; i++)
            {
                string letter = RndLetter[rnd.Next(0, RndLetter.Length)];
                result += letter;
            }

            return result;
        }

        /// <summary>
        /// задание случайных оценок по предметам
        /// </summary>
        /// <returns></returns>
        private List<EducationBall> setRandomMarks()
        {
            var result = new List<EducationBall>();
            var rnd = new Random();

            foreach (Subjects sub in Enum.GetValues(typeof(Subjects)))
            {
                var mark = (byte)rnd.Next(1, 5);
                var ball = new EducationBall(mark, sub);
                result.Add(ball);
            }

            return result;
        }
    }
}

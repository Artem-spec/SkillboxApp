using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleTwo.Model
{
    public class NoteBook
    {
        private List<Record> Records { get; set; }

        public NoteBook()
        {
            Records = new List<Record>();
        }

        /// <summary>
        /// добавление записи
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns></returns>
        public bool AddRecord(Record newRecord)
        {
            if (Records.FindIndex(x => x.FName.Equals(newRecord)) == -1)
            {
                Records.Add(newRecord);
                return true;
            }
            return false;
        }

        /// <summary>
        /// расчёт среднего значения оценок
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetAvgBall(string name)
        {
            var record = Records.Find(x => x.FName.Equals(name));
            if (record == null)
                return default;

            var summ = 0;
            record.EducationBalls.ForEach(x =>
            {
                summ += x.Mark;
            });
            return (double)summ / record.EducationBalls.Count;
        }

        /// <summary>
        /// переопределение метода ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = string.Empty;

            foreach (var record in Records)
            {
                result += $" Имя: {record.FName}\n Возраст: {record.Age}\n " +
                    $"Рост: {record.FName}\n Баллы:\n{GetStrBall(record.EducationBalls)} " +
                    $"Среднее баллов: {GetAvgBall(record.FName):#.##}\n\n";
            }

            return result;
        }

        /// <summary>
        /// получение строки для вывода предметов и оценок
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetStrBall(List<EducationBall> list)
        {
            var result = string.Empty;

            if (!list.Any())
                return "пусто/n";

            list.ForEach(x =>
            {
                result += $" Предмет: {Enum.GetName(typeof(Subjects), x.Subject)} оценка: {x.Mark}\n";
            });

            return result;
        }
    }
}

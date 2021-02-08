using ModuleTwo.Model;
using System;
using System.Text.RegularExpressions;

namespace ModuleTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            var noteBook = new NoteBook();

            var recordOne = new Record();
            var recordTwo = new Record();

            noteBook.AddRecord(recordOne);
            noteBook.AddRecord(recordTwo);

            CenterOutPut(noteBook);
            BasicOutPut(noteBook);

        }

        private static void CenterOutPut(NoteBook noteBook)
        {
            var strToView = noteBook.ToString();
            string[] linesStr = Regex.Split(strToView, "\n");

            int center = Console.WindowWidth / 2;
            int left = center - linesStr[0]?.Length ?? 0 / 2;
            int top = (Console.WindowHeight / 2) - (linesStr.Length / 2) - 1;


            for (int i = 0; i < linesStr.Length; i++)
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine(linesStr[i]);
                top = Console.CursorTop;
            }
        }

        private static void BasicOutPut(NoteBook noteBook)
        {
            Console.WriteLine(noteBook);
        }
    }
}

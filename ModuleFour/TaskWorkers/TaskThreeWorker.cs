using SkillBox.Infrastructure.Builder;
using System;

namespace ModuleFour.TaskWorkers
{
    public class TaskThreeWorker : TaskWork
    {
        protected static int origRow;
        protected static int origCol;

        /// <summary>
        /// выполнение задачи
        /// </summary>
        public override void DoTask()
        {
            SetConfigTask();
            Console.WriteLine();
        }

        /// <summary>
        /// получение информации по заданию данных для выполнения
        /// </summary>
        protected override void SetConfigTask()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("chose number subtask");
            Console.WriteLine("1 - subtask 1");
            Console.WriteLine("2 - subtask 2");
            Console.WriteLine("3 - subtask 3");
            Console.WriteLine("4 - menu");

            var numberTask = Console.ReadLine();
            switch (numberTask)
            {
                case "1":
                    {
                        DoSubTaskOne();
                        break;
                    }
                case "2":
                    {
                        DoSubTaskTwo();
                        break;
                    }
                case "3":
                    {
                        DoSubTaskThree();
                        break;
                    }
                case "4":
                    {
                        NextStep();
                        break;
                    }
                default:
                    {
                        DisplayError("incorrect - chose 1 or 2 or 3 or 4");
                        SetConfigTask();
                        break;
                    }
            }
        }

        /// <summary>
        /// ввод числа
        /// </summary>
        /// <param name="min"></param>
        /// <returns></returns>
        private int InputNumber(int? min)
        {
            try
            {
                var number = Convert.ToInt32(Console.ReadLine());
                if (!(min is null))
                    if (number <= min)
                        throw new ArgumentException();

                return number;
            }
            catch (Exception)
            {
                if (min is null)
                    DisplayError("incorrect - only number");
                else
                    DisplayError($"incorrect - only number > {min}");
                return InputNumber(min);
            }

        }

        /// <summary>
        /// создание и заполнение матрицы слуяайными числами
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int[,] GetRndMatrix(int row, int col)
        {
            Random rnd = new();
            var result = new int[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = rnd.Next(10);
                }
            }

            return result;
        }

        /// <summary>
        /// вывод на консоль матрицы относительно начальной позиции
        /// </summary>
        /// <param name="matrix"></param>
        private void DrawMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.SetCursorPosition(origCol, origRow + i);
                Console.Write("|");
            }

            var mergeOrigCol = origCol;
            var mergeOrigRow = origRow;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.SetCursorPosition(mergeOrigCol + j + 1, mergeOrigRow);
                    Console.Write(matrix[i, j]);
                    mergeOrigCol += 3;
                }
                mergeOrigCol = origCol;
                mergeOrigRow++;
            }
            origCol += 4 * matrix.GetLength(1);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.SetCursorPosition(origCol, origRow + i);
                Console.Write("|");
            }
            origCol += 3;
        }

        /// <summary>
        /// вывод в консоль симвла относительно начальнйо позиции
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="row"></param>
        private void DrawSymbol(string symbol, int row)
        {
            Console.SetCursorPosition(origCol, origRow + row / 2);
            Console.Write($" {symbol} ");
            origCol += 4;
        }

        /// <summary>
        /// подзадача 1
        /// умножение матрицы на число
        /// </summary>
        private void DoSubTaskOne()
        {
            Console.WriteLine("input multiplier:");
            var mult = InputNumber(null);

            Console.WriteLine("input count row:");
            var row = InputNumber(0);

            Console.WriteLine("input count column:");
            var column = InputNumber(0);

            var matrix = GetRndMatrix(row, column);

            origRow = Console.CursorTop;
            origCol = Console.CursorLeft + 5;

            Console.WriteLine();
            DrawSymbol(mult.ToString(), row);
            DrawSymbol("*", row);
            DrawMatrix(matrix);
            DrawSymbol("=", row);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = matrix[i, j] * mult;
                }
            }
            DrawMatrix(matrix);

            SetConfigTask();
        }

        /// <summary>
        /// подзадача 2
        /// сложение матриц
        /// </summary>
        private void DoSubTaskTwo()
        {
            Console.WriteLine("input count row first matrix:");
            var rowMOne = InputNumber(0);

            Console.WriteLine("input count column first matrix:");
            var columnMOne = InputNumber(0);

            Console.WriteLine("input count row second matrix:");
            var rowMTwo = InputNumber(0);

            Console.WriteLine("input count column second matrix:");
            var columnMTwo = InputNumber(0);

            if (rowMOne != rowMTwo || columnMOne != columnMTwo)
            {
                DisplayError("the number of rows and columns did not match");
                DoSubTaskTwo();
            }
            else
            {
                var matrixOne = GetRndMatrix(rowMOne, columnMOne);
                var matrixTwo = GetRndMatrix(rowMTwo, columnMTwo);

                origRow = Console.CursorTop;
                origCol = Console.CursorLeft + 5;

                Console.WriteLine();
                DrawMatrix(matrixOne);
                DrawSymbol("+", rowMOne);
                DrawMatrix(matrixTwo);
                DrawSymbol("=", rowMOne);

                DrawMatrix(SummMatrix(matrixOne, matrixTwo));

                SetConfigTask();
            }
        }

        /// <summary>
        /// сложение матриц
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int[,] SummMatrix(int[,] first, int[,] second)
        {
            int[,] resultMatrix = new int[first.GetLength(0), second.GetLength(1)];
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < first.GetLength(1); j++)
                {
                    resultMatrix[i, j] = first[i, j] + second[i, j];
                }
            }
            return resultMatrix;
        }

        /// <summary>
        /// подзадача 3
        /// перемножение матриц
        /// </summary>
        private void DoSubTaskThree()
        {
            Console.WriteLine("input count row first matrix:");
            var rowMOne = InputNumber(0);

            Console.WriteLine("input count column first matrix:");
            var columnMOne = InputNumber(0);

            Console.WriteLine("input count row second matrix:");
            var rowMTwo = InputNumber(0);

            Console.WriteLine("input count column second matrix:");
            var columnMTwo = InputNumber(0);

            if (columnMOne != rowMTwo)
            {
                DisplayError("the count of column first matrix not match with count of row second matrix");
                DoSubTaskTwo();
            }
            else
            {
                var matrixOne = GetRndMatrix(rowMOne, columnMOne);
                var matrixTwo = GetRndMatrix(rowMTwo, columnMTwo);

                origRow = Console.CursorTop;
                origCol = Console.CursorLeft + 5;

                Console.WriteLine();
                DrawMatrix(matrixOne);
                DrawSymbol("*", rowMOne);
                DrawMatrix(matrixTwo);
                DrawSymbol("=", rowMOne);

                DrawMatrix(Multiplication(matrixOne, matrixTwo));

                SetConfigTask();
            }
        }

        /// <summary>
        /// перемнжение матриц
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int[,] Multiplication(int[,] first, int[,] second)
        {
            int[,] resultMatrix = new int[first.GetLength(0), second.GetLength(1)];
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < second.GetLength(1); j++)
                {
                    for (int k = 0; k < second.GetLength(0); k++)
                    {
                        resultMatrix[i, j] += first[i, k] * second[k, j];
                    }
                }
            }
            return resultMatrix;
        }
    }
}

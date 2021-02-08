using ModuleSix.Model;
using SkillBox.Infrastructure.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ModuleSix.TaskWorkers
{
    internal class TaskOneWorker : TaskWork
    {
        private int CountNumber { get; set; }

        public override void DoTask()
        {
            SetConfigTask();
            Console.WriteLine();

            Stopwatch sw = new();

            sw.Start();
            // var groups = GetListGroup(CountNumber);
            var groups = GetArrGroup(CountNumber);
            sw.Stop();

            TimeSpan time = sw.Elapsed;

            // ViewGroup(groups);
            Console.WriteLine($"time: {time.TotalSeconds} sec");
            Console.WriteLine($"total group(M): {groups.Count()}");

            Console.WriteLine("\nsave to file?(y/n)");
            if (Console.ReadLine().ToLower().Equals("y"))
            {
                var baseDir = AppContext.BaseDirectory;
                var pathToDirectory = Path.Combine(baseDir, "result");

                try
                {
                    var directory = new DirectoryInfo(pathToDirectory);
                    if (!directory.Exists)
                        directory.Create();

                    var pathToFile = Path.Combine(pathToDirectory, $"res_for_{CountNumber}.txt");
                    using StreamWriter stream = new StreamWriter(pathToFile, false, System.Text.Encoding.Default);

                    foreach (var item in groups)
                    {
                        stream.WriteLine("group:");
                        item.ListDiv.ForEach(x => stream.Write(x + " "));
                        stream.WriteLine();
                    }

                    stream.Flush();

                    Console.WriteLine($"\ndone\npath: {pathToFile}");

                    Console.WriteLine("\narchive?(y/n)");
                    if (Console.ReadLine().ToLower().Equals("y"))
                    {
                        stream.Close();
                        var pathToFileArch = Path.Combine(pathToDirectory, $"res_for_{CountNumber}.zip");
                        Compress(pathToFile, pathToFileArch);
                    }

                }
                catch (Exception e)
                {
                    DisplayError(e.Message);
                }
            }


            NextStep();
            Console.WriteLine();
        }

        protected override void SetConfigTask()
        {
            Console.WriteLine();
            Console.WriteLine("partition of the number N into M groups of not multiple divisors");
            CountNumber = InputWholeNumber(0, "input n parametr:");
        }

        // time: 0,0081948 sec with 1000 iter
        // time: 0,1748855 sec with 1000000 iter
        // time: 180,3094892 sec with 1000000000 iter
        /// <summary>
        /// получение списка групп
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private IEnumerable<GroupDiv> GetListGroup(int n)
        {
            List<GroupDiv> result = new List<GroupDiv>();

            for (int i = 1; i < n + 1; i++)
            {
                GroupDiv groupDiv = null;

                foreach (var r in result)
                {
                    var f = r.ListDiv.First();
                    if (i % f != 0)
                    {
                        groupDiv = r;
                        break;
                    }
                }

                #region old alg

                //groupDiv = result.Find(x =>
                //    x.ListDiv.Select(y => (i % y) == 0)
                //    .ToList()
                //    .All(x => !x));

                #endregion

                if (groupDiv == null)
                {
                    result.Add(new());
                    result[result.Count - 1].ListDiv.Add(i);
                }
                else
                    groupDiv.ListDiv.Add(i);
            }
            return result;
        }

        // time: 0,0004258 sec with 1000 iter
        // time: 0,1412083 sec with 1000000 iter
        // time: 148,7030095 sec with 1000000000 iter
        /// <summary>
        /// получение массива групп
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private IEnumerable<GroupDiv> GetArrGroup(int n)
        {

            GroupDiv[] result = new GroupDiv[0];

            for (int i = 1; i < n + 1; i++)
            {
                int groupDivInd = -1;

                for (var c = 0; c < result.Length; c++)
                {
                    var f = result[c].ListDiv.First();
                    if (i % f != 0)
                    {
                        groupDivInd = c;
                        break;
                    }
                }

                if (groupDivInd == -1)
                {
                    var newResult = new GroupDiv[result.Length + 1];
                    result.CopyTo(newResult, 0);
                    newResult[newResult.Length - 1] = new();

                    newResult[newResult.Length - 1].ListDiv.Add(i);

                    result = newResult;
                }
                else
                    result[groupDivInd].ListDiv.Add(i);

            }
            return result;
        }

        /// <summary>
        /// отображение групп
        /// </summary>
        /// <param name="list"></param>
        private void ViewGroup(IEnumerable<GroupDiv> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("group:");
                item.ListDiv.ForEach(x => Console.Write(x + " "));
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate);

            // поток для записи сжатого файла
            using FileStream targetStream = File.Create(compressedFile);

            // поток архивации
            using GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);

            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());



        }
    }
}

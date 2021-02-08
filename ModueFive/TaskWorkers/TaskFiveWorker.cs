using SkillBox.Infrastructure.Builder;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ModueFive.TaskWorkers
{
    internal class TaskFiveWorker : TaskWork
    {
        private volatile bool IsViewStatistic;

        private CancellationTokenSource cts;
        private CancellationToken ct;

        public override void DoTask()
        {
            SetConfigTaskAsync();
            Console.WriteLine();
            NextStep();
            Console.WriteLine();
        }

        protected override void SetConfigTask()
        {
            throw new NotImplementedException();
        }

        private void SetConfigTaskAsync()
        {
            Console.WriteLine();
            Console.WriteLine("function A(n, m)");

            var n = InputWholeNumber(0, "input n parametr:");
            var m = InputWholeNumber(0, "input m parametr:");

            Console.WriteLine("press (F) to stop view statistic if need");
            Console.WriteLine("view statistic?(y/n)");
            var key = Console.ReadLine().Trim().ToLower();
            IsViewStatistic = key.Equals("y");
            if (!key.Equals("n") && !key.Equals("y"))
                Console.WriteLine("auto answer - no");

            try
            {
                cts = new();
                ct = cts.Token;

                Console.WriteLine();
                var tasks = new Task[] { StartWaiterKeyTask(), StartDowWorkTask(n, m) };

                foreach (var t in tasks)
                    t.Start();

                try
                {
                    Task.WaitAll(tasks);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
                }
                finally
                {
                    cts.Dispose();
                }

            }
            catch (Exception e)
            {
                DisplayError($"exception - {e.Message}");
            }

        }

        /// <summary>
        /// поток ожидающий нажатия клавиши
        /// </summary>
        /// <returns></returns>
        private Task StartWaiterKeyTask()
        {
            return new Task(() =>
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    if (Console.ReadKey().KeyChar == 'f')
                    {
                        IsViewStatistic = false;
                        Console.WriteLine($"stopping view statistic");
                    }
                }
                catch (Exception e)
                {
                }

            }, ct);
        }

        /// <summary>
        /// поток выполняющий задачу
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private Task StartDowWorkTask(int n, int m)
        {
            int counter = 0;

            return new Task(() =>
            {
                var result = DunctionAccerman(n, m, ref counter, 1);
                cts.Cancel();
                Console.WriteLine($"result {result}");

                if (!IsViewStatistic)
                    Console.WriteLine($"all step: {counter}");
            });
        }

        /// <summary>
        /// реализация рекурсивной функции Аккермана
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private int DunctionAccerman(int n, int m, ref int step, int rStep)
        {
            try
            {
                if (n == 0)
                {
                    step++;
                    if (IsViewStatistic)
                        Console.WriteLine($"step(g): {step} step(r): {rStep} - 'm + 1' with n = {n} and m = {m}");

                    return m + 1;
                }

                if (m == 0 && n != 0)
                {
                    step++;
                    if (IsViewStatistic)
                        Console.WriteLine($"step(g): {step} step(r): {rStep} - 'A(n - 1, 1)' with n = {n} and m = {m}");

                    return DunctionAccerman(n - 1, 1, ref step, rStep + 1);
                }

                if (n > 0 && m > 0)
                {
                    step++;
                    if (IsViewStatistic)
                        Console.WriteLine($"step(g): {step} step(r): {rStep} - 'A(n - 1, A(n, m - 1))' with n = {n} and m = {m}");

                    return DunctionAccerman(n - 1, DunctionAccerman(n, m - 1, ref step, rStep + 1)
                            , ref step, rStep + 1);
                }

                return -1;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}

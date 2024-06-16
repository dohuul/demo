using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    public class ThreadUtilities
    {
        public static void StartParallelTasksFor(int numberOfThreads,
            Action<string> action,
            List<string> source,
            string pathToLogFile = null,
            int sleepBetweenWaves = 60000,
            bool shouldLogToConsole)
        {

            int NUMBER_OF_THREADS = numberOfThreads;
            int i = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while(i < source.Count())
            {
                if((i  + NUMBER_OF_THREADS) >= source.Count())
                {
                    NUMBER_OF_THREADS = 1;
                }

                Task[] tasks = new Task[NUMBER_OF_THREADS];

                for(int j = 0; j < NUMBER_OF_THREADS; j++)
                {
                    int currentItemIndex = i + j;
                    if(currentItemIndex < source.Count)
                    {
                        string item = source[currentItemIndex];
                        tasks[j] = Task.Factory.StartNew(() => action(item));
                    }
                }

                Stopwatch watchWait = new Stopwatch();
                watchWait.Start();
                Task.WaitAll(tasks);
                watchWait.Stop();


                int elapse = (int)watchWait.ElapsedMilliseconds;
                if(elapse < sleepBetweenWaves)
                {
                    int wait = sleepBetweenWaves - elapse;
                    System.Threading.Thread.Sleep(wait);
                }

                i = i + NUMBER_OF_THREADS;

                long miliseconds = watch.ElapsedMilliseconds;
                double seconds = miliseconds / 1000;
                double minute = seconds / 60;
                double recordPerMinute = i / minute;
                int totalRecords = source.Count();
                int recordsLeft = totalRecords - i;
                double estiamteMinutes = recordsLeft / recordPerMinute;

                string log_content = $"Minutes:{minute}|TotalRecordsProcessed:{i.ToString()}|" 
                    + $"RecordsPerMinute:{recordPerMinute}|TotalRecords:{totalRecords}"
                    + $"TotalRecordsLeft:{recordsLeft}|EstimatedMinutes:{estiamteMinutes}";

                if(!string.IsNullOrEmpty(pathToLogFile))
                {
                    LogUtilities.Log(log_content, pathToLogFile);
                }

                if (shouldLogToConsole)
                {
                    Console.WriteLine(log_content);
                }

            }
        }
    }
}

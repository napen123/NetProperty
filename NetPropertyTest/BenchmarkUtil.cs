using System;
using System.Diagnostics;

namespace NetPropertyTest
{
    public class BenchmarkUtil
    {
        public static double Benchmark(int iterations, Action action)
        {
            var time = 0.0;
            const int warmup = 5;

            GC.WaitForPendingFinalizers();

            for (var i = 0; i < warmup; i++)
                action();

            GC.Collect();

            var watch = Stopwatch.StartNew();

            for (var i = 0; i < iterations; i++)
            {
                action();
                time += (double) watch.ElapsedMilliseconds / iterations;
            }

            watch.Stop();

            return time;
        }
    }
}

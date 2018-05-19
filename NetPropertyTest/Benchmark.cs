using System;
using System.Diagnostics;

namespace NetPropertyTest
{
    public class Benchmark
    {
        public static double Measure(Action action, int iterations, int warmup = 1000)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            for (var i = 0; i < warmup; i++)
                action();
            
            var watch = Stopwatch.StartNew();

            for (var i = 0; i < iterations; i++)
                action();

            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }
    }
}

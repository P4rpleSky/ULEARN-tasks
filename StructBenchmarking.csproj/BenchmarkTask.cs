using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class BuilderTest : ITask
    { 
        public void Run()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i < 10000 + 1; i++)
                stringBuilder.Append('a');
        }
    }

    public class ConstructorTest : ITask
    {
        public void Run()
        {
            var stringConstructor = new string('a', 10000);
        }
    }

    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            task.Run();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var time = Stopwatch.StartNew();
            for (int i = 1; i < repetitionCount + 1; i++)
				task.Run();
            return time.Elapsed.TotalMilliseconds / repetitionCount;
        }
	}

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();
            var builderTest = new BuilderTest();
            var constructorTest = new ConstructorTest();
            var timeOfBuilder =
                benchmark.MeasureDurationInMs(builderTest, 10000);
            var timeOfConstructor =
                benchmark.MeasureDurationInMs(constructorTest, 10000);
            Assert.Less(timeOfConstructor, timeOfBuilder);
        }
    }
}
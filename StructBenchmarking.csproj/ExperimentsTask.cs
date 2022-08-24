using System.Collections.Generic;

namespace StructBenchmarking
{
    public interface IClassAndStructure
    {
        ITask ClassTask(int count);
        ITask StructureTask(int count);
    }

    public class ArrayCreation : IClassAndStructure
    {
        public ITask ClassTask(int count)
        {
            return new ClassArrayCreationTask(count);
        }

        public ITask StructureTask(int count)
        {
            return new StructArrayCreationTask(count);
        }
    }

    public class MethodCall : IClassAndStructure
    {
        public ITask ClassTask(int count)
        {
            return new MethodCallWithClassArgumentTask(count);
        }

        public ITask StructureTask(int count)
        {
            return new MethodCallWithStructArgumentTask(count);
        }
    }

    public class Experiments
    {
        public static ChartData BuildChartData(
            IClassAndStructure classAndStructure, string title, 
                                                IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();
            double averageTime = 0;
            foreach (var count in Constants.FieldCounts)
            {
                var classArray = classAndStructure.ClassTask(count);
                averageTime
                    = benchmark.MeasureDurationInMs(classArray, repetitionsCount);
                classesTimes.Add(new ExperimentResult(count, averageTime));
                var structArray = classAndStructure.StructureTask(count);
                averageTime
                    = benchmark.MeasureDurationInMs(structArray, repetitionsCount);
                structuresTimes.Add(new ExperimentResult(count, averageTime));
            }
            return new ChartData
            {
                Title = title,
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            return BuildChartData(new ArrayCreation(), 
                "Create array", benchmark, repetitionsCount);
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            return BuildChartData(new MethodCall(), 
                "Call method with argument", benchmark, repetitionsCount);
        }
    }
}
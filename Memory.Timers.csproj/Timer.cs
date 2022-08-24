using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        private bool disposedValue;
        private List<Timer> childsList;
        private int level;
        private Stopwatch workTime;
        private StringWriter stringWriter;
        private string timerName;
        private string reportLine;

        public static Timer Start(StringWriter stringWriter, string timerName = "*")
        {
            return new Timer(stringWriter, 0, timerName);
        }

        private Timer(StringWriter stringWriter, int level, string timerName)
        {
            childsList = new List<Timer>();
            this.level = level;
            workTime = Stopwatch.StartNew();
            this.stringWriter = stringWriter;
            this.timerName = timerName;
        }

        // Use this method in your solution to fit report formatting requirements from the tests
        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }

        public Timer StartChildTimer(string timerName)
        {
            var childTimer = new Timer(stringWriter, level + 1, timerName);
            childsList.Add(childTimer);
            return childTimer;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    workTime.Stop();
                    reportLine = FormatReportLine(timerName, level, workTime.ElapsedMilliseconds);
                    if (level == 0)
                        foreach (var repLine in MakeReportFrom(this))
                            stringWriter.Write(repLine);
                    stringWriter.Flush();
                }
                disposedValue = true;
            }
        }

        private IEnumerable<string> MakeReportFrom(Timer rootTimer)
        {
            yield return rootTimer.reportLine;
            if (rootTimer.childsList.Count == 0)
                yield break;
            long totalTime = 0;
            foreach (var child in rootTimer.childsList)
            {
                totalTime += child.workTime.ElapsedMilliseconds;
                foreach (var repLine in MakeReportFrom(child))
                    yield return repLine;
            }
            yield return FormatReportLine(
                "Rest",
                rootTimer.level + 1,
                rootTimer.workTime.ElapsedMilliseconds - totalTime);
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Timer()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
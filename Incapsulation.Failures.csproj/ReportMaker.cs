using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public enum FailureType
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures, 
        ConnectionProblems
    }

    public class Failure
    {
        public FailureType FailureType { get; set; }
        public DateTime Date { get; set; }

        public Failure(FailureType failureType, int year, int month, int day)
        {
            this.FailureType = failureType;
            this.Date = new DateTime(year, month, day);
        }

        public bool IsEarlier(DateTime dateTime)
        {
            return this.Date < dateTime;
        }

        public bool IsFailureSerious()
        {
            return (int)FailureType % 2 == 0;
        }
    }
    
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Failure Failure { get; set; }

        public Device(int id, string name, Failure failure)
        {
            Id = id;
            Name = name;
            Failure = failure;
        }
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDate(Device[] devices, DateTime dateTime)
        { 
            return devices.Where(x => x.Failure.IsEarlier(dateTime) && x.Failure.IsFailureSerious())
                .Select(x => x.Name)
                .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var devicesArr = new Device[devices.Count];
            for (int i = 0; i < devicesArr.Length; ++i)
                devicesArr[i] = new Device(
                    (int)devices[i]["DeviceId"],
                    (string)devices[i]["Name"],
                    new Failure(
                        (FailureType)failureTypes[i],
                        (int)times[i][2],
                        (int)times[i][1],
                        (int)times[i][0]));
            return FindDevicesFailedBeforeDate(devicesArr, new DateTime(year, month, day));
        }
    }
}
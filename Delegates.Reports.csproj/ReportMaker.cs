using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    public interface IReportStatistics
    {
	object MakeStatistics(IEnumerable<double> data);
	string Caption { get; }
    }

    public class MeanAndStdStatistics : IReportStatistics
    {
        public string Caption => "Mean and Std";

        public object MakeStatistics(IEnumerable<double> _data)
        {
			var data = _data.ToList();
			var mean = data.Average();
			var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));
			return new MeanAndStd
			{
				Mean = mean,
				Std = std
			};
		}
    }

	public class MedianStatistics : IReportStatistics
	{
		public string Caption => "Median";

		public object MakeStatistics(IEnumerable<double> data)
		{
			var list = data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
				return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
			return list[list.Count / 2];
		}
	}

	public static class ReportMakerHelper
	{
		public static string MakeReport(
		IEnumerable<Measurement> measurements,
		IReportStatistics reportStatistics,
		Func<string, string> makeCaption,
		Func<string, string, string> makeItem,
		Func<string> beginList,
		Func<string> endList)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(makeCaption(reportStatistics.Caption));
			result.Append(beginList());
			result.Append(makeItem("Temperature", reportStatistics.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(makeItem("Humidity", reportStatistics.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(endList());
			return result.ToString();
		}

		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return MakeReport(
				data,
				new MeanAndStdStatistics(),
				(caption) => $"<h1>{caption}</h1>",
				(valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
				() => "<ul>",
				() => "</ul>"    
				);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return MakeReport(
				data,
				new MedianStatistics(),
				(caption) => $"## {caption}\n\n",
				(valueType, entry) => $" * **{valueType}**: {entry}\n\n",
				() => "",
				() => ""
				);
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return MakeReport(
				measurements,
				new MeanAndStdStatistics(),
				(caption) => $"## {caption}\n\n",
				(valueType, entry) => $" * **{valueType}**: {entry}\n\n",
				() => "",
				() => ""
				);
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			return MakeReport(
				measurements,
				new MedianStatistics(),
				(caption) => $"<h1>{caption}</h1>",
				(valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
				() => "<ul>",
				() => "</ul>"
				);
		}
	}
}

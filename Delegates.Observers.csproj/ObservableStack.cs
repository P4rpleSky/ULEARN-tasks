using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		public StringBuilder Log = new StringBuilder();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Notify += (eventData) => Log.Append(eventData);
		}

		public string GetLog()
		{
			return Log.ToString();
		}
	}

	public class ObservableStack<T>
	{		
		List<T> data = new List<T>();
		public event Action<StackEventData<T>> Notify;

		public void Push(T obj)
		{
			data.Add(obj);
			if (Notify != null)
				Notify.Invoke(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			if (Notify != null)
				Notify.Invoke(new StackEventData<T> { IsPushed = false, Value = result });
			return result;
		}
	}
}
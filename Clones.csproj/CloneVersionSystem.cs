using System;
using System.Collections.Generic;

namespace Clones
{
	public class StackItem<T>
	{
		public T Value { get; set; }
		public StackItem<T> Prev { get; set; }
	}

	public class NodeStack<T>
	{
		public StackItem<T> Tail;

		public NodeStack() { }
		public NodeStack(NodeStack<T> stack)
        {
			Tail = stack.Tail;
        }

		public void Push(T value)
		{
			var item = new StackItem<T> { Value = value, Prev = Tail };
			Tail = item;
		}

		public T Pop()
		{
			if (Tail == null) throw new InvalidOperationException();
			T result = Tail.Value;
			Tail = Tail.Prev;
			return result;
		}
	}

	public class Clone
    {
		private readonly NodeStack<string> programList;
		private readonly NodeStack<string> listOfCancel;

		public Clone()
        {
			programList = new NodeStack<string>();
			listOfCancel = new NodeStack<string>();
		}

		public Clone(Clone clone)
        {
			programList = new NodeStack<string>(clone.programList);
			listOfCancel = new NodeStack<string>(clone.listOfCancel);
        }

		public void Learn(string program)
        {
			programList.Push(program);
			listOfCancel.Tail = null;
        }

		public void Rollback()
        {
			if (programList.Tail == null) return;
			listOfCancel.Push(programList.Pop());
        }

		public void Relearn()
        {
			if (listOfCancel.Tail == null) return;
			programList.Push(listOfCancel.Pop());
		}

		public string Check()
        {
			if (programList.Tail is null)
				return "basic";
			else
				return programList.Tail.Value;
        }
	}

	public class CloneVersionSystem : ICloneVersionSystem
	{
		public List<Clone> Clones;

		public CloneVersionSystem()
        {
            Clones = new List<Clone> { new Clone() };
        }

		public string Execute(string query)
		{
			var subs = query.Split(' ');
			string command = subs[0];
			int number = int.Parse(subs[1]) - 1;
			switch (command)
            {
				case "learn":
					string program = subs[2];
					Clones[number].Learn(program);
					break;
				case "rollback":
					Clones[number].Rollback();
					break;
				case "relearn":
					Clones[number].Relearn();
					break;
				case "clone":
					Clones.Add(new Clone(Clones[number]));
					break;
				case "check":
					return Clones[number].Check();
			}
			return null;
		}
	}
}
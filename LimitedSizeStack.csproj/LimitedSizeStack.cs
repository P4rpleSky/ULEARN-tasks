using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        LinkedList<T> list = new LinkedList<T>();
        int limit = 0;
        public LimitedSizeStack(int limit)
        {
            list = new LinkedList<T>();
            this.limit = limit;
        }

        public void Push(T item)
        {
            list.AddLast(item);
            if (list.Count > limit) list.RemoveFirst();
        }

        public T Pop()
        {
            T element = list.Last.Value;
            list.RemoveLast();
            return element;
        }

        public int Count
        {
            get { return list.Count; }
        }
    }
}

using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class OperationInfo<T>
    {
        public int Index;
        public string NameOfButton;
        public T Task;

        public OperationInfo(int index, string nameOfButton, T task)
        {
            Index = index;
            NameOfButton = nameOfButton;
            Task = task;
        }
    }
    
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        public LimitedSizeStack<OperationInfo<TItem>> StackOfOperations;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            StackOfOperations = new LimitedSizeStack<OperationInfo<TItem>>(Limit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            StackOfOperations.Push(
                new OperationInfo<TItem>(Items.Count - 1, "Добавить", item));
        }

        public void RemoveItem(int index)
        {
            TItem item = Items[index];
            Items.RemoveAt(index);
            StackOfOperations.Push(
                new OperationInfo<TItem>(index, "Удалить", item));
        }

        public bool CanUndo()
        {
            return StackOfOperations.Count != 0;
        }

        public void Undo()
        {
            if (!CanUndo()) return;
            OperationInfo<TItem> lastOperation = StackOfOperations.Pop();
            switch (lastOperation.NameOfButton)
            {
                case "Удалить":
                    Items.Insert(lastOperation.Index, lastOperation.Task);
                    break;
                case "Добавить":
                    Items.RemoveAt(lastOperation.Index);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
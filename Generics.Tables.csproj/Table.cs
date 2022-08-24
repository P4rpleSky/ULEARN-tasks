using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class ExistedCell<T1, T2, T3>
    {
        Table<T1, T2, T3> table;

        public ExistedCell(Table<T1, T2, T3> table) => this.table = table;

        public T3 this[T1 row, T2 column]
        {
            get
            {
                var cell = Tuple.Create(row, column);
                if (!table.Rows.Contains(row) || !table.Columns.Contains(column))
                    throw new ArgumentException();
                if (!table.Cells.ContainsKey(cell))
                    table.Cells[cell] = default(T3);
                return table.Cells[cell];
            }
            set
            {
                if (!table.Rows.Contains(row) || !table.Columns.Contains(column))
                    throw new ArgumentException();
                table.Cells[Tuple.Create(row, column)] = value;
            }
        }
    }

    public class OpenCell<T1, T2, T3>
    {
        Table<T1, T2, T3> table;

        public OpenCell(Table<T1, T2, T3> table) => this.table = table;

        public T3 this[T1 row, T2 column]
        {
            get
            {
                var cell = Tuple.Create(row, column);
                if (!table.Rows.Contains(row) || !table.Columns.Contains(column))
                    return default(T3);
                if (!table.Cells.ContainsKey(cell))
                    table.Cells[cell] = default(T3);
                return table.Cells[cell];
            }
            set
            {
                if (!table.Rows.Contains(row))
                    table.Rows.Add(row);
                if (!table.Columns.Contains(column))
                    table.Columns.Add(column);
                table.Cells[Tuple.Create(row, column)] = value;
            }
        }
    }

    public class Table<T1, T2, T3>
    {
        public HashSet<T1> Rows { get; private set; }
        public HashSet<T2> Columns { get; internal set; }
        public Dictionary<Tuple<T1, T2>, T3> Cells { get; private set; }
        public ExistedCell<T1, T2, T3> Existed { get; private set; }
        public OpenCell<T1, T2, T3> Open { get; private set; }

        public Table()
        {
            this.Rows = new HashSet<T1>();
            this.Columns = new HashSet<T2>();
            this.Cells = new Dictionary<Tuple<T1, T2>, T3>();
            this.Existed = new ExistedCell<T1, T2, T3>(this);
            this.Open = new OpenCell<T1, T2, T3>(this);
        }

        public void AddRow(T1 row)
        {
            if (!this.Rows.Contains(row))
                this.Rows.Add(row);
        }

        public void AddColumn(T2 column)
        {
            if (!this.Columns.Contains(column))
                this.Columns.Add(column);
        }
    }
}
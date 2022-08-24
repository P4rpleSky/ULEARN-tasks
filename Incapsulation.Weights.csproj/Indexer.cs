using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private readonly double[] weights;
        private readonly int start;
        private readonly int length;
        public int Length { get => length; }

        public Indexer(double[] weights, int start, int length)
        {
            this.weights = weights;
            if (length == 0) 
            {
                this.start = start;
                this.length = length;
            }
            else if (!IsIndexInRangeOfArray(start) || !IsLengthInRangeOfArray(length) || start + length > weights.Length)
                throw new ArgumentException();
            this.start = start;
            this.length = length;
        }

        private bool IsIndexInRangeOfArray(int index) => index >= 0 && index < weights.Length;
        private bool IsLengthInRangeOfArray(int length) => length >= 0 && length <= weights.Length;

        public double this[int index]
        {
            get
            {
                if (!IsIndexInRangeOfArray(index) || index >= length)
                    throw new IndexOutOfRangeException();
                return this.weights[start + index];
            }
            set
            {
                if (!IsIndexInRangeOfArray(index) || index >= length)
                    throw new IndexOutOfRangeException();
                this.weights[start + index] = value;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.API
{
    public class APIObject : IDisposable
    {
        private bool disposedValue;
        private Stack<int> allocatedValues = new Stack<int>();

        public APIObject(int id)
        {
            MagicAPI.Allocate(id);
            allocatedValues.Push(id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                while (allocatedValues.Count > 0)
                    MagicAPI.Free(allocatedValues.Pop());
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~APIObject()
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
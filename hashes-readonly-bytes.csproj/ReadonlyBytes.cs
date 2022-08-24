using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytesEnumerator : IEnumerator<byte>
    {
        readonly ReadonlyBytes readonlyBytes;
        int index = -1;

        public ReadonlyBytesEnumerator(ReadonlyBytes readonlyBytes)
        {
            this.readonlyBytes = readonlyBytes;
        }

        public byte Current
        {
            get
            {
                return readonlyBytes[index];
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            index++;
            return index < readonlyBytes.Length;
        }

        public void Dispose()
        {

        }

        public void Reset()
        {

        }
    }

    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] bytes;
        private int hashCode = FNVConstants.OffsetBasis;
        public int Length
        {
            get 
            { 
                return bytes.Length;
            }
        }

        public ReadonlyBytes(params byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException();
            this.bytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                this.bytes[i] = bytes[i];
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return new ReadonlyBytesEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Length) throw new IndexOutOfRangeException();
                return bytes[index];
            }
        }

        public static class FNVConstants
        {
            public static readonly int OffsetBasis = unchecked((int)2166136261);
            public static readonly int Prime = 16777619;
        }

        public override int GetHashCode()
        {
            if (hashCode != FNVConstants.OffsetBasis)
                return hashCode;
            int primeFNV = FNVConstants.Prime;
            unchecked
            {
                foreach (var e in this)
                {
                    hashCode ^= e;
                    hashCode *= primeFNV;
                }
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType().Name != "ReadonlyBytes") return false;
            var byteArray = obj as ReadonlyBytes;
            return this.GetHashCode() == byteArray.GetHashCode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder("[");
            foreach (var e in this)
                builder.Append(e.ToString() + ", ");
            if (builder[builder.Length - 1] == ' ')
                builder.Remove(builder.Length - 2, 2);
            builder.Append(']');
            return builder.ToString();
        }
    }
}
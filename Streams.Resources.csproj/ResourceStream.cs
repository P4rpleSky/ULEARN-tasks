using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Resources
{
    public class ResourceReaderStream : Stream
    {
        private readonly byte[] requiredKey;
        private readonly BufferedStream underlayingStream;
        private int pointer;
        private bool keyWasFound;
        private byte[] receivedData;
        private int receivedBytes;
        private bool keyWasRead;
        private bool valueWasSkipped;
        private bool valueWasRead;

        private byte[] InsertZerosInKey(byte[] key)
        {
            var result = new List<byte>();
            for (int i = 0; i < key.Length; i++)
            {
                //if (SeparatorWasReached(i, key))
                //    result.Add(0);
                result.Add(key[i]);
            }
            return result.ToArray();
        }
        
        public ResourceReaderStream(Stream stream, string key)
        {
            // You should not use stream in the constructor of wrapping stream.
            this.requiredKey = InsertZerosInKey(Encoding.ASCII.GetBytes(key));
            this.underlayingStream = new BufferedStream(stream, Constants.BufferSize);
            this.keyWasFound = false;
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            // if key not found yet: SeekValue();
            // if value is not read yet: ReadFieldValue(...)
            // else return 0;
            if (valueWasRead)
                return 0;
            receivedBytes = 0;
            receivedData = new byte[count];
            if (!keyWasFound)
                SeekValue();
            return ReadFieldValue(buffer);
        }

        private void ReadFromStreamAndCheckForEnd()
        {
            receivedBytes = underlayingStream.Read(receivedData, 0, receivedData.Length);
            if (receivedBytes == 0)
                throw new EndOfStreamException();
            pointer = 0;
        }

        private void SeekValue()
        {
            // while not end of stream read next section key,
            // compare with required key and skip value if read key is not equal to required key
            var potentialKey = new List<byte>();
            valueWasSkipped = true;
            while (!keyWasFound)
            {
                ReadFromStreamAndCheckForEnd();
                while (pointer < receivedData.Length)
                {
                    if (valueWasSkipped)
                        ReadNextPartOfKey(potentialKey);
                    else
                    {
                        SkipNextPartOfValue();
                        continue;
                    }
                    if (keyWasRead && potentialKey.SequenceEqual(requiredKey))
                    {
                        keyWasFound = true;
                        return;
                    }
                    else if (keyWasRead)
                    {
                        keyWasRead = false;
                        potentialKey.Clear();
                        valueWasSkipped = false;
                    }
                }   
            }
        }

        private void SkipNextPartOfValue()
        {
            for (; pointer < receivedData.Length; pointer++)
            {
                if (SeparatorWasReached(pointer, receivedData))
                {
                    pointer += 2;
                    pointer = pointer < receivedData.Length ?
                        pointer : receivedData.Length - pointer;
                    valueWasSkipped = true;
                    return;
                }
            }
        }

        private void ReadNextPartOfKey(List<byte> potentialKey)
        {
            for (; pointer < receivedData.Length; pointer++)
            {
                if (!SeparatorWasReached(pointer, receivedData))
                {
                    if (!(potentialKey.Count > 0 && potentialKey[potentialKey.Count - 1] == 0 &&
                        receivedData[pointer] == 0))
                        potentialKey.Add(receivedData[pointer]);
                }
                else
                {
                    pointer += 2;
                    pointer = pointer < receivedData.Length ?
                        pointer : receivedData.Length - pointer;
                    keyWasRead = true;
                    return;
                }
            }
        }

        private bool SeparatorWasReached(int index, byte[] array)
        {
            if (index == 0)
                return false;
            if (index == 1 && array[0] == 0 && array[1] == 1)
                return true;
            return array[index - 1] != 0 && array[index] == 0 &&
                index < array.Length && array[index + 1] == 1;
        }

        private int ReadFieldValue(byte[] buffer)
        {
            if (pointer >= receivedData.Length)
                ReadFromStreamAndCheckForEnd();
            int i = 0;
            while (true)
            {
                if (pointer == receivedData.Length)
                    break;
                if (SeparatorWasReached(pointer, receivedData))
                {
                    valueWasRead = true;
                    break;
                }
                if (!(i > 0 && buffer[i - 1] == 0 && receivedData[pointer] == 0))
                {
                    buffer[i] = receivedData[pointer];
                    i++;
                }
                pointer++;
            }
            return i;
        }

        public override void Flush()
        {
            // nothing to do
        }
        
        public override long Length => throw new NotSupportedException();

        public override long Position 
        { 
            get => throw new NotSupportedException(); 
            set => throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;
    }
}

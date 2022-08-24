using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		private static void Inc(IVirtualMachine b)
        {
			if (b.Memory[b.MemoryPointer] == 255)
			{
				b.Memory[b.MemoryPointer] = 0;
				return;
			}
			b.Memory[b.MemoryPointer]++;
		}

		private static void Dec(IVirtualMachine b)
		{
			if (b.Memory[b.MemoryPointer] == 0)
			{
				b.Memory[b.MemoryPointer] = 255;
				return;
			}
			b.Memory[b.MemoryPointer]--;
		}

		private static void ShiftUp(IVirtualMachine b)
		{
			if (b.MemoryPointer == b.Memory.Length - 1)
			{
				b.MemoryPointer = 0;
				return;
			}
			b.MemoryPointer++;
		}

		private static void ShiftDown(IVirtualMachine b)
		{
			if (b.MemoryPointer == 0)
			{
				b.MemoryPointer = b.Memory.Length - 1;
				return;
			}
			b.MemoryPointer--;
		}

		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
			vm.RegisterCommand('+', b => Inc(b));
			vm.RegisterCommand('-', b => Dec(b));
			vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
			vm.RegisterCommand('>', b => ShiftUp(b));
			vm.RegisterCommand('<', b => ShiftDown(b));
			char symbol = Char.MinValue;
			while (symbol < char.MaxValue)
            		{
				char temp = symbol;
				if (Char.IsLetterOrDigit(temp))
					vm.RegisterCommand(temp, b => b.Memory[b.MemoryPointer] = (byte)temp);
				symbol++;
			}
        	}
	}
}
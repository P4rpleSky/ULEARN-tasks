using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
        private static Dictionary<int, int> bracketsDictionary = new Dictionary<int, int>();
        private static Dictionary<int, int> inversedDictionary = new Dictionary<int, int>();

        public static void BracketsIndexes(IVirtualMachine b)
        {
            Stack<int> bracketsStack = new Stack<int>();
            for (int i = 0; i < b.Instructions.Length; i++)
            {
                if (b.Instructions[i] == '[')
                    bracketsStack.Push(i);
                if (b.Instructions[i] == ']')
                    bracketsDictionary[bracketsStack.Pop()] = i;
            }
        }

        public static void InverseBracketsIndexes()
        {
            foreach (var key in bracketsDictionary.Keys)
                inversedDictionary[bracketsDictionary[key]] = key;
        }

        public static void OpenBracket(IVirtualMachine b)
        {
            if (b.Memory[b.MemoryPointer] == 0)
                b.InstructionPointer = bracketsDictionary[b.InstructionPointer];
        }

        public static void CloseBracket(IVirtualMachine b)
        {
            if (b.Memory[b.MemoryPointer] != 0)
                b.InstructionPointer = inversedDictionary[b.InstructionPointer];
        }

        public static void RegisterTo(IVirtualMachine vm)
		{
            BracketsIndexes(vm);
            InverseBracketsIndexes();
            vm.RegisterCommand('[', b => OpenBracket(b));
			vm.RegisterCommand(']', b => CloseBracket(b));
		}
	}
}
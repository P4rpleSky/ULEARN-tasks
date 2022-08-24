using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("\'a\"", 0, "a\"", 3)]
        [TestCase("\'", 0, "", 1)]
        [TestCase("''", 0, "", 2)]
        [TestCase("\"", 0, "", 1)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("\"abc\"", 0, "abc", 5)]
        [TestCase("b \"a'\"", 2, "a'", 4)]
        [TestCase(@"'a\' b'", 0, "a' b", 7)]
        [TestCase(@"some_text ""QF \"""" other text", 10, "QF \"", 7)]
        [TestCase("a \"c\"", 0, "a \"c\"", 5)]
        [TestCase("\'a\'b", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static bool IsCurrentSymbolShielded(string line, int index)
        {
            if (line.Length == 0)
                return false;
            var shieldedSymbols = new char[] { '\\', '\"', '\'' };
            return shieldedSymbols.Contains(line[index]);
        }

        public static void CheckForUseless(StringBuilder builder)
        {
            if (builder.Length >= 2)
            {
                bool isUselessSingle = builder[0] == '\'' && builder[builder.Length - 1] == '\'';
                bool isUselessDouble = builder[0] == '\"' && builder[builder.Length - 1] == '\"';
                if (isUselessSingle || isUselessDouble)
                {
                    builder.Remove(0, 1);
                    builder.Remove(builder.Length - 1, 1);
                }
            }
        }

        public static bool IsEndOfInput(StringBuilder builder)
        {
            if (builder.Length >= 2)
            {
                int length = builder.Length;
                var Symbol1 = builder[length - 1];
                var Symbol2 = builder[0];
                bool firstCond = Symbol1 == '\"' && Symbol2 == '\"';
                bool secondCond = Symbol1 == '\'' && Symbol2 == '\'';
                return firstCond || secondCond;
            }
            return false;
        }

        public static void OneSymbolCheck(StringBuilder builder)
        {
            if (builder.Length == 1 && new char[] { '\'', '\"' }.Contains(builder[0]))
                builder.Clear();
        }

        public static void DifferentEndsCheck(StringBuilder builder)
        {
            if (builder.Length >= 2)
            {
                int length = builder.Length;
                var Symbol1 = builder[length - 1];
                var Symbol2 = builder[0];
                bool areNotEqual = Symbol1 != Symbol2;
                var allowedSymbols = new char[] { '\"', '\'' };
                if (allowedSymbols.Contains(Symbol1) && allowedSymbols.Contains(Symbol2) && areNotEqual)
                    builder.Remove(0, 1);
                else if (allowedSymbols.Contains(Symbol2) && !allowedSymbols.Contains(Symbol1))
                    builder.Remove(0, 1);
            }
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            var builder = new StringBuilder();
            bool isPreviousSlash = false;
            int tokenLength = 0;
            for (int i = startIndex; i < line.Length; i++)
            {
                tokenLength++;
                if (char.IsLetter(line[i]))
                    builder.Append(line[i]);
                else if (line[i] == '\\' && !isPreviousSlash)
                {
                    isPreviousSlash = true;
                    builder.Append(line[i]);
                }
                else if ((IsCurrentSymbolShielded(line, i) && isPreviousSlash))
                {
                    builder[builder.Length - 1] = line[i];
                    isPreviousSlash = false;
                    continue;
                }
                else
                    builder.Append(line[i]);
                if (builder.Length >=2 && IsEndOfInput(builder))
                    break;
            }
            OneSymbolCheck(builder);
            CheckForUseless(builder);
            DifferentEndsCheck(builder);
            return new Token(builder.ToString(), startIndex, tokenLength);
        }
    }
}
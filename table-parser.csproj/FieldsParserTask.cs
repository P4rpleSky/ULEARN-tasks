using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("\'a\"", new[] { "a\"" })]
        [TestCase("\'", new[] { "" })]
        [TestCase("''", new[] { "" })]
        [TestCase("\"", new[] { "" })]
        [TestCase("hello    world", new[] { "hello", "world" })]
        [TestCase("b \"a'\"", new[] { "b", "a'" })]
        [TestCase("\"a'\"b", new[] { "a'", "b" })]
        [TestCase(@"'a\' b'", new[] { "a' b" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase("\'a ", new[] { "a " })]
        [TestCase("a    ", new[] { "a" })]
        [TestCase("", new string[0])]
        [TestCase(@"""QF \""""", new[] { "QF \"" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            List<Token> result = new List<Token>();
            int i = 0;
			while (i < line.Length && line[i] == ' ')
                i++;
            while (i < line.Length)
            {
                if (line[i] != '\"' && line[i] != '\'')
                    result.Add(ReadField(line, i));
                else
                    result.Add(ReadQuotedField(line, i));
                i = IndexAfterTokenAndSpaces(line, result[result.Count - 1]);
            }
            return result;
        }

        public static int IndexAfterTokenAndSpaces(string line, Token token)
        {
            int index = token.GetIndexNextToToken();
            while (index < line.Length && line[index] == ' ')
                index++;
            return index;
        }

        public static Token ReadField(string line, int startIndex)
        {
            var builder = new StringBuilder();
            var illegalSymbols = new char[] { ' ', '\"', '\'' };
            int index = startIndex;
            while (index < line.Length && !illegalSymbols.Contains(line[index]))
            {
                builder.Append(line[index]);
                index++;
            }
            return new Token(builder.ToString(), startIndex, builder.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}
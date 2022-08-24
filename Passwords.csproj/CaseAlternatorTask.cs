using System;
using System.Collections.Generic;
using System.Linq;

namespace Passwords
{
    public class CaseAlternatorTask
    {
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
            {
                var temp = new char[word.Length];
                for (int j = 0; j < word.Length; j++)
                    temp[j] = word[j];
                if (result.IndexOf(new string(temp)) == -1)
                    result.Add(new string(temp));
            }
            else
            {
                AlternateCharCases(word, startIndex + 1, result);
                if (char.IsLetter(word[startIndex]))
                {
                    word[startIndex] = char.ToUpper(word[startIndex]);
                    AlternateCharCases(word, startIndex + 1, result);
                    word[startIndex] = char.ToLower(word[startIndex]);
                }
            }
        }
    }
}
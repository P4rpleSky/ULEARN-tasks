using System;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            List<string> result = new List<string>();
            string[] words = phraseBeginning.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
                result.Add(word);
            if (result.Count == 0)
                return phraseBeginning;
            while (wordsCount > 0)
            {
                if (result.Count == 1)
                {
                    if (!nextWords.ContainsKey(result[0]))
                        return phraseBeginning;
                    else
                        result.Add(nextWords[result[0]]);
                    wordsCount--;
                    continue;
                }
                if (result.Count >= 2)
                {
                    int lastWordIndex = result.Count - 1;
                    string singleKey = result[lastWordIndex];
                    string doubleKey = result[lastWordIndex - 1] + ' ' + singleKey;
                    if (nextWords.ContainsKey(doubleKey))
                        result.Add(nextWords[doubleKey]);
                    else if (nextWords.ContainsKey(singleKey))
                        result.Add(nextWords[singleKey]);
                    else
                        return String.Join(" ", result.ToArray());
                    wordsCount--;
                    continue;
                } 
            }
            return String.Join(" ", result.ToArray());
        }
    }
}
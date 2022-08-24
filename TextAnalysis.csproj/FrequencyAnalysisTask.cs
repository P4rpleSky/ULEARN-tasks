using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, Dictionary<string, int>> BigramDictionary (List<List<string>> text)
        {
            var dictionary = new Dictionary<string, Dictionary<string, int>>();
            foreach (var sentence in text)
                if (sentence.Count >= 2)
                    for (int i = 0; i < sentence.Count - 1; i++)
                    {
                        var bigramBegin = sentence[i];
                        var bigramEnd = sentence[i + 1];
                        if (!dictionary.ContainsKey(bigramBegin))
                            dictionary[bigramBegin] = new Dictionary<string, int>();
                        if (!dictionary[bigramBegin].ContainsKey(bigramEnd))
                            dictionary[bigramBegin][bigramEnd] = 0;
                        dictionary[bigramBegin][bigramEnd]++;
                    }
            return dictionary;
        }

        public static Dictionary<string, Dictionary<string, int>> TrigramDictionary(List<List<string>> text)
        {
            var dictionary = new Dictionary<string, Dictionary<string, int>>();
            foreach (var sentence in text)
                if (sentence.Count >= 3)
                    for (int i = 0; i < sentence.Count - 2; i++)
                    {
                        var bigramBegin = sentence[i] + ' ' + sentence[i + 1];
                        var bigramEnd = sentence[i + 2];
                        if (!dictionary.ContainsKey(bigramBegin))
                            dictionary[bigramBegin] = new Dictionary<string, int>();
                        if (!dictionary[bigramBegin].ContainsKey(bigramEnd))
                            dictionary[bigramBegin][bigramEnd] = 0;
                        dictionary[bigramBegin][bigramEnd]++;
                    }
            return dictionary;
        }

        public static Dictionary<string, string> SortedByFrequencyDictionary(Dictionary<string, Dictionary<string, int>> dictionary)
        {
            var sortedDictionary = new Dictionary<string, string>();
            foreach (var subDictionary in dictionary)
            {
                string key = "zzz";
                int max = 0;
                foreach (var e in subDictionary.Value)
                {
                    if (max < e.Value)
                    {
                        key = e.Key;
                        max = e.Value;
                    }
					if (max == e.Value && string.CompareOrdinal(e.Key, key) < 0)
					{
                        key = e.Key;
                        max = e.Value;
                    }
                }
                sortedDictionary[subDictionary.Key] = key;
            }
            return sortedDictionary;
        }


        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, int>> Bigrams = new Dictionary<string, Dictionary<string, int>>();
            Bigrams = BigramDictionary(text);
            Dictionary<string, Dictionary<string, int>> Trigrams = new Dictionary<string, Dictionary<string, int>>();
            Trigrams = TrigramDictionary(text);
            SortedByFrequencyDictionary(Bigrams).ToList().ForEach(x => result.Add(x.Key, x.Value));
            SortedByFrequencyDictionary(Trigrams).ToList().ForEach(x => result.Add(x.Key, x.Value));
            return result;
        }
   }
}
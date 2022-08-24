using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private readonly char[] charsForSplit = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
        private readonly Dictionary<int, Dictionary<string, List<int>>> wordsAndPosByIndex 
            = new Dictionary<int, Dictionary<string, List<int>>>();
        private readonly Dictionary<string, HashSet<int>> indexesByWord
            = new Dictionary<string, HashSet<int>>();        
 
        public void Add(int id, string documentText)
        {
            string[] words = documentText.Split(charsForSplit);
            wordsAndPosByIndex.Add(id, new Dictionary<string, List<int>>());
            int currentPos = 0;
            foreach (string word in words)
            {
                if (!indexesByWord.ContainsKey(word))
                    indexesByWord[word] = new HashSet<int>();
                if (!indexesByWord[word].Contains(id))
                    indexesByWord[word].Add(id);
                if (!wordsAndPosByIndex[id].ContainsKey(word))
                    wordsAndPosByIndex[id].Add(word, new List<int>());
                wordsAndPosByIndex[id][word].Add(currentPos);
                currentPos += word.Length + 1;
            }
        }
 
        public List<int> GetIds(string word)
        {
            return indexesByWord.ContainsKey(word) ? indexesByWord[word].ToList() : new List<int>();
        }
 
        public List<int> GetPositions(int id, string word)
        {
            List<int> positions = new List<int>();
            if (wordsAndPosByIndex.ContainsKey(id) && wordsAndPosByIndex[id].ContainsKey(word))
                positions = wordsAndPosByIndex[id][word];
            return positions;            
        }
 
        public void Remove(int id)
        {
            string[] words = wordsAndPosByIndex[id].Keys.ToArray();
            foreach (var word in words)            
				indexesByWord[word].Remove(id);
            wordsAndPosByIndex.Remove(id);
        }
    }
}
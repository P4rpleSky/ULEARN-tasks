using System;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<string> WordsParse(string sentence)
        {
            var wordsList = new List<string>();
            string word = "";
            for (int i = 0; i < sentence.Length; i++)
            {
                //char symbol = sentence[i];
                bool allowed = Char.IsLetter(sentence[i]) || sentence[i] == '\'';
                if (allowed)
                    word += Char.ToLower(sentence[i]);
                if (!allowed && word.Length != 0)
                {
                    wordsList.Add(word);
                    word = "";
                }
                if (i == sentence.Length - 1 && word.Length != 0)
                {
                    wordsList.Add(word);
                    word = "";
                }
            }
            return wordsList;
        }
        
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var symbols = new char[] { '.', '!', '?', ';', ':', '(', ')' };
            string[] sentences = text.Split(symbols, StringSplitOptions.RemoveEmptyEntries);
            if (sentences.Length == 0)
            {
                var wordsList = new List<string>();
                return sentencesList;
            }
            foreach (var sentence in sentences)
                if (WordsParse(sentence).Count != 0)
                    sentencesList.Add(WordsParse(sentence));
            return sentencesList;
        }
    }
}
using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        private static double LevenshteinDistance(DocumentTokens first, DocumentTokens second)
        {
            var previous = new double[second.Count + 1];
            var current = new double[second.Count + 1];
            for (var j = 0; j <= second.Count; ++j)
                current[j] = j;
            for (var i = 1; i <= first.Count; ++i)
            {
                current.CopyTo(previous, 0);
                current[0] = i;
                for (var j = 1; j <= second.Count; ++j)
                {
                    if (first[i - 1] == second[j - 1])
                        current[j] = previous[j - 1];
                    else
                        current[j] = Math.Min(
                            TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]) + previous[j - 1],
                            Math.Min(1 + previous[j], 1 + current[j - 1]));
                }
            }
            return current[second.Count];
        }

        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var result = new List<ComparisonResult>();
            for (var i = 0; i < documents.Count; ++i)
            {
                for (var j = i + 1; j < documents.Count; ++j)
                {
                    result.Add(
                        new ComparisonResult(
                            documents[i],
                            documents[j],
                            LevenshteinDistance(documents[i], documents[j])));
                }
            }
            return result;
        }
    }
}

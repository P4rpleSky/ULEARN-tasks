using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var outputList = new List<string>();
            int left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            int amount = 0;
            while (left < phrases.Count && amount < count
                && phrases[left].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                outputList.Add(phrases[left]);
                left++;
                amount++;
            }
            return outputList.ToArray();
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            int left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            int right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            return right - left - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            var emptyList = new List<string>();
            var actualResult = AutocompleteTask.GetTopByPrefix(emptyList, "popa", 4);
            CollectionAssert.IsEmpty(actualResult);
        }

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            var outputList = new List<string>();
            outputList.Add("aaaaa");
            outputList.Add("sdsrew");
            outputList.Add("edwws");
            var actualResult = AutocompleteTask.GetCountByPrefix(outputList, "");
            Assert.AreEqual(outputList.Count, actualResult);
        }

        [Test]
        public void TestNumbaOne()
        {
            var outputList = new List<string>();
            outputList.Add("aa");
            outputList.Add("ab");
            outputList.Add("ac");
            var actualResult = AutocompleteTask.GetTopByPrefix(outputList, "a", 2);
            Assert.AreEqual(2, actualResult.Length);
        }
    }
}
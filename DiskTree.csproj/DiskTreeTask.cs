using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskTree
{
    public class DiskTreeTask
    {
        private class Directory
        {
            public Dictionary<string, Directory> SubdirDict;
            public Directory Previous { get; private set; }
            public string Name { get; private set; }
            public int Nesting { get; private set; }

            public Directory()
            {
                SubdirDict = new Dictionary<string, Directory>();
                Previous = null;
                Name = null;
                Nesting = -1;
            }

            public Directory(Directory previous, string value)
            {
                SubdirDict = new Dictionary<string, Directory>();
                Previous = previous;
                Name = value;
                Nesting = Previous.Nesting + 1;
            }

            private string GetNestingSpaces()
            {
                var result = new StringBuilder();
                for (int i = 0; i < this.Nesting; i++)
                    result.Append(" ");
                return result.ToString();
            }

            private string GetDirectoryNameWithNesting()
                => String.Concat(this.GetNestingSpaces(), this.Name);

            public IEnumerable<string> GetDirectoriesByPriority()
            {
                yield return this.GetDirectoryNameWithNesting();
                if (this.SubdirDict.Count == 0) yield break;
                foreach (var directory in this.SubdirDict
                        .OrderBy(x => x.Value.Name, StringComparer.Ordinal))
                    foreach(var dirName in directory.Value.GetDirectoriesByPriority())
                        yield return dirName;
            }
        }

        private static Directory GetTreeOfDirectories(List<string> input)
        {
            var fictiveRoot = new Directory();
            foreach (var path in input)
            {
                var splitPath = path.Split('\\');
                var previousDir = fictiveRoot;
                foreach (var dirName in splitPath)
                {
                    var currentDir = new Directory(previousDir, dirName);
                    if (!previousDir.SubdirDict.ContainsKey(currentDir.Name))
                    {
                        previousDir.SubdirDict[currentDir.Name] = currentDir;
                        previousDir = currentDir;
                    }
                    else
                        previousDir = previousDir.SubdirDict[currentDir.Name];
                }
            }
            return fictiveRoot;
        }
    
        public static List<string> Solve(List<string> input)
        {
            Directory treeRoot = GetTreeOfDirectories(input);
            return treeRoot.SubdirDict
                .OrderBy(x => x.Value.Name, StringComparer.Ordinal)
                .SelectMany(x => x.Value.GetDirectoriesByPriority())
                .ToList();
        }
    }
}
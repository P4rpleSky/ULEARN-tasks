using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        private static IEnumerable<TOut> GetCustomTraversal<TIn, TOut>(
            TIn currentRoot, 
            Func<TIn, IEnumerable<TOut>> itemsSelector,
            Func<TIn, IEnumerable<TIn>> nextRootsSelector)
        {
            foreach (var item in itemsSelector(currentRoot))
                yield return item;
            var nextRoots = nextRootsSelector(currentRoot);
            if(nextRoots != null)
                foreach (var nextRoot in nextRoots)
                    foreach (var item in GetCustomTraversal(nextRoot, itemsSelector, nextRootsSelector))
                        yield return item;
        }
        
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            return GetCustomTraversal(
                root, 
                (productCategory) => productCategory.Products,
                (currentCategory) => currentCategory.Categories);
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            return GetCustomTraversal(
                root,
                (job) => job.Subjobs.Count == 0 ? new[] { job } : new Job[0],
                (currentJob) => currentJob.Subjobs);
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            return GetCustomTraversal(
                root,
                (treeNode) => treeNode.Left is null && treeNode.Right is null ? 
                        new[] { treeNode.Value } : new T[0],
                (currentNode) =>
                {
                    var result = new List<BinaryTree<T>>();
                    if (currentNode.Left != null) result.Add(currentNode.Left);
                    if (currentNode.Right != null) result.Add(currentNode.Right);
                    return result;
                });
        }
    }
}
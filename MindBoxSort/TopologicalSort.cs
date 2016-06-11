using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindBoxSort
{
    /// <summary>
    /// A little tuned variant of "depth-first search" algoritm from here:
    /// http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
    /// </summary>
    public static class TopologicalSortForOneDependency
    {
        private static Func<T, T> MapDependency<T, TKey>(IEnumerable<T> source, Func<T, TKey> getDependency, Func<T, TKey> getKey) where T : class
        {
            Dictionary<TKey, T> map = source.ToDictionary(getKey);
            return item => 
            {
                TKey dependency = getDependency(item);
                T value;
                if (dependency != null && map.TryGetValue(dependency, out value))
                {
                    return value;
                }
                return null;
            };
        }

        public static IList<T> Sort<T, TKey>(IEnumerable<T> source, Func<T, TKey> getDependency, Func<T, TKey> getKey, bool ignoreCycles = false) where T : class
        {
            return Sort<T>(source, MapDependency(source, getDependency, getKey), ignoreCycles);
        }

        private static IList<T> Sort<T>(IEnumerable<T> source, Func<T, T> getDependency, bool ignoreCycles = false) where T : class
        {
            var sorted = new List<T>();

            var visited = new Dictionary<T, bool>();

            foreach (var item in source)
            {
                Visit(item, getDependency, sorted, visited, ignoreCycles);
            }

            return sorted.ToArray().Reverse().ToList();
        }

        private static void Visit<T>(T item, Func<T, T> getDependency, List<T> sorted, Dictionary<T, bool> visited, bool ignoreCycles)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess && !ignoreCycles)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[item] = true;

                var dependency = getDependency(item);
                if (dependency != null)
                {
                    Visit(dependency, getDependency, sorted, visited, ignoreCycles);
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }
    }
}

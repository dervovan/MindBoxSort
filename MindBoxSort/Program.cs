using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindBoxSort
{
    class Program
    {
        private const int townNameLength = 12;
        private const int numberOfCards = 100;

        static void Main(string[] args)
        {
            TownDataService dataService = new TownDataService(townNameLength);

            var data = dataService.GetNoCyclicData(numberOfCards);

            //// shuffle data before sorting
            Random rand = new Random();
            var unsorted = data.OrderBy(o => rand.Next());

            var sorted = TopologicalSortForOneDependency.Sort(unsorted, o => o.To, o => o.From);

            Console.WriteLine();
        }
    }
}

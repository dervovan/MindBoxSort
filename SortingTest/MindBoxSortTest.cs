using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindBoxSort;
using System.Collections.Generic;
using System.Linq;

namespace SortingTest
{
    [TestClass]
    public class MindBoxSortTest
    {
        public int townNameLength = 8;
        public int cardCollectionLength = 100;

        [TestMethod]
        public void TestNoCyclicDataHundredTimes()
        {
            TownDataService dataService = new TownDataService(townNameLength);

            for (int i = 0; i < 100; i++)
            {
                var data = dataService.GetNoCyclicData(cardCollectionLength);

                //// shuffle data before sorting
                Random rand = new Random();
                var unsorted = data.OrderBy(o => rand.Next());

                var sorted = TopologicalSortForOneDependency.Sort(unsorted, o => o.To, o => o.From);

                CollectionAssert.AreEqual(data.ToArray(), sorted.ToArray());
            }
        }

        [TestMethod]
        public void TestCyclicDataHundredTimes()
        {
            TownDataService dataService = new TownDataService(townNameLength);

            for (int i = 0; i < 100; i++)
            {
                var data = dataService.GetCyclicData(cardCollectionLength);

                //// shuffle data before sorting
                Random rand = new Random();
                var unsorted = data.OrderBy(o => rand.Next()).ToList();

                var sorted = TopologicalSortForOneDependency.Sort(unsorted, o => o.To, o => o.From, true);

                //// because of the cyclic reference for proper comparison 
                //// we need to position initial data start element to the
                //// begining of the sorted cllection
                var startElementIndex = sorted.IndexOf(data.First());
                var normalizedSorted = sorted
                                        .Skip(startElementIndex)
                                        .ToList();
                normalizedSorted.AddRange(sorted.Take(startElementIndex));

                CollectionAssert.AreEqual(data.ToArray(), normalizedSorted.ToArray());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Cyclic dependency found.")]
        public void TestCyclicDataException()
        {
            TownDataService dataService = new TownDataService(townNameLength);

            var data = dataService.GetCyclicData(cardCollectionLength);

            //// shuffle data before sorting
            Random rand = new Random();
            var unsorted = data.OrderBy(o => rand.Next()).ToList();

            var sorted = TopologicalSortForOneDependency.Sort(unsorted, o => o.To, o => o.From);
        }

        [TestMethod]
        public void TestCyclicDataNoException()
        {
            TownDataService dataService = new TownDataService(townNameLength);

            var data = dataService.GetCyclicData(cardCollectionLength);

            //// shuffle data before sorting
            Random rand = new Random();
            var unsorted = data.OrderBy(o => rand.Next()).ToList();

            var sorted = TopologicalSortForOneDependency.Sort(unsorted, o => o.To, o => o.From, true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MindBoxSort
{
    public class TownDataService
    {
        /// <summary>
        /// size of the town name
        /// </summary>
        private int _minWordSize;

        /// <summary>
        /// alhpabet for town names
        /// </summary>
        private List<char> _possibleLetters;

        private List<string> _createdNames;
        private Random _rand;

        /// <summary>
        /// Helper object for generating some cards data
        /// </summary>
        public TownDataService(int townNameSize) 
        {
            _minWordSize = townNameSize;

            _possibleLetters = new List<char>();
            for (char ch = 'A'; ch <= 'Z'; ch++)
            {
                _possibleLetters.Add(ch);
            }

            _createdNames = new List<string>();
            _rand = new Random();
        } 

        /// <summary>
        /// Generate town cards
        /// </summary>
        private List<TownPair> GetData (int count)
        {
            _createdNames.Clear();
            var result = new List<TownPair>();
            var firstPair = new TownPair (
                CompileWord(_possibleLetters, _minWordSize), 
                CompileWord(_possibleLetters, _minWordSize));
            
            result.Add(firstPair);
            var currentPair = firstPair;

            for (int i = 0; i < count - 2; i++)
            {
                var newPair = GetNewPairBasedOnExisting(_possibleLetters, currentPair, _minWordSize);
                result.Add(newPair);
                currentPair = newPair;
            }

            return result;
        }

        /// <summary>
        /// Prepare data for unit testing with no cyclic references
        /// </summary>
        public IList<TownPair> GetNoCyclicData(int count) 
        {
            var data = GetData(count);
            data.Add(new TownPair (data.Last().To));
            return data;
        }

        /// <summary>
        /// Prepare data for unit testing with cyclic references - circle route
        /// </summary>
        public IList<TownPair> GetCyclicData(int count)
        {
            var data = GetData(count);
            data.Add(new TownPair(data.Last().To, data.First().From));
            return data;
        }

        /// <summary>
        /// To mimic real data we need to know previous pair to build next
        /// </summary>
        private TownPair GetNewPairBasedOnExisting(List<char> letters, TownPair pair, int wordSize)
        {
            return new TownPair(pair.To, CompileWord(letters, wordSize));
        }

        /// <summary>
        /// Compiling new random word (town name in our case) based on possible caracters and desired size
        /// </summary>
        private string CompileWord(List<char> letters, int wordSize)
        {
            char[] word = new char[wordSize];
            int maxLetter = letters.Count;

            for (int i = 0; i < wordSize; i++)
            {
                word[i] = letters[_rand.Next(maxLetter)];
            }
            var result = new String(word);

            //// we don't need collisions in our random town names
            //// that will be cyclic references
            if (_createdNames.Contains(result))
            {
                result += DateTime.Now.Ticks.ToString();
            }

            _createdNames.Add(result);

            return result;
        }
    }
}

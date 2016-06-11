using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindBoxSort
{
    /// <summary>
    /// Data structure
    /// </summary>
    public class TownPair
    {
        public string From { get; private set; }
        public string To { get; private set; }

        /// <summary>
        /// initialization and cheking for parameters errors 
        /// </summary>
        public TownPair(string from, string to = null) 
        {
            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException("we can't come out of nowere");
            }

            From = from;
            To = to;
        }
    }
}

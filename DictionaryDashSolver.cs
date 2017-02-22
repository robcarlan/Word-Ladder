using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryDash
{
    /// <summary>
    /// The solver class for the problem. Responsibility of this class is to validate the data, and then pass to the algorithm. 
    /// The user is given control over both the strategy used and the dictionary.
    /// </summary>
    public class DictionaryDashSolver
    {
        public const int INVALID_INPUT = -2;
        public const int NO_TRANSFORMATION = -1;

        public DictionaryDashStrategy strategy { get; set; }
        public WordDictionary dictionary { get; set; }

        public DictionaryDashSolver(DictionaryDashStrategy strategy, WordDictionary dictionary)
        {
            this.strategy = strategy;
            this.dictionary = dictionary;
        }

        /// <summary>
        /// Attempts to solve 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The minimum length to transform one word to another. INVALID_INPUT (-2) if this word does not exist, NO_TRANSFORMATION (-1) if no transformation</returns>
        public int solve(string start, string end)
        {
            bool valid = true;

            //perform validation of arguments here
            if (start.Length != end.Length)
            {
                valid = false;
                Console.WriteLine("Error, {0} and {1} are of different lengths.", start, end);
            }

            if (!dictionary.isInDictionary(start))
            {
                valid = false;
                Console.WriteLine("Error, {0} is not contained in the dictionary.", start);
            }

            if (!dictionary.isInDictionary(end))
            {
                valid = false;
                Console.WriteLine("Error, {0} is not contained in the dictionary.", end);
            }

            if (!valid) return INVALID_INPUT;

            int result = strategy.getShortestTransformation(start, end, dictionary);

            return result;
        }
        
    }
}

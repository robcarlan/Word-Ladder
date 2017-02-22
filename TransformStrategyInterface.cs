using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DictionaryDash.GraphFunctions;

namespace DictionaryDash
{
    public abstract class DictionaryDashStrategy
    {

        //What is returned on errorenous inputs / invalid
        /// <summary>
        /// precondition: length(start) == length(end), and both start and end are contained in the WordDictionary
        ///     makes sense, as this will be called by DictionaryDashSolver
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>
        /// -1 if could not find transformation
        /// </returns>
        public abstract int getShortestTransformation(string start, string end, WordDictionary words);
    }

    /// <summary>
    /// Solves dictionary dash by use of a BFS. Note that there might exist non graph based methods to solving this problem!
    /// </summary>
    public abstract class BFSStrategy : DictionaryDashStrategy
    {

        protected GraphBase<string> graph;

        public override int getShortestTransformation(string start, string end, WordDictionary words)
        {
            Tuple<bool, List<string>> result = graphSearchBFS<string>(graph, start, end);

            if (result.Item1 == false)
            {
                //Item not found, no transformation
                return DictionaryDashSolver.NO_TRANSFORMATION;
            } else
            {

                //Simple testing method to verify all words in a path are in the dictionary.
                result.Item2.ForEach(s => System.Diagnostics.Debug.Assert(words.isInDictionary(s)));

                //For this implementation, we will always print the path.
                Console.WriteLine("Path: {0}", string.Join(" -> ", result.Item2.ToArray()));

                if (result.Item2.Count == 0) return 0; //0 hops
                return result.Item2.Count - 1;
            }
        }
    }


    //These different BFS strategies could have been implemented in BFSStrategy as a boolean, but these subclasses fit the idea of the strategy pattern more.
    //I.e. if a new Graph was created, only a new BFSStrat needs to be created, rather than changing BFSStrategy's ctor to handle more strategies 
    public class BFSStrategyMemoized : BFSStrategy
    {
        public BFSStrategyMemoized(WordDictionary words)
        {
            graph = new GraphBase<string>(words.wordSet, new MemoizedWordGraphStrategy());
        }
    }

    public class BFSStrategyLowMemory : BFSStrategy
    {
        public BFSStrategyLowMemory(WordDictionary words)
        {
            graph = new GraphBase<string>(words.wordSet, new LowMemoryWordGraphStrategy());
        }
    }

}

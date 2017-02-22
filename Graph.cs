using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryDash
{
    public static class GraphFunctions
    {
        //These classes implement various graph structures / algorithms which are not unique to the DictionaryDash solution. DictionaryDash uses the Graph<string> classes and 
        //BFS<string> of these in order to solve the problem.

        /// <summary>
        /// This class acts as a representation of a graph to be used in graph algorithms. The repressentation is somewhat more complicated as it allwos a strategy for creating the edges of a graph.
        /// </summary>
        public class GraphBase<T>
        {
            protected ISet<T> set;

            protected IDictionary<T, List<T>> graph;
            protected GraphCreateStrategy<T> createStrategy;

            public GraphBase(ISet<T> set, GraphCreateStrategy<T> strat)
            {
                this.set = set;
                this.createStrategy = strat;
                graph = new Dictionary<T, List<T>>();

                createStrategy.onGraphInitialise(set, graph);
            }

            public List<T> getNext(T word)
            {
                return createStrategy.getEdges(word, set, graph);
            }
        }

        //BFS algorithm which tries to find a path from start to end
        public static Tuple<bool, List<T>> graphSearchBFS<T>(GraphBase<T> graph, T start, T end)
        {
            
            HashSet<T> visitedNode = new HashSet<T>();

            Dictionary<T, T> parentDict = new Dictionary<T, T>();

            Queue<T> toExplore = new Queue<T>();

            toExplore.Enqueue(start);

            //start == end => we found a simple path
            if (start.Equals(end)) return new Tuple<bool, List<T>>(true, new List<T> { start });

            bool done = false;

            //BFS which also ends when end is found
            while (toExplore.Count > 0 && !done)
            {
                T currentNode = toExplore.Dequeue();

                List<T> nextNodes = graph.getNext(currentNode);

                nextNodes = nextNodes.FindAll(s => !visitedNode.Contains(s));

                foreach (T n in nextNodes)
                { 
                    if (end.Equals(n))
                    {
                        //End node found, so we can exit early
                        done = true;

                        //Create the path, backwards, and return it.
                        List<T> path = new List<T> { end };
                        T parentNode = currentNode;

                        while (!parentNode.Equals(start))
                        {
                            //Prepend every element until we get back to the start
                            path.Insert(0, parentNode);
                            parentNode = parentDict[parentNode];
                        }

                        path.Insert(0, start);

                        return new Tuple<bool, List<T>>(true, path);
                    }

                    visitedNode.Add(n);
                    toExplore.Enqueue(n);
                    parentDict[n] = currentNode;
                }
            }

            //All nodes explored, and end was never reached. Hence we return false.
            return new Tuple<bool, List<T>>(false, new List<T>());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryDash
{
    /// <summary>
    /// This class was created because the method for building a graph:
    ///     a) varies from type to type and implementation to implementation (A wordgraph is constructed uniquely to this problem)
    ///     b) Even within the given problem, multiple strategies exist. Two are highlighted below
    /// </summary>
    /// <typeparam name="T">The type of nodes in the graph</typeparam>
    public interface GraphCreateStrategy<T>
    {
        void onGraphInitialise(ISet<T> set, IDictionary<T, List<T>> graph);
        List<T> getEdges(T start, ISet<T> set, IDictionary<T, List<T>> graph);
    }

    /// <summary>
    /// This method builds the graph at the start, and stores all of it in memory. As a result, this is likely to use a large amount of resources for large dictionaries.
    /// </summary>
    public class MemoizedWordGraphStrategy : GraphCreateStrategy<string>
    {
        public List<string> getEdges(string start, ISet<string> set, IDictionary<string, List<string>> graph)
        {
            return graph[start];
        }

        public void onGraphInitialise(ISet<string> set, IDictionary<string, List<string>> graph)
        {
            Dictionary<string, List<string>> wordToBuckets  = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> buckets        = new Dictionary<string, List<string>>();

            foreach (string word in set)
            {
                //For a given word, we can create a bucket for each variation of the word without a given letter
                //ie., dog will be added to the buckets _og, d_g, do_, representing one transformation.
                //We then get each edge by collecting the entries in each bucket.
                //So edges(dog) = bucket[_og] ++ bucket[d_g] ++ bucket[do_]

                List<string> variations = new List<string>();

                for (int i = 0; i < word.Length; i++)
                {
                    string wordVariation = word.Remove(i, 1).Insert(i, "_");
                    variations.Add(wordVariation);

                    //initialise bucket if not already
                    if (!buckets.ContainsKey(wordVariation))
                        buckets[wordVariation] = new List<string> { word };

                    buckets[wordVariation].Add(word);
                }

                wordToBuckets[word] = variations;

                //Initialise graph
                graph[word] = new List<string>();
            }

            //Collect each bucket to create the edges
            foreach (string word in set)
            {
                List<string> edges = new List<string>();
                List<string> listBucketsForWord = wordToBuckets[word];

                foreach(string wordVariationBucket in listBucketsForWord)
                {
                    edges.AddRange(buckets[wordVariationBucket]);
                }

                //Remove occurences of own word
                edges.RemoveAll(s => s == word);

                graph[word] = edges;
            }

            return;
        }
    }

    /// <summary>
    /// This method uses a negligible amount of memory by creating the list on demand, when each word is queried. As a result, it will be slower, but use less memory.
    /// I didn't implement this class, as it is more for the sake of example
    /// </summary>
    public class LowMemoryWordGraphStrategy : GraphCreateStrategy<string>
    {
        public List<string> getEdges(string start, ISet<string> set, IDictionary<string, List<string>> graph)
        {
            throw new NotImplementedException();
        }

        public void onGraphInitialise(ISet<string> set, IDictionary<string, List<string>> graph)
        {
            throw new NotImplementedException();
        }
    }
}

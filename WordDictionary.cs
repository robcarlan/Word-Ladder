using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryDash
{
    /// <summary>
    /// This class acts to load a list of words from a text file, format them, and place them inside a set to be used to create the graph
    /// </summary>
    /// 

    public class WordDictionary
    {
        int minLength = int.MaxValue;
        int maxLength = 0;

        public int getMinLength() { return minLength; }
        public int getMaxLength() { return maxLength; }

        bool isLoaded = false;
        public ISet<string> wordSet { get; }

        public WordDictionary(string fpath)
        {
            wordSet = new HashSet<string>();

            loadDictionary(fpath);
        }

        public bool isInitialised()
        {
            return isLoaded;
        }

        protected bool loadDictionary(string fpath)
        {

            try
            {
                string[] lines = System.IO.File.ReadAllLines(fpath);

                foreach (string line in lines)
                {
                    //Perform some formatting on each loaded line - remove any unwanted characters
                    string formatted = line.ToLower();

                    if (!formatted.All(Char.IsLetter))
                    {
                        //Unexpected characters, will handle by skipping this line for simplicity.
                        continue;
                    }

                    maxLength = Math.Max(maxLength, formatted.Length);
                    minLength = Math.Min(minLength, formatted.Length);

                    wordSet.Add(formatted);
                }

                Console.WriteLine("Loaded " + wordSet.Count + " words.");

                isLoaded = true;
                return true;
            }
            catch (System.IO.IOException e)
            {

                Console.WriteLine("Exception reading word list from filepath: " + fpath + ", exception details: " + e.Message);

                return false;

            }
            
        }

        /// <summary>
        /// Simple set method.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool isInDictionary(string word)
        {
            System.Diagnostics.Debug.Assert(isInitialised());

            return wordSet.Contains(word);
        }
    }
}

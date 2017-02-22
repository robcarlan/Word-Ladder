using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryDash
{
    class Program
    {

        static void Main(string[] args)
        {

            string loadPath = "words.txt";
            bool doTesting = true;

            if (args.Length == 0)
            {
                //Load dictionary from default path, and do testing, this is the default case so nothing happens

            }
            else if (args.Length != 1)
            {
                Console.WriteLine("Usage: DictionaryDash.exe path_to_dictionary to manually test the program on a supplied dictionary. ");
                Console.WriteLine("DictionaryDash.exe to run tests on the default dictionary, words.txt");

                return;
            } else
            {
                //Try loading dictionary from supplied path.
                loadPath = args[0];
                doTesting = false;
            }

            WordDictionary words = new WordDictionary(loadPath);

            if (!words.isInitialised())
            {
                Console.WriteLine("Error loading WordDictionary. Quitting.");
                return;
            }

            DictionaryDashSolver solverMemoized = new DictionaryDashSolver(
                new BFSStrategyMemoized(words),
                words);

            #region testing

            if (doTesting)
            {
               
                bool passedDictionaryTests = true;

                Console.WriteLine("Running tests:");

                Console.WriteLine("Testing dictionary:");
                passedDictionaryTests = passedDictionaryTests && 
                    testIsInDictionary("dog", words, true);
                passedDictionaryTests = passedDictionaryTests && 
                    testIsInDictionary("happy", words, true);
                passedDictionaryTests = passedDictionaryTests && 
                    testIsInDictionary("Dog", words, false);
                passedDictionaryTests = passedDictionaryTests && 
                    testIsInDictionary("Aaron", words, false);
                passedDictionaryTests = passedDictionaryTests && 
                    testIsInDictionary("abc", words, false);
                passedDictionaryTests = passedDictionaryTests && 
                    testIsInDictionary("_1dog", words, false);

                bool passedSolvingTests = true;

                //Considers:
                // Words of different length, words not in the dictionary
                // The same word, words with transformations, words without
                //Long words, short words

                Console.WriteLine("Testing solver:");
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("dog", "dog", solverMemoized, 0);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("aaa", "aaa", solverMemoized, DictionaryDashSolver.INVALID_INPUT);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("dog", "face", solverMemoized, DictionaryDashSolver.INVALID_INPUT);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("face", "fact", solverMemoized, 1);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("face", "aaaa", solverMemoized, DictionaryDashSolver.INVALID_INPUT);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("aaaa", "face", solverMemoized, DictionaryDashSolver.INVALID_INPUT);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("face", "dogs", solverMemoized, 5);
                passedSolvingTests = passedSolvingTests &&
                     testTransformationLength("happy", "beach", solverMemoized, 8);

                //Its quite difficult to find words that almost work with this weird dictionary I downloaded
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("aaaa", "face", solverMemoized, DictionaryDashSolver.INVALID_INPUT);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("aardvark", "aardwolf", solverMemoized, DictionaryDashSolver.NO_TRANSFORMATION);
                passedSolvingTests = passedSolvingTests &&
                    testTransformationLength("juicy", "peach", solverMemoized, DictionaryDashSolver.NO_TRANSFORMATION);

                if (passedDictionaryTests)
                {
                    Console.WriteLine("Passed Dictionary tests!");
                }
                else
                {
                    Console.WriteLine("Failed Dictioanry tests!");
                }

                if (passedSolvingTests)
                {
                    Console.WriteLine("Passed Solving tests!");
                }
                else
                {
                    Console.WriteLine("Failed Solving tests!");
                }

                if (passedSolvingTests && passedDictionaryTests)
                {
                    Console.WriteLine("Passed all tests.");
                }
                else
                {
                    Console.WriteLine("Failed some tests.");
                }
            }

            #endregion testing

            string input = "";

            bool quit = false;

            while (!quit)
            {
                //Get words, then test on these words
                string word1, word2;
                Console.WriteLine("Enter first word (q to quit): ");
                word1 = Console.ReadLine();
                Console.WriteLine("Enter second word (q to quit): ");
                word2 = Console.ReadLine();

                if (word1 == "q") quit = true;
                if (word2 == "q") quit = true;

                if (!quit)
                {
                    Console.WriteLine("Transformation length between {0} and {1}", word1, word2);
                    int result = solverMemoized.solve(word1, word2);

                    if (result == DictionaryDashSolver.NO_TRANSFORMATION)
                    {
                        Console.WriteLine("No transformation between {0} and {1}.", word1, word2);
                    }
                    else if (result == DictionaryDashSolver.INVALID_INPUT)
                    {
                        Console.WriteLine("{0} and {1} invalid input.", word1, word2);
                    }
                    else 
                        Console.WriteLine("Solved in {0} hops.", result);
                }
            }

            return;
        }


        //Simple testing helper functions ---------

        static public bool testIsInDictionary(string word, WordDictionary dictionary, bool expectedResult)
        {
            Console.WriteLine("Is {0} in the dictionary:", word);

            bool result = dictionary.isInDictionary(word);

            Console.WriteLine("{0} = {1}, expected {2}", word, result, expectedResult);

            return (result == expectedResult);
        }

        static public bool testTransformationLength(string word1, string word2, DictionaryDashSolver solver, int expectedResult)
        {
            Console.WriteLine("Testing transformation length between {0} and {1}", word1, word2);

            int result = solver.solve(word1, word2);

            Console.WriteLine("Returned {0}, expected {1}", result, expectedResult);

            return (result == expectedResult);
        }
    }
}

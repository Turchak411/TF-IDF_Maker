using System;
using System.Collections.Generic;
using TF_IDF_Maker.Model;
using TF_IDF_Maker.Services;

namespace TF_IDF_Maker
{
    class Program
    {
        static void Main(string[] args)
        {
            FileManager fileManager = new FileManager();

            TFIDFCalculator tfidfCalculator = new TFIDFCalculator(fileManager);

            string fileName = "..//..//..//Data//yelp_labelled.txt";

            List<TFIDFNote> dictionary = tfidfCalculator.GetIFIDFDictionary(fileName);

            PrintValues(dictionary);

            fileManager.SaveValues(dictionary);

            Console.ReadKey();
        }

        private static void PrintValues(List<TFIDFNote> dictionary)
        {
            for (int i = 0; i < 15; i++) //dictionary.Count; i++)
            {
                Console.WriteLine($"=======================================\nWord: {dictionary[i].Word}" +
                                  "\n=======================================\nTFIDF values:");

                for (int k = 0; k < dictionary[i].ValuesList.Count; k++)
                {
                    Console.WriteLine($"\n\t> Document name: \"{dictionary[i].ValuesList[k].DocumentName}\"");
                    Console.WriteLine($"\t> Value: \"{dictionary[i].ValuesList[k].Value}\"");
                }

                Console.WriteLine("=======================================\n");
            }
        }
    }
}

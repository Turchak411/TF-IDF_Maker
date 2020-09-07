using System;
using System.Collections.Generic;
using TF_IDF_Maker.Model;

namespace TF_IDF_Maker
{
    class Program
    {
        static void Main(string[] args)
        {
            TFIDFCalculator tfidfCalculator = new TFIDFCalculator();

            string fileName = "yelp_labelled.txt";

            List<TFIDFNote> dictionary = tfidfCalculator.GetIFIDFDictionary(fileName);

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

            Console.ReadKey();
        }
    }
}

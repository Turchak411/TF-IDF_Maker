﻿using System;
using System.Collections.Generic;
using Porter2Stemmer;
using TF_IDF_Maker.Model;

namespace TF_IDF_Maker.Services
{
    public class TFIDFCalculator
    {
        private FileManager _fileManager;

        private TFIDFCalculator() { }

        public TFIDFCalculator(FileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public List<TFIDFNote> GetIFIDFDictionary(string generalFilename)
        {
            // Divide general file and load words from positive& negative documents:
            FileManager fileManager = new FileManager();

            fileManager.DivideDocument(generalFilename);

            List<string> filePathList = new List<string>();
            filePathList.Add("positive.txt");
            filePathList.Add("negative.txt");

            List<List<string>> documents = fileManager.LoadDocuments(filePathList);

            // Fill TF-IDF dictionary and return:
            List<TFIDFNote> dictionary = new List<TFIDFNote>();

            for (int i = 0; i < documents.Count; i++)
            {
                for (int k = 0; k < documents[i].Count; k++)
                {
                    TFIDFNote tfidfNote = new TFIDFNote();
                    tfidfNote.Word = documents[i][k];

                    // Fill the values list for each document:
                    // With word stemming:
                    EnglishPorter2Stemmer englishPorter = new EnglishPorter2Stemmer();
                    tfidfNote.ValuesList = new List<TFIDFValue>();
                    for (int j = 0; j < documents.Count; j++)
                    {
                        tfidfNote.ValuesList.Add(
                            new TFIDFValue
                            {
                                DocumentName = filePathList[j],
                                Value = GetTFIDFValue(englishPorter.Stem(documents[i][k]), documents[j], documents)
                            });
                    }

                    dictionary.Add(tfidfNote);
                }
            }

            return dictionary;
        }

        private double GetTFIDFValue(StemmedWord word, List<string> currentDocument, List<List<string>> allDocuments)
        {
            return GetTFValue(word, currentDocument) * GetIDFValue(word, allDocuments);
        }

        private double GetTFValue(StemmedWord word, List<string> document)
        {
            int countOfOccurs = document.FindAll(x => x == word.Unstemmed).Count;
            return (double)countOfOccurs / (double)document.Count;
        }

        private double GetIDFValue(StemmedWord word, List<List<string>> allDocuments)
        {
            int countOfDocOccurs = 0;

            for (int i = 0; i < allDocuments.Count; i++)
            {
                countOfDocOccurs += allDocuments[i].Contains(word.Unstemmed) ? 1 : 0;
            }

            return Math.Log10((double)allDocuments.Count / (double)countOfDocOccurs);
        }
    }
}

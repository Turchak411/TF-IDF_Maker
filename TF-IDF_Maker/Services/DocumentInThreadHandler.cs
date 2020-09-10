using System;
using System.Collections.Generic;
using Porter2Stemmer;
using TF_IDF_Maker.Model;

namespace TF_IDF_Maker.Services
{
    public class DocumentInThreadHandler
    {
        public List<TFIDFNote> DocumentDictionary { get; set; }

        private List<List<string>> _documents;

        private int _documentIndex;

        private List<string> _filePathList;

        private DocumentInThreadHandler() { }

        public DocumentInThreadHandler(List<List<string>> documents, int documentIndex, List<string> filePathList)
        {
            _documents = documents;
            _documentIndex = documentIndex;
            _filePathList = filePathList;
        }

        public void Handle()
        {
            for (int k = 0; k < _documents[_documentIndex].Count; k++)
            {
                TFIDFNote tfidfNote = new TFIDFNote();
                tfidfNote.Word = _documents[_documentIndex][k];

                // Fill the values list for each document:
                // With word stemming:
                EnglishPorter2Stemmer englishPorter = new EnglishPorter2Stemmer();
                tfidfNote.ValuesList = new List<TFIDFValue>();
                for (int j = 0; j < _documents.Count; j++)
                {
                    tfidfNote.ValuesList.Add(
                        new TFIDFValue
                        {
                            DocumentName = _filePathList[j],
                            Value = GetTFIDFValue(englishPorter.Stem(_documents[_documentIndex][k]), _documents[j], _documents)
                        });
                }

                DocumentDictionary.Add(tfidfNote);
            }
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

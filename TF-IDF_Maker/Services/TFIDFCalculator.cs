using System.Collections.Generic;
using System.Threading;
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

        public List<TFIDFNote> GetIFIDFDictionaryFromDocuments(string positiveDocumentPath, string negativeDocumentPath)
        {
            try
            {
                // Divide general file and load words from positive& negative documents:
                FileManager fileManager = new FileManager();

                List<string> filePathList = new List<string>();
                filePathList.Add(positiveDocumentPath);
                filePathList.Add(negativeDocumentPath);

                List<List<string>> documents = fileManager.LoadDocuments(filePathList);

                // Fill TF-IDF dictionary and return:
                return GetFilledDictionary(documents, filePathList);
            }
            catch
            {
                return new List<TFIDFNote>();
            }
        }

        public List<TFIDFNote> GetIFIDFDictionaryFromStructuredDocument(string generalFilename)
        {
            try
            {
                // Divide general file and load words from positive& negative documents:
                FileManager fileManager = new FileManager();

                fileManager.DivideDocument(generalFilename);

                List<string> filePathList = new List<string>();
                filePathList.Add("positive.txt");
                filePathList.Add("negative.txt");

                List<List<string>> documents = fileManager.LoadDocuments(filePathList);

                // Fill TF-IDF dictionary and return:
                return GetFilledDictionary(documents, filePathList);
            }
            catch
            {
                return new List<TFIDFNote>();
            }
        }

        private List<TFIDFNote> GetFilledDictionary(List<List<string>> documents, List<string> filePathList)
        {
            List<TFIDFNote> dictionary = new List<TFIDFNote>();

            // Multithreading in document handling:
            List<SingleDocumentHandler> docThreadHandlers = new List<SingleDocumentHandler>();
            List<Thread> threadListHandling = new List<Thread>();

            for (int i = 0; i < documents.Count; i++)
            {
                SingleDocumentHandler docThreadHandler = new SingleDocumentHandler(documents, i, filePathList);
                docThreadHandlers.Add(docThreadHandler);

                threadListHandling.Add(new Thread(docThreadHandlers[i].Handle));
                threadListHandling[i].Start();
            }

            // Wait all doc-threads:
            Wait(threadListHandling);

            // Unions dictionary from all documents:
            for (int i = 0; i < docThreadHandlers.Count; i++)
            {
                dictionary.AddRange(docThreadHandlers[i].DocumentDictionary);
            }

            return dictionary;
        }

        /// <summary>
        /// Ожидание завершения всех потоков
        /// </summary>
        /// <param name="threadList"></param>
        private void Wait(List<Thread> threadList)
        {
            while (true)
            {
                int WorkCount = 0;

                for (int i = 0; i < threadList.Count; i++)
                {
                    WorkCount += (threadList[i].IsAlive) ? 0 : 1;
                }

                if (WorkCount == threadList.Count) break;
            }
        }
    }
}

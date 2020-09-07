using System.Collections.Generic;
using System.IO;

namespace TF_IDF_Maker.Services
{
    public class FileManager
    {
        public List<List<string>> LoadDocuments(List<string> filePathList)
        {
            List<List<string>> documentsContentList = new List<List<string>>();

            for (int i = 0; i < filePathList.Count; i++)
            {
                List<string> documentContent = new List<string>();

                using (StreamReader fileReader = new StreamReader(filePathList[i]))
                {
                    while (!fileReader.EndOfStream)
                    {
                        string[] readedLine = fileReader.ReadLine().Split(' ');

                        for (int k = 0; k < readedLine.Length; k++)
                        {
                            documentContent.Add(readedLine[k].Trim(
                                new char[]
                                {
                                    ' ', ',', '.', '\'', '#', '!', '?', '$', '@', '-', '+',
                                    '=', '^', '№', '%', '&', ';', ':', '(', ')', '*', '_',
                                    '<', '>', '\\', '\'', '~'
                                }));
                        }
                    }
                }

                documentsContentList.Add(documentContent);
            }

            return documentsContentList;
        }

        public void DivideDocument(string generalFile)
        {
            List<string> posDocument = new List<string>();
            List<string> negDocument = new List<string>();

            using (StreamReader fileReader = new StreamReader(generalFile))
            {
                while (!fileReader.EndOfStream)
                {
                    string[] readedLine = fileReader.ReadLine().Split('\t');

                    // Universal structure hanling:
                    if (readedLine[1] == "1")
                    {
                        posDocument.Add(readedLine[0]);
                    }

                    if (readedLine[1] == "0")
                    {
                        negDocument.Add(readedLine[0]);
                    }
                }
            }

            // Saving documents:
            WriteDocument(posDocument, "positive.txt");
            WriteDocument(negDocument, "negative.txt");
        }

        private void WriteDocument(List<string> document, string documentName)
        {
            using (StreamWriter fileWriter = new StreamWriter(documentName))
            {
                for (int i = 0; i < document.Count; i++)
                {
                    fileWriter.WriteLine(document[i]);
                }
            }
        }
    }
}

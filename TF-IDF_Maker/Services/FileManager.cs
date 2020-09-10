using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;
using TF_IDF_Maker.Model;

namespace TF_IDF_Maker.Services
{
    public class FileManager
    {
        public string ResultsFolder { get; set; }

        public FileManager()
        {
            ResultsFolder = "..//..//..//Data//Results";

            if (!Directory.Exists(ResultsFolder))
            {
                Directory.CreateDirectory(ResultsFolder);
            }
        }

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
                                }).ToLower());
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

        public void SaveValues(List<TFIDFNote> dictionary)
        {
            WriteValues(dictionary, "values_" + DateTime.Now.Ticks + ".txt");
        }

        public void SaveValues(List<TFIDFNote> dictionary, string path)
        {
            WriteValues(dictionary, path);
        }

        public void WriteValues(List<TFIDFNote> values, string path)
        {
            using (StreamWriter fileWriter = new StreamWriter(Path.Combine(ResultsFolder, path)))
            {
                for (int i = 0; i < values.Count; i++)
                {
                    fileWriter.Write("{0}\t", values[i].Word);

                    for (int k = 0; k < values[i].ValuesList.Count; k++)
                    {
                        fileWriter.Write(" [\"{0}\" : {1:f16}]",
                                             values[i].ValuesList[k].DocumentName,
                                             values[i].ValuesList[k].Value);
                    }

                    fileWriter.WriteLine();
                }
            }
        }
    }
}

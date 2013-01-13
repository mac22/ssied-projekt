using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataTransfer
{
    class Program
    {
        private static char SEPARATOR = ';';

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Wrong arguments!");
                Console.WriteLine("Type DataTransfer source_path destination_path");
                Console.WriteLine("Press any key to continue");
                return;
            }

            string path = args[0];
            string outputPath = args[1];

            if (!File.Exists(path))
            {
                Console.WriteLine("Source file doesn't exist!");
                Console.WriteLine("Press any key to continue");
                return;
            }

            if (File.Exists(outputPath))
            {
                Console.WriteLine("Destination file already exists!");
                Console.WriteLine("Press any key to continue");
                return;
            }

            try
            {
                List<Document> documents = ReadData(path);
                Console.WriteLine();
                SaveDataToCsv(outputPath, documents);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to continue");
            }
        }

        private static void SaveDataToCsv(string path, List<Document> documents)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach(Document d in documents)
                {
                    Console.Write("Transfering data " + d.Name + " ...");
                    string line = d.Name + SEPARATOR + "{0}" + SEPARATOR + "{1}" + SEPARATOR + d.DocumentClass;

                    foreach (KeyValuePair<string, string> kvp in d.Words)
                    {
                        writer.WriteLine(string.Format(line, kvp.Key, kvp.Value));
                    }

                    Console.WriteLine("done");
                }
            }
        }

        private static List<Document> ReadData(string path)
        {
            List<Document> result = new List<Document>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine().Trim();

                Document doc = new Document(line);
                Console.Write("Reading data " + doc.Name + " ...");
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("@@@"))
                    {
                        Console.WriteLine("done");
                        result.Add(doc);
                        doc = new Document(line);
                        Console.Write("Reading file " + doc.Name + " ...");
                        continue;
                    }
                    else if (line.StartsWith("__key") || line == null || line == "")
                        continue;

                    doc.AddWord(line);
                }
            }
            return result;
        }
    }
}

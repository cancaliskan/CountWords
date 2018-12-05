using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;

namespace CountWords
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> wordList = new List<string>();
            downloadFile();
            #region Fill Word List
            using (StreamReader sr = File.OpenText("Moby_Dick.txt"))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] tempWordList = s.Split(' ');
                    foreach (string word in tempWordList)
                    {
                        string tempWord = RemoveSpecialCharacters(word);
                        if (tempWord != "")
                            wordList.Add(tempWord.ToLower());
                    }
                }
            }
            #endregion
            findDublicatesWords(wordList);
            Console.ReadKey();
        }

        static private string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        static private void findDublicatesWords(List<string> wordList)
        {
            var q = from x in wordList
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };

            var xml = new XElement("Words", q.Select(x => new XElement("word",
                                               new XAttribute("text", x.Value),
                                               new XAttribute("count", x.Count))));

            saveXmlFile(xml);
        }

        static private void saveXmlFile(System.Xml.Linq.XElement xml)
        {
            if (File.Exists("result.xml"))
                File.Delete("result.xml");

            xml.Save("result.xml");
            Console.WriteLine("XML dosyası kaydedildi..");
        }

        static private void downloadFile()
        {
            if(File.Exists("Moby_Dick.txt"))
                File.Delete("Moby_Dick.txt");

            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            webClient.DownloadFile("http://www.gutenberg.org/files/2701/2701-0.txt", "Moby_Dick.txt");
            Console.WriteLine("Dosya İndirildi !");
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransfer
{
    class Document
    {
        public string Name { get; set; }
        public Dictionary<string, string> Words { get; set; }
        public Class DocumentClass { get; set; }

        public Document(string line)
        {
            InitializeNameAndClass(line);
            Words = new Dictionary<string, string>();
        }

        private void InitializeNameAndClass(string name)
        {
            string[] tab = name.Split('/');

            if (tab.Length < 1)
                throw new Exception("Cannot retrieve document name");

            Name = tab.Last();

            int idx = tab.ToList().IndexOf("webkb") + 1;

            Class docClass;
            if (!Enum.TryParse(tab[idx], true, out docClass))
                throw new Exception("Cannot retrieve document class");

            DocumentClass = docClass;
        }

        public void AddWord(string line)
        {
            string[] tab = line.Split(new char[] { ' ', '\t' });

            if (tab.Length != 2)
                throw new Exception("Incorrect file format");

            Words.Add(tab[0], tab[1]);
        }
    }
}

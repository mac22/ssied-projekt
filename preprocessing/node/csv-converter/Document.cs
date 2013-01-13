using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransfer
{
    class Document
    {
        private bool ParseClass(string name, out Class className)
        {
            switch(name.ToLower())
            {
                case "course":
                    className = Class.Course;
                    return true;

                case "department":
                    className = Class.Department;
                    return true;

                case "faculty":
                    className = Class.Faculty;
                    return true;

                case "other":
                    className = Class.Other;
                    return true;

                case "project":
                    className = Class.Project;
                    return true;

                case "staff":
                    className = Class.Staff;
                    return true;

                case "student":
                    className = Class.Student;
                    return true;
            }

            className = Class.Course;
            return false;
        }

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

            int calculated = tab.ToList().IndexOf("webkb");

            if (calculated == -1)
            {
                calculated = tab.ToList().IndexOf("test");

                if (calculated == -1)
                {
                    calculated = tab.ToList().IndexOf("training");
                }
            }

            int idx = calculated + 1;

            Class docClass = Class.Course;
            if (!ParseClass(tab[idx], out docClass))
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

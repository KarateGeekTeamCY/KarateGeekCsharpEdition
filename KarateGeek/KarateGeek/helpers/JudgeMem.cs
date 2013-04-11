using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarateGeek.helpers
{
    class JudgeMem
    {

        public JudgeMem()
        { 
        }

        public void save(string dir, string line)
        {
            string[] lines = { line };
            System.IO.File.WriteAllLines(dir, lines);
        }

        public string load(string dir)
        {

            if (System.IO.File.Exists(dir))
            {
                string text = System.IO.File.ReadAllText(dir);
                return text;
            }
            else
                return null;
            }
    }
}

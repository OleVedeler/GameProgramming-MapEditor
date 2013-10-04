using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Classes
{
    class ImportObject
    {
        public string name { get; set; }
        public string parent { get; set; }
        public string filename { get; set; }

        public ImportObject(){}

        public ImportObject(string name, string parent, string filename)
        {
            this.name = name;
            this.parent = parent;
            this.filename = filename;
        }
    }
}

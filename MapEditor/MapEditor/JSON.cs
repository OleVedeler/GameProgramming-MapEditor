using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapEditor
{
    public class Tile
    {
        public int id { get; set; }
        public bool isObstacle { get; set; }
    }

    class JSON
    {
        public Tile[] tiles { get; set; }
    }
}






using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace MapEditor
{
    public class Tile
    {
        public int id { get; set; }
        public int isObstacle { get; set; }
    }

    public class JSON
    {
        public List<Tile> tiles { get; set; }

        public JSON()
        {
            tiles = new List<Tile>();
        }
    }
}






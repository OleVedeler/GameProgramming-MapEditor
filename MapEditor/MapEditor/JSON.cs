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
        public int Id { get; set; }
        public bool IsObstacle { get; set; }
    }

    class JSON
    {
        public Tile[] Tiles { get; set; }

        public JSON(int size)
        {
            Tiles = new Tile[size];

            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = new Tile();
            }
        }
    }
}






using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MapEditor.Handlers
{
	class JsonHandler
	{
		public JSON _jsonFile { get; set; }

		public JsonHandler()
		{
			_jsonFile = new JSON();
		}
		/// <summary>
		/// Serializes the jasonfile
		/// </summary>
		/// <returns></returns>
		public string JsonString()
		{
			return Serialize(_jsonFile);
		}

		/// <summary>
		/// Gets the tiles
		/// Makes it shorter to write
		/// </summary>
		/// <returns></returns>
		public List<Tile> Tiles()
		{
			return _jsonFile.tiles;
		}


		/// <summary>
		/// Converts a string of json code back into a readable json file
		/// </summary>
		/// <param name="json"></param>
        public void Deserialize(String json)
        {
            _jsonFile = JsonConvert.DeserializeObject<JSON>(json);
        }

		public String Serialize(JSON json)
		{
            return JsonConvert.SerializeObject(json);
		}
	}
}

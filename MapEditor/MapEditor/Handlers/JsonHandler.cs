using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapEditor.Handlers
{
	class JsonHandler
	{
		public JsonHandler()
		{
			
		}

		public T _download_serialized_json_data<T>(string url) where T : new()
		{
			using (var w = new WebClient())
			{
				var json_data = string.Empty;
				// attempt to download JSON data as a string
				try
				{
					json_data = w.DownloadString(url);
				}
				catch (Exception) { }
				// if string with JSON data is not empty, deserialize it to class and return its instance 
				return !string.IsNullOrEmpty(json_data) ? 
					JsonConvert.DeserializeObject<T>(json_data) : 
					new T();
			}
		}

		public String ToJSON(JSON json)
		{
			// gjør det samme som for loopen som var her før.
			String ret = json.Tiles.Aggregate(
										"{\"tiles\": [", (current, t) 
										=> current 
										+ ("{\"id\":" + t.Id + ",\"isObstacle\":" 
										+ t.IsObstacle + "}"));
			ret += "]}";

			return ret;
		}

	}
}

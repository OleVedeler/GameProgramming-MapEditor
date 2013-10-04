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

        public void loadFromJson()
        {

        }

		public String ToJSON(JSON json)
		{
			// gjør det samme som for loopen som var her før.
			String ret = json.tiles.Aggregate(
										"{\"tiles\": [", (current, t) 
										=> current 
										+ ("{\"id\":" + t.id + ",\"isObstacle\":" 
										+ t.isObstacle + "}"));
			ret += "]}";

			return ret;
		}

	}
}

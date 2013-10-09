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
		public JsonHandler(){}

        public JSON Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<JSON>(json);
        }

		public String Serialize(JSON json)
		{
            return JsonConvert.SerializeObject(json);
		}
	}
}

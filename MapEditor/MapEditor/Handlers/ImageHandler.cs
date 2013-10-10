using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditor.Handlers
{
	class ImageHandler
	{
		private readonly AssetDatabaseHandler _assetDatabase;
		private readonly Image _image;

		
		public ImageHandler(
						Image image, 
						AssetDatabaseHandler assetDatabase)
		{
			_assetDatabase = assetDatabase;
			_image = image;
		}

		public void ShowcaseAsset(Asset asset)
		{
			_image.Source = _assetDatabase.DecodeImage(asset.Image.ToArray());
		}

	}
}

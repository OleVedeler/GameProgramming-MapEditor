using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MapEditor.Classes;

namespace MapEditor.Handlers
{
	class AssetDatabaseHandler
	{
		private readonly LinqToAssetDataContext _dataContext;
		private List<Asset> _assetList; 
		
		public AssetDatabaseHandler()
		{
			_dataContext = new LinqToAssetDataContext();
			GetFromDatabase();
		}

		public List<Asset> GetAllRows()
		{
			return _assetList;
		}

		/// <summary>
		/// Gets all rows from the database and stores it in a list
		/// </summary>
		private void GetFromDatabase()
		{
			_assetList = (
				from asset in _dataContext.Assets
				select asset
				).ToList();
		}

		/// <summary>
		/// gets a single row from the database
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private Asset GetRowFromDatabase(string name)
		{
			return (
				from asset in _dataContext.Assets
				where asset.Name == name 
				select asset
				).First();
		}

		/// <summary>
		/// gets a row from the assetlist
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Asset GetRowBy(String name)
		{
			for (int i = 0; i < _assetList.Count; i++)
			{
				if(_assetList[i].Name == name)
				return _assetList[i];	
			}
			return null;
		}

		public Asset GetRowBy(int id)
		{
			for (int i = 0; i < _assetList.Count; i++)
			{
				if (_assetList[i].Id == id)
					return _assetList[i];
			}
			return null;

		} 

		/// <summary>
		/// Deletes a row from the database
		/// </summary>
		/// <param name="name"></param>
		public void Delete(string name)
		{
			Asset row = GetRowBy(name);
			if(row == null) return;

			_dataContext.Assets.DeleteOnSubmit(row);
			_dataContext.SubmitChanges();
		}

		public bool Contains(string name)
		{
			return _assetList.Any(t => t.Name.Trim(' ') == name.Trim(' '));
		}


		/// <summary>
		/// Adds a new asset to the database 
		/// then adds the single row into the assetlist
		/// </summary>
		/// <param name="i"></param>
		public void Add(ImportObject i)
		{
			if (Contains(i.name)) return;
			Asset newAsset = new Asset();
			BitmapImage bitmapImage = new BitmapImage(new Uri(i.filename));

			newAsset.Name = i.name;
			newAsset.Parent = i.parent;
			newAsset.Image = EncodeImage(bitmapImage);
			
			_dataContext.Assets.InsertOnSubmit(newAsset);
			_dataContext.SubmitChanges();
			_assetList.Add(GetRowFromDatabase(newAsset.Name));
			
		}

		/// <summary>
		/// converts the image into a byte array that the database can read
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public Byte[] EncodeImage(BitmapImage image)
		{
			MemoryStream memStream = new MemoryStream();
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			encoder.Save(memStream);
			byte[] bytestream = memStream.GetBuffer();
			return bytestream;
		}

		/// <summary>
		/// Converts the database byte array back into a image
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public BitmapImage DecodeImage(byte[] value)
		{
			//TODO: Må fikses med exceptions
			if (value == null) return new BitmapImage();
			byte[] byteme = value;

			try
			{
				MemoryStream memStream = new MemoryStream(byteme);
				BitmapImage myBitmapImage = new BitmapImage();
				myBitmapImage.BeginInit();
				myBitmapImage.StreamSource = memStream;
				myBitmapImage.DecodePixelWidth = 374;
				myBitmapImage.DecodePixelHeight = 374;
				myBitmapImage.EndInit();
				return myBitmapImage;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			
			return new BitmapImage();
		}
	}
}

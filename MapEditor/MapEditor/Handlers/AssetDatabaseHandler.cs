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

		private void GetFromDatabase()
		{
			_assetList = (
				from asset in _dataContext.Assets
				select asset
				).ToList();
		}

		private Asset GetRowFromDatabase(string name)
		{
			return (
				from asset in _dataContext.Assets
				where asset.Name == name 
				select asset
				).First();
		}

		public Asset GetRowBy(String name)
		{


			for (int i = 0; i < _assetList.Count; i++)
			{
				if(_assetList[i].Name == name)
				return _assetList[i];	
			}
			
			/*
			using(var enumerator = _assetList.GetEnumerator())
				while (enumerator.MoveNext())
				{
					if(enumerator.Current.Name == name)
						return enumerator.Current;
				}*/
			return null;
		}

		public Asset GetRowBy(int id)
		{

			for (int i = 0; i < _assetList.Count; i++)
			{
				if (_assetList[i].Id == id)
					return _assetList[i];
			}

			/*
			using (var enumerator = _assetList.GetEnumerator())
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == id)
						return enumerator.Current;
				}
			 * 
			 * */
			return null;

		} 

		public void DeleteAll()
		{
			_dataContext.Assets.DeleteAllOnSubmit(GetAllRows());
			_dataContext.SubmitChanges();
		}
		
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

		public void Add(ImportObject i)
		{
			if (Contains(i.name)) return;
			Asset newAsset = new Asset();
			BitmapImage bitmapImage = new BitmapImage(new Uri(i.filename));

			newAsset.Name = i.name;
			newAsset.Parent = i.parent;
			newAsset.Image = EncodeImage(bitmapImage);


			
			_dataContext.Assets.InsertOnSubmit(newAsset);
			
			//todo: håndtere navn duplikater
			// legger inn 2 assets i databasen for en 
			// eller annen grunn når du importer mer en en hver gang
			_dataContext.SubmitChanges();

			// Trengs for å få med indexen
			_assetList.Add(GetRowFromDatabase(newAsset.Name));
			
		}

		public Byte[] EncodeImage(BitmapImage image)
		{
			MemoryStream memStream = new MemoryStream();
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			encoder.Save(memStream);
			byte[] bytestream = memStream.GetBuffer();
			return bytestream;
		}

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
				// TODO: Skriv errormelding til egen fil, slik at det kan evalueres!
				Console.WriteLine(e.ToString());
			}
			
			return new BitmapImage();
		}
	}
}

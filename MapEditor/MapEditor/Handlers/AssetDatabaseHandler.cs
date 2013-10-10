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

		public AssetDatabaseHandler()
		{
			_dataContext = new LinqToAssetDataContext();
		}

		public IQueryable<Asset> GetAllRows()
		{
			var assets =(
				from asset in _dataContext.Assets
				select asset
			);

			return assets;
		}

		public Asset GetRowBy(String name)
		{
			var assetData = (
			from asset in _dataContext.Assets
			where asset.Name == name
			select asset);

			return assetData.AsEnumerable().Any() ? assetData.First() : null;
		}

		public Asset GetRowBy(int id)
		{
			var assetData = (
			from asset in _dataContext.Assets
			where asset.Id == id
			select asset);

			return assetData.AsEnumerable().Any() ? assetData.First() : null;
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

		public void Add(ImportObject i)
		{
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

			return assetData.First();
		}

		public Asset GetRowBy(int id)
		{
			var assetData = (
			from asset in _dataContext.Assets
			where asset.Id == id
			select asset);

			return assetData.First();
		} 

		public void DeleteAll()
		{
			_dataContext.Assets.DeleteAllOnSubmit(GetAllRows());
			_dataContext.SubmitChanges();
		}

		public void Add(string name, string parent, string fileName)
		{
			Asset newAsset = new Asset();
			BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));

			newAsset.Name = name;
			newAsset.Parent = parent;

			newAsset.Image = EncodeImage(bitmapImage);

			_dataContext.Assets.InsertOnSubmit(newAsset);
			_dataContext.SubmitChanges();
		}

		public Byte[] EncodeImage(BitmapImage bi)
		{
			MemoryStream memStream = new MemoryStream();
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(bi));
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
				MemoryStream strmImg = new MemoryStream(byteme);
				BitmapImage myBitmapImage = new BitmapImage();
				myBitmapImage.BeginInit();
				myBitmapImage.StreamSource = strmImg;
				myBitmapImage.DecodePixelWidth = 374;
				myBitmapImage.DecodePixelHeight = 500;
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

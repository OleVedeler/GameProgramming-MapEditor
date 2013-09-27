using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int pixelWidth = 25;
		private const int pixelHeight = 25;

		public MainWindow()
		{
			InitializeComponent();
			EditorGrid.Background = Brushes.Black;
			for (int coloum = 0; coloum < EditorGrid.Width / pixelWidth; coloum++)
			{
				for (int row = 0; row < EditorGrid.Height / pixelHeight; row++)
				{
					Grid temp = new Grid();
					temp.Height = pixelHeight;
					temp.Width = pixelWidth;
					temp.HorizontalAlignment = HorizontalAlignment.Left;
					temp.VerticalAlignment = VerticalAlignment.Top;
					
					temp.Background = Brushes.Black;
					temp.Margin = new Thickness(pixelHeight * coloum, pixelWidth * row, 0, 0);

					temp.MouseEnter += new MouseEventHandler(DrawToGrid);
					temp.MouseLeftButtonDown += new MouseButtonEventHandler(DrawToGrid);
					
					EditorGrid.Children.Add(temp);
				}
			}

			/*LinqToAssetDataContext assetDataContext = new LinqToAssetDataContext();

			var assetShitt = 
				( 
				from asset in assetDataContext.Assets
				select asset
				);

			assetDataContext.Assets.DeleteAllOnSubmit(assetShitt);
			assetDataContext.SubmitChanges();
			*/


			UpdateTreeView();
		}


		private void DrawToGrid(Object o, MouseEventArgs e)
		{
			//tester om noe har blitt valgt
			if ((ComponentsTreeView.SelectedItem) == null) return;
			string selectedName = ((TreeViewItem) ComponentsTreeView.SelectedItem).Header.ToString();
			
			//Finner Bilde som tilhører  navnet
			LinqToAssetDataContext assetsDataContext = new LinqToAssetDataContext();
			var assetData = (
			from asset in assetsDataContext.Assets
			where asset.Name == selectedName
			select asset);
			
			// Tegner på sub griddene
			Grid temp = (Grid) o;
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				temp.Background = new ImageBrush(DecodePhoto(assetData.First().Image.ToArray()));
			}
		}



		/// <summary>
		/// Legger DatabaseObjekter inn til TreeViewet. 
		/// ikke Superheit kode, men har sittet 6 timer med dette og det funker.
		/// Kommer mest sannsynlig til skrive om koden igjen når jeg er mer edru og har tid.
		/// </summary>
		/// <param name="newAsset">Er en database rad</param>
			
		private void AddToTree(Asset newAsset)
		{
			TreeViewItem newItem = new TreeViewItem();
			newItem.Header = newAsset.Name;

			TreeViewItem parentItem = new TreeViewItem();
			parentItem.Header = newAsset.Parent;
			
			// For å kunne compare, Burde kunne gjøres bedre, men fant ingen enkel måte
			List<TreeViewItem> Testing = new List<TreeViewItem>();

			// Leger inn det første elementet hvis, den er tom. Burde kunne implementeres inni loopen
			if (ComponentsTreeView.Items.Count == 0)
			{
				ComponentsTreeView.Items.Add(parentItem);
			}
			// Legger treet inn i listen
			for (int i = 0; i < ComponentsTreeView.Items.Count; i++)
			{
				Testing.Add((TreeViewItem)ComponentsTreeView.Items[i]);
			}

			bool parentExists = false;
			// Tester om parenten finnes, hvis ikke setter vi et flag som sier fra at den ikke finnes
			for (int i = 0; i < ComponentsTreeView.Items.Count; i++)
			{
				if ((Testing[i].Header.Equals(parentItem.Header)))
				{
					((TreeViewItem)ComponentsTreeView.Items[i]).Items.Add(newItem);
					parentExists = true;
					break;
				}	
			}
			// setter parenten og barne hvis flaget er satt
			if (!parentExists)
			{
				ComponentsTreeView.Items.Add(parentItem);
				parentItem.Items.Add(newItem);
			}
		}

		private void AddToAssetDatabase(string name, string parent, string fileName)
		{
			LinqToAssetDataContext assetsDataContext = new LinqToAssetDataContext();
			Asset newAsset = new Asset();
			BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));
			
			newAsset.Name = name;
			newAsset.Parent = parent;
			
			newAsset.Image = convertBitmapImageToBytestream(bitmapImage);

			assetsDataContext.Assets.InsertOnSubmit(newAsset);
			assetsDataContext.SubmitChanges();
		}


		public static Byte[] convertBitmapImageToBytestream(BitmapImage bi)
		{
			MemoryStream memStream = new MemoryStream();
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(bi));
			encoder.Save(memStream);
			byte[] bytestream = memStream.GetBuffer();
			return bytestream;
		}

		public BitmapImage DecodePhoto(byte[] value)
		{
			//Må fikses for exceptions
			if (value == null) return new BitmapImage();

			byte[] byteme = value as byte[];
			if (byteme == null) return new BitmapImage();

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
			catch (Exception ex)
			{

			}
			return new BitmapImage();
		}

        private void MenuItem_new(object sender, RoutedEventArgs e)
        {
            var url = "{\"tiles\": [{\"id\":1, \"isObstacle\":true}]}";
        }
        private void MenuItem_save(object sender, RoutedEventArgs e)
        {
            int size = (int)((EditorGrid.Height / pixelHeight) * (EditorGrid.Width / pixelWidth));

            JSON jsonFile = new JSON(size);

            for (int i = 0; i < size; i++)
            {
                jsonFile.tiles[i].id = i;
                jsonFile.tiles[i].isObstacle = (0 == i % 2) ? true : false;
        }

            Console.WriteLine(toJSON(jsonFile));
        }
        private void MenuItem_load(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_import(object sender, RoutedEventArgs e)
        {

			AddToAssetDatabase("Bush", "Landskap", "C:\\ToolsProgrammering\\MapEditor\\MapEditor\\Bush.jpg");
			UpdateTreeView();

        }
        private void MenuItem_exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

		private void UpdateTreeView()
		{
			
			ComponentsTreeView.Items.Clear();
			LinqToAssetDataContext assetsDataContext = new LinqToAssetDataContext();

			var assetData = (
				from asset in assetsDataContext.Assets
				select asset);

			using (var enumerator = assetData.GetEnumerator())
				while (enumerator.MoveNext())
					AddToTree(enumerator.Current);
		}

        private static T _download_serialized_json_data<T>(string url) where T : new()
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
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }

        private String toJSON(JSON j)
        {
            String ret = "{\"tiles\": [";

            for (int i = 0; i < j.tiles.Length; i++)
            {
                ret += "{\"id\":" + j.tiles[i].id + ",\"isObstacle\":" + j.tiles[i].isObstacle + "}";
            }

            ret += "]}";

            return ret;
        }
	}
}

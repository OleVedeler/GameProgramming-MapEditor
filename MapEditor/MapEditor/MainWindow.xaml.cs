using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			for (int coloum = 0; coloum < 50; coloum++)
			{
				for (int row = 0; row < 50; row++)
				{
					Grid temp = new Grid();
					temp.Height = pixelHeight;
					temp.Width = pixelWidth;
					temp.HorizontalAlignment = HorizontalAlignment.Left;
					temp.VerticalAlignment = VerticalAlignment.Top;
					
					temp.Background = Brushes.Black;
					temp.Margin = new Thickness(pixelHeight * coloum, pixelWidth * row, 0, 0);

					temp.MouseEnter += new MouseEventHandler(ColourGrid);
					temp.MouseLeftButtonDown += new MouseButtonEventHandler(ColourGrid);
					
					EditorGrid.Children.Add(temp);
				}
			}
			AddToAssetDatabase("Gress","Landskap", @"C:\\ToolsProgramming\\MapEditor\\MapEditor\\grassTile.jpg");

			TreeViewItem colorItem = new TreeViewItem();
			colorItem.Header = "Color";
			ComponentsTreeView.Items.Add(colorItem);

			TreeViewItem redItem = new TreeViewItem();
			redItem.Header = "Red";
			colorItem.Items.Add(redItem);

			TreeViewItem blueItem = new TreeViewItem();
			blueItem.Header = "Blue";
			colorItem.Items.Add(blueItem);

			TreeViewItem greenItem = new TreeViewItem();
			greenItem.Header = "Green";
			colorItem.Items.Add(greenItem);
		}

		private void AddToTree(Asset newAsset)
		{
			TreeViewItem newItem = new TreeViewItem();
			newItem.Header = newAsset.Name;
			//for (int i = 0; i < UPPER; i++)
			{
				//ComponentsTreeView. .Items.Add(greenItem);	
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

		private void ColourGrid(Object o, MouseEventArgs e)
		{
			Grid temp = (Grid) o;


			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (((TreeViewItem) ComponentsTreeView.SelectedItem).Header == "Red")
					temp.Background = Brushes.Red;
				else if (((TreeViewItem) ComponentsTreeView.SelectedItem).Header == "Blue")
					temp.Background = Brushes.Blue;
				else if (((TreeViewItem) ComponentsTreeView.SelectedItem).Header == "Green")
					temp.Background = Brushes.Green;
			}
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

		public void DecodePhoto(byte[] value)
		{
			if (value == null) return;
			byte[] byteme = value as byte[];
			if (byteme == null) return;

			try
			{
				MemoryStream strmImg = new MemoryStream(byteme);
				BitmapImage myBitmapImage = new BitmapImage();
				myBitmapImage.BeginInit();
				myBitmapImage.StreamSource = strmImg;
				myBitmapImage.DecodePixelWidth = 374;
				myBitmapImage.DecodePixelHeight = 500;
				myBitmapImage.EndInit();
				//DebugImage.Source = myBitmapImage;
			}
			catch (Exception ex)
			{

			}

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

        }
        private void MenuItem_exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

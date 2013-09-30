using MapEditor.Handlers;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;


namespace MapEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly AssetDatabaseHandler _assetDatabaseHandler;
		private readonly TreeViewHandler _treeViewHandler;
		private readonly GameGridHandler _gameGridHandler;
		
		public MainWindow()
		{
			InitializeComponent();

			_assetDatabaseHandler = new AssetDatabaseHandler();
			_treeViewHandler = new TreeViewHandler(ComponentsTreeView, _assetDatabaseHandler);
			_gameGridHandler = new GameGridHandler(EditorGrid, _assetDatabaseHandler, _treeViewHandler);
		}

        private void MenuItem_new(object sender, RoutedEventArgs e)
        {
            var url = "{\"tiles\": [{\"id\":1, \"isObstacle\":true}]}";
        }
        private void MenuItem_save(object sender, RoutedEventArgs e)
        {
            int size = _gameGridHandler.Size();

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
			//_assetDatabaseHandler.Add("Busk", "Landskap", @"C:\ToolsProgrammering\MapEditor\MapEditor\Images\Bush.jpg");
			//_treeViewHandler.Update();

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

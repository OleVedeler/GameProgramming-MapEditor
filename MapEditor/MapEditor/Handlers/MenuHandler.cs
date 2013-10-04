using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Handlers
{
	class MenuHandler
	{

		private readonly GameGridHandler _gameGridHandler;
		private readonly JsonHandler _jsonHandler;
		private readonly Menu _mainMenu;

		public MenuHandler(Menu mainMenu ,GameGridHandler gameGridHandler)
		{
			_gameGridHandler = gameGridHandler;
			_jsonHandler = new JsonHandler();
			_mainMenu = mainMenu;
			Init();

		}

		private void Init()
		{

			MenuItem file = new MenuItem {Header = "_File", Name = "File"};
			_mainMenu.Items.Add(file);

			MenuItem newItem = new MenuItem {Header = "_New", Name = "New"};
			newItem.Click += MenuItem_new;
			file.Items.Add(newItem);

			MenuItem loadItem = new MenuItem {Header = "_Load", Name = "Load"};
			loadItem.Click += MenuItem_load;
			file.Items.Add(loadItem);

			MenuItem saveItem = new MenuItem { Header = "_Save", Name = "Save" };
			saveItem.Click += MenuItem_save;
			file.Items.Add(saveItem);

			MenuItem importItem = new MenuItem { Header = "_Import", Name = "import" };
			importItem.Click += MenuItem_import;
			file.Items.Add(importItem);

			MenuItem exitItem = new MenuItem { Header = "_Exit", Name = "exit" };
			exitItem.Click += MenuItem_exit;
			file.Items.Add(exitItem);
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
				jsonFile.Tiles[i].Id = i;
				jsonFile.Tiles[i].IsObstacle = (0 == i % 2) ? true : false;
			}

			Console.WriteLine(_jsonHandler.ToJSON(jsonFile));
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

	}
}

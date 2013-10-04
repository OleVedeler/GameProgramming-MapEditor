using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MapEditor.Classes;

namespace MapEditor.Handlers
{
    class MenuHandler
    {

        private readonly GameGridHandler _gameGridHandler;
        private readonly Menu _mainMenu;
        private readonly Grid _inputBox;
        private ImportObject imp;

        public MenuHandler(Menu mainMenu, GameGridHandler gameGridHandler, Grid inputBox)
        {
            _gameGridHandler = gameGridHandler;
            _mainMenu = mainMenu;
            _inputBox = inputBox;
            Init();

        }

        private void Init()
        {

            MenuItem file = new MenuItem { Header = "_File", Name = "File" };
            _mainMenu.Items.Add(file);

            MenuItem newItem = new MenuItem { Header = "_New", Name = "New" };
            newItem.Click += MenuItem_new;
            file.Items.Add(newItem);

            MenuItem loadItem = new MenuItem { Header = "_Load", Name = "Load" };
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
            //var url = "{\"tiles\": [{\"id\":1, \"isObstacle\":true}]}";
        }
        private void MenuItem_save(object sender, RoutedEventArgs e)
        {
            /*
			int size = _gameGridHandler.Size();

			JSON jsonFile = new JSON();

			for (int i = 0; i < size; i++)
			{
                Tile t = new Tile();
                t.id = i;
                t.isObstacle = (i % 2);

                jsonFile.tiles.Add(t);
			}

			Console.WriteLine(_jsonHandler.ToJSON(jsonFile));
             */
        }
        private void MenuItem_load(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_import(object sender, RoutedEventArgs e)
		{
            imp = new ImportObject();
            OpenFileDialog file = new OpenFileDialog();

            file.DefaultExt = ".jpg";
            file.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            Nullable<bool> result = file.ShowDialog();

            if (result == true)
            {
                string filename = file.FileName;
                imp.filename = filename;

                _inputBox.Visibility = Visibility.Visible;

                ((Button)_inputBox.FindName("YesButton")).Click += YesClick;
                ((Button)_inputBox.FindName("NoButton")).Click += NoClick;
            }
            else
            {
                //ERROR
            }
		}
        private void MenuItem_exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            imp.parent = ((TextBox)_inputBox.FindName("InputParent")).Text;
            imp.name = ((TextBox)_inputBox.FindName("InputName")).Text;

            Console.Write("Name: " + imp.name + " | Parent: " + imp.parent + " | Filename: " + imp.filename);

            _inputBox.Visibility = Visibility.Collapsed;

            _gameGridHandler.import(imp);
        }

        private void NoClick(object sender, RoutedEventArgs e)
        {
            _inputBox.Visibility = Visibility.Collapsed;
        }
    }
}

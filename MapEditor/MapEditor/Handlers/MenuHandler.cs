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
        private ImportObject _imp;
        private bool _showCollisions;

        public MenuHandler(Menu mainMenu, GameGridHandler gameGridHandler, Grid inputBox)
        {
            _gameGridHandler = gameGridHandler;
            _mainMenu = mainMenu;
            _inputBox = inputBox;
            _showCollisions = false;
            Init();
        }

        private void Init()
        {
            Separator s = new Separator();
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

            file.Items.Add(s);

            MenuItem importItem = new MenuItem { Header = "_Import", Name = "import" };
            importItem.Click += MenuItem_import;
            file.Items.Add(importItem);

            s = new Separator();
            file.Items.Add(s);

            MenuItem exitItem = new MenuItem { Header = "_Exit", Name = "exit" };
            exitItem.Click += MenuItem_exit;
            file.Items.Add(exitItem);

            MenuItem view = new MenuItem { Header = "_View", Name = "View" };
            _mainMenu.Items.Add(view);
            
            MenuItem obstacles = new MenuItem { Header = "_Show Collisionmap", Name = "ShowCollisionmap", IsCheckable = true };
            obstacles.Click += MenuItem_showCollisionmap;
            view.Items.Add(obstacles);
        }
		/// eventhandlers for the menues, delegates the work to other classes
        private void MenuItem_new(object sender, RoutedEventArgs e)
        {
            _gameGridHandler.NewMap();
        }
        private void MenuItem_save(object sender, RoutedEventArgs e)
        {
            _gameGridHandler.Save();
        }
        private void MenuItem_load(object sender, RoutedEventArgs e)
        {
            _gameGridHandler.Load();
        }
        /// <summary>
        /// Asks for a filepath and sets two events that waits for an answer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void MenuItem_import(object sender, RoutedEventArgs e)
		{
            _imp = new ImportObject();
            OpenFileDialog file = new OpenFileDialog
            {
	            DefaultExt = ".jpg",
	            Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png"
            };

	        bool? result = file.ShowDialog();

            if (result == true)
            {
                string filename = file.FileName;
                _imp.filename = filename;

                _inputBox.Visibility = Visibility.Visible;

                ((Button)_inputBox.FindName("YesButton")).Click += YesClick;
                ((Button)_inputBox.FindName("NoButton")).Click += NoClick;
            }
		}
        private void MenuItem_exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

		/// <summary>
		/// gets the name and parent of the new asset. then it closes the dialog 
		/// and delegates the work
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void YesClick(object sender, RoutedEventArgs e)
        {
		    _imp.parent = ((TextBox) _inputBox.FindName("InputParent")).Text;
		    _imp.name = ((TextBox) _inputBox.FindName("InputName")).Text;


			if (_imp.name == "" || _imp.parent == "") return;
            _inputBox.Visibility = Visibility.Collapsed;
 
			_gameGridHandler.Import(_imp);

            ((TextBox)_inputBox.FindName("InputParent")).Text = "";
            ((TextBox)_inputBox.FindName("InputName")).Text = "";
        }
		
		/// <summary>
		/// Closes the dialog
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void NoClick(object sender, RoutedEventArgs e)
        {
            _inputBox.Visibility = Visibility.Collapsed;
        }
		/// <summary>
		/// Turns on and off the collisionmap
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void MenuItem_showCollisionmap(object sender, RoutedEventArgs e)
        {
            _showCollisions = !_showCollisions;
            _gameGridHandler.ShowCollisionmap(_showCollisions);
        }
    }
}

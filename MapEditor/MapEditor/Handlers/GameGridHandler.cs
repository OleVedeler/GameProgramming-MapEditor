using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MapEditor.Classes;
using Microsoft.Win32;

namespace MapEditor.Handlers
{
	class GameGridHandler
	{
		private const int PixelWidth = 25;
		private const int PixelHeight = 25;
        private JSON jsonFile;
        private readonly Grid _editorGrid;
		private readonly AssetDatabaseHandler _assetDatabaseHandler;
		private readonly TreeViewHandler _treeViewHandler;
        private readonly JsonHandler _jsonHandler;

		public GameGridHandler(
							Grid editorGrid, 
							AssetDatabaseHandler assetDatabaseHandler, 
							TreeViewHandler treeViewHandler)
		{
			_editorGrid = editorGrid;
			_assetDatabaseHandler = assetDatabaseHandler;
			_treeViewHandler = treeViewHandler;
            _jsonHandler = new JsonHandler();
            jsonFile = new JSON();
			InitGameGrid();
		}

		public int Size()
		{
			return Width() * Height();
		}

		public int Width()
		{
			return (int)(_editorGrid.Width/PixelWidth);
		}

		public int Height()
		{
			return (int) (_editorGrid.Height/PixelHeight);
		}

        public void import(ImportObject imp)
        {
            _assetDatabaseHandler.Add(imp);
            _treeViewHandler.Update();
        }

        public void save() 
        {
            string json = _jsonHandler.ToJSON(jsonFile);

            SaveFileDialog save = new SaveFileDialog();

            save.FileName = "Map"; // Default file name
            save.DefaultExt = ".json"; // Default file extension
            save.Filter = "JSON files (.json)|*.json"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = save.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = save.FileName;
            }
        }

		private void InitGameGrid()
		{
			for (int coloum = 0; coloum < Width(); coloum++)
			{
				for (int row = 0; row < Height(); row++)
				{
					Grid childGrid = new Grid
					{
						Height = PixelHeight,
						Width = PixelWidth,
						HorizontalAlignment = HorizontalAlignment.Left,
						VerticalAlignment = VerticalAlignment.Top,
						Background = Brushes.Black,
						Margin = new Thickness(PixelHeight*coloum, PixelWidth*row, 0, 0)
					};

					childGrid.MouseEnter += DrawToGridEvent;
					childGrid.MouseLeftButtonDown += DrawToGridEvent;

					_editorGrid.Children.Add(childGrid);
                    jsonFile.tiles.Add(new Tile());
				}
			}
		}

		private void DrawToGridEvent(Object o, MouseEventArgs e)
		{
			//tester om noe har blitt valgt
			if ((_treeViewHandler.SelectedItem()) == null) return;
			string selectedName = ((TreeViewItem)_treeViewHandler.SelectedItem()).Header.ToString();

			//Finner Bilde som tilhører  navnet
			var assetData = _assetDatabaseHandler.GetRowBy(selectedName);

			// Tester om valgt element finnes i databasen
			if(assetData == null) return;

			// Tegner på sub griddene
			Grid currentGrid = (Grid)o;
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				currentGrid.Background = new ImageBrush(_assetDatabaseHandler.DecodeImage(assetData.Image.ToArray()));
                addToJsonList(currentGrid, assetData);
			}
		}

        private void addToJsonList(Grid g, Asset assetData)
        {
            //id
            jsonFile.tiles.ElementAt(_editorGrid.Children.IndexOf(g)).id = assetData.Id;

            //isObstacle
            //Check if chekcbox is checked
        }
	}
}

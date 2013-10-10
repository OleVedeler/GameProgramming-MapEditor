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
using System.IO;

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
            string json = _jsonHandler.Serialize(jsonFile);

            SaveFileDialog save = new SaveFileDialog();

            save.FileName = "Map"; // Default file name
            save.DefaultExt = ".json"; // Default file extension
            save.Filter = "JSON files (.json)|*.json"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = save.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    // Save document
                    string filename = save.FileName;
                    File.WriteAllText(filename, json);
                    Console.WriteLine(json);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: Could not read to file. Original error: " + ex.Message);
                }
            }
        }

        public void load()
        {
            Stream stream = null;
            OpenFileDialog open = new OpenFileDialog();
            string jsonStr = "";

            open.Filter = "JSON files (.json)|*.json";
            open.FilterIndex = 2;

            Nullable<bool> result = open.ShowDialog();

            if (result == true)
            {
                try
                {
                    if ((stream = open.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            StreamReader reader = new StreamReader(stream);
                            jsonStr = reader.ReadToEnd();
                            reader.Close();
                        }
                    }

                    newMap();

                    jsonFile = _jsonHandler.Deserialize(jsonStr);
                    DrawToGridEvent();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public void newMap()
        {
            InitGameGrid();
        }

		private void InitGameGrid()
		{
            jsonFile.tiles.Clear();
            _editorGrid.Children.Clear();

			for (int column = 0; column < Height(); column++)
			{
				for (int row = 0; row < Width(); row++)
				{
					Grid childGrid = new Grid
					{
						Height = PixelHeight,
						Width = PixelWidth,
						HorizontalAlignment = HorizontalAlignment.Left,
						VerticalAlignment = VerticalAlignment.Top,
						Background = Brushes.Black,
						Margin = new Thickness(PixelHeight*row, PixelWidth*column, 0, 0)
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

        //Draw from json
        private void DrawToGridEvent()
        {
            var assetData = _assetDatabaseHandler.GetAllRows();
            
            for(int i = 0; i < jsonFile.tiles.Count; i++) {
                if (assetData == null) return;

                Grid currentGrid = (Grid)_editorGrid.Children[i];

                using (var enumerator = assetData.GetEnumerator())
				while (enumerator.MoveNext()) {
                    if(jsonFile.tiles[i].id == enumerator.Current.Id) {
                        currentGrid.Background = new ImageBrush(_assetDatabaseHandler.DecodeImage(enumerator.Current.Image.ToArray()));
                    }
                }
            }
        }

        private void addToJsonList(Grid g, Asset assetData)
        {
            //id
            jsonFile.tiles[_editorGrid.Children.IndexOf(g)].id = assetData.Id;

            //isObstacle
            //Check if chekcbox is checked
        }

    }
}

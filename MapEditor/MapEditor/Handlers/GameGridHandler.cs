using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MapEditor.Handlers
{
	class GameGridHandler
	{

		private const int PixelWidth = 25;
		private const int PixelHeight = 25;
		private readonly Grid _editorGrid;
		private readonly AssetDatabaseHandler _assetDatabaseHandler;
		private readonly TreeViewHandler _treeViewHandler;

		public GameGridHandler(
							Grid editorGrid, 
							AssetDatabaseHandler assetDatabaseHandler, 
							TreeViewHandler treeViewHandler)
		{
			_editorGrid = editorGrid;
			_assetDatabaseHandler = assetDatabaseHandler;
			_treeViewHandler = treeViewHandler;
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
			}
		}
	}
}

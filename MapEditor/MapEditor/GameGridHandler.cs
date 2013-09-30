using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MapEditor
{
	class GameGridHandler
	{
		private readonly Grid _editorGrid;
		private const int PixelWidth = 25;
		private const int PixelHeight = 25;
		private readonly AssetDatabase _assetDatabase;
		private readonly TreeViewHandler _treeViewHandler;

		public GameGridHandler(Grid editorGrid, AssetDatabase assetDatabase, TreeViewHandler treeViewHandler)
		{
			_editorGrid = editorGrid;
			_assetDatabase = assetDatabase;
			_treeViewHandler = treeViewHandler;
			InitGameGrid();
		}

		public int Size()
		{
			return (int)((_editorGrid.Height / PixelHeight) * (_editorGrid.Width / PixelWidth));
		}

		private void InitGameGrid()
		{
			_editorGrid.Background = Brushes.Black;
			for (int coloum = 0; coloum < _editorGrid.Width / PixelWidth; coloum++)
			{
				for (int row = 0; row < _editorGrid.Height / PixelHeight; row++)
				{
					Grid temp = new Grid();
					temp.Height = PixelHeight;
					temp.Width = PixelWidth;
					temp.HorizontalAlignment = HorizontalAlignment.Left;
					temp.VerticalAlignment = VerticalAlignment.Top;

					temp.Background = Brushes.Black;
					temp.Margin = new Thickness(PixelHeight * coloum, PixelWidth * row, 0, 0);

					temp.MouseEnter += new MouseEventHandler(DrawToGridEvent);
					temp.MouseLeftButtonDown += new MouseButtonEventHandler(DrawToGridEvent);

					_editorGrid.Children.Add(temp);
				}
			}
		}

		private void DrawToGridEvent(Object o, MouseEventArgs e)
		{
			//tester om noe har blitt valgt
			if ((_treeViewHandler.TreeView.SelectedItem) == null) return;
			string selectedName = ((TreeViewItem)_treeViewHandler.TreeView.SelectedItem).Header.ToString();

			//Finner Bilde som tilhører  navnet
			var assetData = _assetDatabase.GetRowsBy(selectedName);

			// Tegner på sub griddene
			Grid currentGrid = (Grid)o;
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				currentGrid.Background = new ImageBrush(_assetDatabase.DecodeImage(assetData.First().Image.ToArray()));
			}
		}
	}
}

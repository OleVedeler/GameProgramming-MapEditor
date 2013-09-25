using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	}
}

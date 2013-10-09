using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor
{
	public class TreeItem : TreeViewItem
	{
		public event RoutedEventHandler OnClickEvent;


		private MenuItem deleteItem;
		public TreeItem(string header)
		{
			base.Header = header;


			deleteItem = new MenuItem { Header = "_Delete" };
			deleteItem.Click += DeleteItemOnClick;

			base.ContextMenu = new ContextMenu();
			base.ContextMenu.Items.Add(deleteItem);

		}

		private void DeleteItemOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			MessageBoxResult result = 
				MessageBox.Show("Do you want to delete this asset?",
						"Confirmation", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				OnClickEvent(this, routedEventArgs);
			}
		}
	}
}

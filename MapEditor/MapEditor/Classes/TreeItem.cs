using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MapEditor.Handlers;

namespace MapEditor
{
	public class TreeItem : TreeViewItem
	{
		public event RoutedEventHandler OnClickEvent;

		public PropertyHandler PropertyHandler { get; set; }

		public TreeItem(string header)
		{
			base.Header = header;
			PropertyHandler = new PropertyHandler(new ListBox(), header);

			MenuItem obsticelItem = new MenuItem { Header = "_Set as obstacle", IsCheckable = true };
			obsticelItem.Click += obstacleItem_Click;

			MenuItem deleteItem = new MenuItem { Header = "_Delete" };
			deleteItem.Click += DeleteItemOnClick;

			base.ContextMenu = new ContextMenu();
			base.ContextMenu.Items.Add(obsticelItem);
			base.ContextMenu.Items.Add(deleteItem);
		}

		/// <summary>
		/// sends back an event that can be handled in other classes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="routedEventArgs"></param>
		void obstacleItem_Click(object sender, RoutedEventArgs routedEventArgs)
		{
			OnClickEvent(sender, routedEventArgs);
		}


		/// <summary>
		/// sends back an event that can be handled in other classes
		/// Askes if you realy want to delete the item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="routedEventArgs"></param>
		private void DeleteItemOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			MessageBoxResult result = 
				MessageBox.Show("Do you want to delete this asset?",
						"Confirmation", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes)
			{
				OnClickEvent(sender, routedEventArgs);
			}
		}
	}
}

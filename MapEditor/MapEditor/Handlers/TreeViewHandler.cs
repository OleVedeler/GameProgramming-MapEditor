using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MapEditor.Handlers
{
	class TreeViewHandler
	{
		private readonly TreeView _treeView;
		private readonly AssetDatabaseHandler _assetDatabaseHandler;
		private readonly ImageHandler _imageHandler;
        private readonly PropertyHandler _propertyHandler;

		public TreeViewHandler(
						TreeView treeView, 
						AssetDatabaseHandler assetDatabaseHandler,
						ImageHandler imageHandler,
                        PropertyHandler propertyHandler)
		{
			_treeView = treeView;
			_assetDatabaseHandler = assetDatabaseHandler;
			_imageHandler = imageHandler;
            _propertyHandler = propertyHandler;
			Init();
			// Eventhandlers
			_propertyHandler.ObsticalCheckBox.Checked += ObsticalCheckBox_Checked;
			_propertyHandler.ObsticalCheckBox.Unchecked += ObsticalCheckBox_Checked;
			_treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;

		}


		/// <summary>
		/// Activates when selectedItem has changed
		/// removes the property
		/// if the new selected item is in the database it will be shown in the preview 
		/// the propertyscreen will be activated
		/// </summary>
		private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
		{
			_propertyHandler.SetVisability(false);
            if (SelectedItem() == null) return;
			Asset tempAsset = _assetDatabaseHandler.GetRowBy(((TreeViewItem) SelectedItem()).Header.ToString());
			if (tempAsset == null) return;
			
			_imageHandler.ShowcaseAsset(tempAsset);
	
			_propertyHandler.ObsticalCheckBox.IsChecked = ((SelectedItem() as TreeItem).ContextMenu.Items[0] as MenuItem).IsChecked;
			(_propertyHandler.PropertyBox.Items[0] as ListBoxItem).Content = ((SelectedItem() as TreeItem).PropertyHandler.Name);

			_propertyHandler.SetVisability(true);
		}

		/// <summary>
		/// Gives the selected item of the treeview
		/// </summary>
		/// <returns></returns>
		public object SelectedItem()
		{
			return _treeView.SelectedItem;
		}

		/// <summary>
		///  Adds the element to the treeView
		///  If the parent does not exist, it creates it.
		/// </summary>
		/// <param name="newAsset"></param>
		public void Add(Asset newAsset)
		{
			TreeItem newItem = new TreeItem (newAsset.Name);
			newItem.OnClickEvent += TreeItemOnClickEvent;
			TreeViewItem parentItem = new TreeViewItem {Header = newAsset.Parent};

			if (_treeView.Items.Count != 0)
			{
				List<TreeViewItem> treeViewList = _treeView.Items.Cast<TreeViewItem>().ToList();

				for (int i = 0; i < _treeView.Items.Count; i++)
				{
					if ((((string) treeViewList[i].Header).TrimEnd(' ') != (parentItem.Header.ToString().TrimEnd(' '))))
						continue;

					((TreeViewItem) _treeView.Items[i]).Items.Add(newItem);
					return;
				}
			}
			_treeView.Items.Add(parentItem);
			parentItem.Items.Add(newItem);
		}

		/// <summary>
		/// runs when you choose an option from right clicking on a element in the treeView
		/// then find what was choosen and excecutes that code
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="routedEventArgs"></param>
		private void TreeItemOnClickEvent(object sender, RoutedEventArgs routedEventArgs)
		{
			if (((MenuItem)sender).Header.ToString() == "_Delete")
			{
				RemoveAsset();
			}
			if (((MenuItem)sender).Header.ToString() == "_Set as obsticel")
			{
				// sets the checkbox on/off
				_propertyHandler.ObsticalCheckBox.IsChecked = ((SelectedItem() as TreeItem).ContextMenu.Items[0] as MenuItem).IsChecked;
			}
		}

		/// <summary>
		/// Runs if the checkbox is (un)checked
		/// sets the check on the contextmenu when you right click an element
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ObsticalCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			((SelectedItem() as TreeItem).ContextMenu.Items[0] as MenuItem).IsChecked = (bool)_propertyHandler.ObsticalCheckBox.IsChecked;
		}

		/// <summary>
		/// removes the currently selected item from the database and the treeView
		/// also removes parents if they are empty
		/// </summary>
		private void RemoveAsset()
		{
			_assetDatabaseHandler.Delete(((TreeViewItem)SelectedItem()).Header.ToString());
			TreeViewItem tempItem = new TreeViewItem();
			foreach (TreeViewItem treeView in _treeView.Items)
			{
				treeView.Items.Remove(SelectedItem());
				if (!treeView.HasItems)
				{
					tempItem = treeView;
				}
			}
			// Removes parent that are empty
			_treeView.Items.Remove(tempItem);	
		}


		/// <summary>
		/// adds all the assets in the database to the TreeView
		/// </summary>
		private void Init()
		{
			var assetData = _assetDatabaseHandler.GetAllRows();
			foreach (var asset in assetData)
			{
				Add(asset);
			}			
		}

		/// <summary>
		/// removes everything from the treeView and builds it up again
		/// </summary>
		public void Update()
		{
			_treeView.Items.Clear();
			Init();
		}
	}
}

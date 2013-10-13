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

			_propertyHandler.ObsticalCheckBox.Checked += ObsticalCheckBox_Checked;
			_propertyHandler.ObsticalCheckBox.Unchecked += ObsticalCheckBox_Checked;
			_treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;

		}

		private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
		{
			_propertyHandler.SetVisability(false);
            if (SelectedItem() == null) return;
			Asset tempAsset = _assetDatabaseHandler.GetRowBy(((TreeViewItem) SelectedItem()).Header.ToString());
			if (tempAsset == null) return;
			_imageHandler.ShowcaseAsset(tempAsset);
	
			// Setter checkboxen på elementet og i property menyen til det samme.
			_propertyHandler.ObsticalCheckBox.IsChecked = ((SelectedItem() as TreeItem).ContextMenu.Items[0] as MenuItem).IsChecked;
			(_propertyHandler.PropertyBox.Items[0] as ListBoxItem).Content = ((SelectedItem() as TreeItem).PropertyHandler.Name);

			_propertyHandler.SetVisability(true);
		}

		public object SelectedItem()
		{
			return _treeView.SelectedItem;
		}


		/// <summary>
		/// Legger DatabaseObjekter inn til TreeViewet. 
		/// ikke Superheit kode, men har sittet 6 timer med dette og det funker.
		/// Kommer mest sannsynlig til skrive om koden igjen når jeg er mer edru og har tid.
		/// </summary>
		/// <param name="newAsset">Er en database rad</param>
		public void Add(Asset newAsset)
		{
			TreeItem newItem = new TreeItem (newAsset.Name);
			newItem.OnClickEvent += TreeItemOnClickEvent;
			TreeViewItem parentItem = new TreeViewItem {Header = newAsset.Parent};

			if (_treeView.Items.Count != 0)
			{
				// Legger treet inn i listen
				List<TreeViewItem> treeViewList = _treeView.Items.Cast<TreeViewItem>().ToList();

				// returnerer hvis den har lagt til elementet
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

		private void TreeItemOnClickEvent(object sender, RoutedEventArgs routedEventArgs)
		{
			if (((MenuItem)sender).Header.ToString() == "_Delete")
			{
				RemoveAsset(sender);
			}
			if (((MenuItem)sender).Header.ToString() == "_Set as obsticel")
			{
				_propertyHandler.ObsticalCheckBox.IsChecked = ((SelectedItem() as TreeItem).ContextMenu.Items[0] as MenuItem).IsChecked;
			}
		}

		private void ObsticalCheckBox_Checked(object sender, RoutedEventArgs e)
			{
			((SelectedItem() as TreeItem).ContextMenu.Items[0] as MenuItem).IsChecked = (bool)_propertyHandler.ObsticalCheckBox.IsChecked;
			}


		private void RemoveAsset(object sender)
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

		private void Init()
		{
			var assetData = _assetDatabaseHandler.GetAllRows();
			foreach (var asset in assetData)
			{
				Add(asset);
			}			
		}

		public void Update()
		{
			_treeView.Items.Clear();
			Init();
		}
	}
}

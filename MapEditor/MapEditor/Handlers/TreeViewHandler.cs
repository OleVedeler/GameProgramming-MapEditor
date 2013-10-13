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
		public readonly TreeView _treeView;
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

			_treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
		}

		private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
		{
            if (SelectedItem() == null) return;
			Asset tempAsset = _assetDatabaseHandler.GetRowBy(((TreeViewItem) SelectedItem()).Header.ToString());
			if (tempAsset == null) return;
			_imageHandler.ShowcaseAsset(tempAsset);
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
			TreeItem parentItem = new TreeItem(newAsset.Parent);
			parentItem.OnClickEvent += TreeItemOnClickEvent;

			if (_treeView.Items.Count != 0)
			{
				//_treeView.Items.Add(parentItem);
				// Legger treet inn i listen
				List<TreeViewItem> treeViewList = _treeView.Items.Cast<TreeViewItem>().ToList();

				//newItem.MouseRightButtonUp += RightButtonClick;
				// returnerer hvis den har lagt til elementet
				for (int i = 0; i < _treeView.Items.Count; i++)
				{
					// Todo: Fix Feil med spacing. 
					// eks. Landskap og Landskap2 vil bli det samme grunnet Contains
					if ((!((string) treeViewList[i].Header).Contains(parentItem.Header.ToString())))
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
			_treeView.Items.Remove(sender);

			foreach (TreeViewItem treeView in _treeView.Items)
			{
				treeView.Items.Remove(sender);				
			}

			if (((TreeViewItem)sender).HasItems)
			{
				foreach (var assets in ((TreeViewItem)sender).Items)
				{
					_assetDatabaseHandler.Delete(((TreeViewItem)assets).Header.ToString());
				}
			}
			_assetDatabaseHandler.Delete(((TreeViewItem)sender).Header.ToString());

		}

		private void Init()
		{
			var assetData = _assetDatabaseHandler.GetAllRows();

			using (var enumerator = assetData.GetEnumerator())
				while (enumerator.MoveNext())
					Add(enumerator.Current);
		}

		public void Update()
		{
			_treeView.Items.Clear();
			Init();
		}
	}
}

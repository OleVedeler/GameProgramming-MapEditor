﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MapEditor.Handlers
{
	class TreeViewHandler
	{
		private readonly TreeView _treeView;
		private readonly AssetDatabaseHandler _assetDatabaseHandler;

		public TreeViewHandler(
						TreeView treeView, 
						AssetDatabaseHandler assetDatabaseHandler)
		{
			_treeView = treeView;
			_assetDatabaseHandler = assetDatabaseHandler;
			Init();
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
			TreeViewItem newItem = new TreeViewItem {Header = newAsset.Name};
			TreeViewItem parentItem = new TreeViewItem {Header = newAsset.Parent};

			if (_treeView.Items.Count != 0)
			{
				//_treeView.Items.Add(parentItem);
				// Legger treet inn i listen
				List<TreeViewItem> treeViewList = _treeView.Items.Cast<TreeViewItem>().ToList();

				// returnerer hvis den har lagt til elementet
				for (int i = 0; i < _treeView.Items.Count; i++)
				{
					// Todo: Fix Feil med spaceing. 
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
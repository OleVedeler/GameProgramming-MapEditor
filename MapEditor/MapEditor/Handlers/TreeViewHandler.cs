using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MapEditor.Handlers
{
	class TreeViewHandler
	{
		public TreeView TreeView { get; private set;}
		private readonly AssetDatabaseHandler _assetDatabaseHandler;

		public TreeViewHandler(TreeView treeView, AssetDatabaseHandler assetDatabaseHandler)
		{
			TreeView = treeView;
			_assetDatabaseHandler = assetDatabaseHandler;
			InitTreeView();
		}

		/// <summary>
		/// Legger DatabaseObjekter inn til TreeViewet. 
		/// ikke Superheit kode, men har sittet 6 timer med dette og det funker.
		/// Kommer mest sannsynlig til skrive om koden igjen når jeg er mer edru og har tid.
		/// </summary>
		/// <param name="newAsset">Er en database rad</param>

		public void Add(Asset newAsset)
		{
			TreeViewItem newItem = new TreeViewItem { Header = newAsset.Name };
			TreeViewItem parentItem = new TreeViewItem { Header = newAsset.Parent };

			// For å kunne compare, Burde kunne gjøres bedre, men fant ingen enkel måte

			// Leger inn det første elementet hvis, den er tom. Burde kunne implementeres inni loopen
			if (TreeView.Items.Count == 0)
			{
				TreeView.Items.Add(parentItem);
			}
			// Legger treet inn i listen
			List<TreeViewItem> treeViewList = TreeView.Items.Cast<TreeViewItem>().ToList();

			// returnerer hvis den har lagt til elementet
			for (int i = 0; i < TreeView.Items.Count; i++)
			{
				// Todo: Fix Feil me spaceing. 
				// eks. Landskap og Landskap2 vil bli det samme grunnet Contains
				if ((!((string)treeViewList[i].Header).Contains(parentItem.Header.ToString()))) continue;
				
				((TreeViewItem)TreeView.Items[i]).Items.Add(newItem);
				return;
			}

			TreeView.Items.Add(parentItem);
			parentItem.Items.Add(newItem);
		}

		private void InitTreeView()
		{
			UpdateTreeView();
		}

		public void UpdateTreeView()
		{
			TreeView.Items.Clear();
			var assetData = _assetDatabaseHandler.GetAllRows();

			using (var enumerator = assetData.GetEnumerator())
				while (enumerator.MoveNext())
					Add(enumerator.Current);
		}
	}
}

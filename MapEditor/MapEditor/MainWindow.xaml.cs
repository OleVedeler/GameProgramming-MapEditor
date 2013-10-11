using MapEditor.Handlers;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;


namespace MapEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{		
		public MainWindow()
		{
			InitializeComponent();
            PropertyHandler propertyHandler = new PropertyHandler(isObstacle);
			AssetDatabaseHandler assetDatabaseHandler = new AssetDatabaseHandler();
			ImageHandler imageHandler = new ImageHandler(ShowcaseAsset, assetDatabaseHandler);
			TreeViewHandler treeViewHandler = new TreeViewHandler(ComponentsTreeView, assetDatabaseHandler, imageHandler);
			GameGridHandler gameGridHandler = new GameGridHandler(EditorGrid, assetDatabaseHandler, treeViewHandler, propertyHandler);
			MenuHandler menuHandler = new MenuHandler(MainMenu, gameGridHandler, inputBox);
		}

        private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
	}
}

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
			AssetDatabaseHandler assetDatabaseHandler = new AssetDatabaseHandler();
			TreeViewHandler treeViewHandler = new TreeViewHandler(ComponentsTreeView, assetDatabaseHandler);
			GameGridHandler gameGridHandler = new GameGridHandler(EditorGrid, assetDatabaseHandler, treeViewHandler);
			MenuHandler menuHandler = new MenuHandler(MainMenu, gameGridHandler, inputBox);
		}
	}
}

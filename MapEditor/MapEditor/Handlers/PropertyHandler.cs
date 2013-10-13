using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Handlers
{
	public class PropertyHandler
    {
        public CheckBox ObsticalCheckBox { get; set; }

		private ListBox _propertyBox;
		public ListBox PropertyBox {
			get
			{
				return _propertyBox;
			}
			private set
			{
				if (_propertyBox == null)
				{
					_propertyBox = value;
				}
				_propertyBox.Items.Clear();

				for (int index = 0; index < value.Items.Count; index++)
				{
					var item = new CheckBox();
					item.Content = ((ListBoxItem)value.Items[index]).Content;
					item.IsChecked = ((CheckBox)value.Items[index]).IsChecked;

					_propertyBox.Items.Add(item);
				}
			}
		}

		public string Name;
        public PropertyHandler(ListBox propertyBox, string name)
    {
	        PropertyBox = propertyBox;
	        Name = name;
            ObsticalCheckBox = new CheckBox {Content = "Is Obsticle"};
			PropertyBox.Items.Add(new ListBoxItem { Content = Name });
	        PropertyBox.Items.Add(ObsticalCheckBox);
			SetVisability(false);
        }

		public void SetVisability(bool visible)
        {
			if (!visible){
				(PropertyBox.Items[0] as ListBoxItem).Visibility = Visibility.Collapsed;
				(PropertyBox.Items[1] as CheckBox).Visibility = Visibility.Collapsed;
			} else {
				(PropertyBox.Items[0] as ListBoxItem).Visibility = Visibility.Visible;
				(PropertyBox.Items[1] as CheckBox).Visibility = Visibility.Visible;
			}
        }
    }
}

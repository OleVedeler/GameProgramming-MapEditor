using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MapEditor.Handlers
{
    class PropertyHandler
    {
        public CheckBox _isObstacle;
        public TextBlock _nameText;
        public TextBlock _pathText;

        public PropertyHandler(CheckBox isObstacle, TextBlock nameText, TextBlock pathText)
        {
            _isObstacle = isObstacle;
            _nameText = nameText;
            _pathText = pathText;
        }
    }
}

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

        public PropertyHandler(CheckBox isObstacle)
        {
            _isObstacle = isObstacle;
        }
    }
}

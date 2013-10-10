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
        public CheckBox isObstacle;

        public PropertyHandler(CheckBox isObstacle)
        {
            this.isObstacle = isObstacle;
        }
    }
}

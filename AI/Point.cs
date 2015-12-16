using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1.AI
{
    class Point 
    {
        public int x, y;
        public DirectionConstant end_direction, start_direction;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

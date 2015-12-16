using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1.AI
{
    class Node
    {
        DirectionConstant startDirection, endDirection;
        int g, h; //values from the dijkstra's, Heuristics 
        int x, y;
        Node parent;
        int nodeType;
        bool walkable;

        public DirectionConstant StartDirection
        {
            get
            {
                return startDirection;
            }
            set
            {
                startDirection = value;
            }
        }

        public DirectionConstant EndDirection
        {
            get
            {
                return endDirection;
            }
            set
            {
                endDirection = value;
            }
        }

        public int G
        {
            get
            {
                return g;
            }
            set
            {
                g = value;
            }
        }

        public int H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
            }
        }

        public int F
        {
            get
            {
                return G + H;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public Node Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public int Type
        {
            get
            {
                return nodeType;
            }
            set
            {
                nodeType = value;
            }
        }

        public bool Walkable
        {
            get
            {
                return walkable;
            }
            set
            {
                walkable = value;
            }
        }
    }
}

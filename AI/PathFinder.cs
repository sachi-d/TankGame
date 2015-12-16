using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1.AI
{
    class PathFinder
    {
        Map clientMap;
        const int width = Map.MAP_WIDTH;
        const int height = Map.MAP_HEIGHT;
        Node[,] map = new Node[width, height];
        int startX, startY, endX, endY;
        List<Node> openList, closedList;
        Game_AI.Mode mode;
        Stack<Point> path;
        int pathLength;
        DirectionConstant currentDirection;
        bool path_calculated;

        public PathFinder(Map map)
        {
            clientMap = map;
            openList = new List<Node>();
            closedList = new List<Node>();
            mode = Game_AI.Mode.Greedy;
        }

        //genarating the client map which contains nodes

        public void generateMap()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Actor actor = clientMap.getActor(i, j);
                    Node node = new Node();
                    node.X = i;
                    node.Y = j;
                    if (actor != null)
                    {
                        if (actor.GetType() == typeof(Brick))
                        {
                            node.Type = NodeType.brick;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Player))
                        {
                            node.Type = NodeType.tank;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Coin))
                        {
                            node.Type = NodeType.coinPile;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Stone))
                        {
                            node.Type = NodeType.stone;
                            node.Walkable = false;
                        }
                        else if (actor.GetType() == typeof(LifePack))
                        {
                            node.Type = NodeType.lifePack;
                            node.Walkable = true;
                        }
                        else if (actor.GetType() == typeof(Water))
                        {
                            node.Type = NodeType.water;
                            node.Walkable = false;
                        }
                    }
                    else
                    {
                        node.Type = NodeType.empty;
                        node.Walkable = true;
                    }
                    map[i, j] = node;
                }
            }
        }

        public Stack<Point> findShortestPath()
        {
            openList.Clear();
            closedList.Clear();
            path_calculated = false;
            Stack<Point> path;
            currentDirection = clientMap.Client.getcDirection();

            switch (mode)
            {
                //revalue the node types in the context of each mode
                case Game_AI.Mode.Offensive:
                    NodeType.tank = 1;
                    NodeType.coinPile = 2;
                    NodeType.brick = 3;
                    NodeType.lifePack = 4;
                    break;
                case Game_AI.Mode.Greedy:
                    NodeType.coinPile = 1;
                    NodeType.lifePack = 2;
                    NodeType.brick = 3;
                    NodeType.tank = 4;
                    break;
                case Game_AI.Mode.Defensive:
                    NodeType.lifePack = 1;
                    NodeType.coinPile = 2;
                    NodeType.brick = 3;
                    NodeType.tank = 4;
                    break;
            }

            //generates the client map for path finding
            generateMap();

            startX = clientMap.Client.getxCordinate();
            clientMap.Client.getyCordinate();

            openList.Add(map[startX, startY]);

            while (!path_calculated)
            {
                analyzeNeighbours();
            }
            this.path = findPath();
            return this.path;
        }

        public void analyzeNeighbours()
        {
            if (openList.Count() == 0)
            {
                path_calculated = true;
            }
            Node currentNode = openList.ElementAt(0);
            if (!openList.Remove(currentNode))
            {
                throw new InvalidOperationException("Node not found in the list");
            }
            closedList.Add(currentNode);
            if (currentNode.X == endX && currentNode.Y == endY)
            {
                path_calculated = true;
            }
            if (currentNode.X - 1 >= 0)
            {
                Node neighbor = map[currentNode.X - 1, currentNode.Y];
                analyzeNode(currentNode, neighbor);
            }
            if (currentNode.X + 1 <= width)
            {
                Node neighbor = map[currentNode.X + 1, currentNode.Y];
                analyzeNode(currentNode, neighbor);
            }
            if (currentNode.Y - 1 >= 0)
            {
                Node neighbor = map[currentNode.X , currentNode.Y-1];
                analyzeNode(currentNode, neighbor);
            }
            if (currentNode.Y + 1 >= 0)
            {
                Node neighbor = map[currentNode.X , currentNode.Y+1];
                analyzeNode(currentNode, neighbor);
            }
            sortList(openList);
        }

        public void analyzeNode(Node current, Node neighbor){
            if (neighbor.Walkable && !closedList.Contains(neighbor))
            {
                int g = current.G + neighbor.Type + RotationalDelay(current, neighbor);
                int h = calculate_H(neighbor);

                if (g < neighbor.G || !openList.Contains(neighbor))
                {
                    neighbor.G = g;
                    neighbor.H = h;
                    neighbor.Parent = current;
                    setDirection(neighbor);

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    
                }               
            }
        }

        public int RotationalDelay(Node current, Node next){
            int delay = 0;
            if (current.StartDirection == DirectionConstant.Down)
            {
                if (current.X != next.X || current.Y > next.Y)
                {
                    delay = NodeType.empty;
                }
            }
            if (current.StartDirection == DirectionConstant.Up)
            {
                if (current.X != next.X || current.Y < next.Y)
                {
                    delay = NodeType.empty;
                }
            }
            
            if(current.StartDirection == DirectionConstant.Right){
                if(current.Y != next.Y || current.X > next.X){
                    delay = NodeType.empty;
                }
            }
            if(current.StartDirection == DirectionConstant.Left){
                if(current.Y != next.Y || current.X < next.X){
                    delay = NodeType.empty;
                }
            }
            return delay;
        }

        public int calculate_H(Node node)
        {
            int xLength = Math.Abs(endX - node.X);
            int yLength = Math.Abs(endY - node.Y);
            return xLength + yLength;
        }

        public void setDirection(Node node)
        {
            Node parent = node.Parent;
            if (parent == null)
            {
                node.StartDirection = currentDirection;
            }
            if (node.X > parent.X)
            {
                node.StartDirection = DirectionConstant.Right;
            }
            if (node.X < node.Y)
            {
                node.StartDirection = DirectionConstant.Left;
            }
            if (node.Y > parent.Y)
            {
                node.StartDirection = DirectionConstant.Down;
            }
            if (node.Y < parent.Y)
            {
                node.StartDirection = DirectionConstant.Up;
            }
        }

        public void sortList(List<Node> list)
        {
            list.Sort(sortListByFValue);
        }

        public int sortListByFValue(Node node_1, Node node_2)
        {
            int return_value = 0;
            return_value = node_1.F - node_2.F;
            if (return_value == 0)
            {
                return_value = node_1.G - node_2.G;
            }
            if (return_value == 0)
            {
                return_value = node_1.H - node_2.H;
            }
            return return_value;
        }

        public Stack<Point> findPath()
        {
            Stack<Point> path = new Stack<Point>();
            Node node = map[endX, endY];  //get target
            while (node != null && !(node.X == startX && node.Y == startY))
            {
                Point point = new Point(node.X, node.Y);
                point.start_direction = node.StartDirection;
                point.end_direction = node.EndDirection;
                path.Push(point);
                node = node.Parent;
            }
            return path;
        }

        public Stack<Cell> calculateShortestPath(Cell target, Cell current)
        {
            this.startX = current.x;
            this.startY = current.y;
            this.endX = current.x;
            this.endY = current.y;

            Stack<Point> point_path = findShortestPath();
            Stack<Cell> temp = new Stack<Cell>();
            foreach (Point p in point_path)
            {
                Cell c = new Cell();
                c.x = p.x;
                c.y = p.y;
                temp.Push(c);
            }

            Stack<Cell> cell_path = new Stack<Cell>();      //to reverse the stack
            foreach (Cell cell in temp)
            {
                cell_path.Push(cell);
            }

            return cell_path;
        }
    }
}

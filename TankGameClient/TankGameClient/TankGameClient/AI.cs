using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankGameClient
{
    class AI
    {

        #region variables
        TcpClient serv;
        NetworkStream stm;
        String server_ip = "localhost";

        // our tank location
        int[] our_tank_location = { 0, 0 };
        
        //target
        private Block target;

        // mssages form the server
        string server_msg = "";

        //constant directions
       public enum DirectionConstant
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

        //Node class
       public  class Node
    {
        DirectionConstant startDirection, endDirection;
        int g, h; //values from the dijkstra's, Heuristics 
        int x, y;

        //int[] parent = new int[2];
        
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
        
      public class NodeType
       {
           public static int coinPile = 1,
               lifePack = 10,
               tank = 100,
               empty = 50,
               brick = 250,
               bullet = 10000,
               water = 100000,
               stone = 100000;


       }

      public class Point
      {
          public int x, y;
          public DirectionConstant end_direction, start_direction;

          public Point(int x, int y)
          {
              this.x = x;
              this.y = y;
          }
      }

       // grid of the game
       Block[,] blocks;
       int grid_size;
       String player_id;
        
       // node grid
       Node[,] grid;
        #endregion

       const int width = 10;
       const int height =10;
       int startX, startY, endX, endY;
       List<Node> openList, closedList;
       Stack<Point> path;
       int pathLength;
       int currentDirection;
       bool path_calculated;

       //AI constructor
       public AI(Block[,] blocks, int size, String player_id)
       {
           openList = new List<Node>();
           closedList = new List<Node>();
           this.blocks = blocks;
           this.grid_size = size;
           this.player_id = player_id;
           grid = new Node[size, size];
           // initializing grid 
           for (int i = 0; i < size; i++)
           {
               for (int j = 0; j < size; j++)
               {
                   Node node = new Node();
                   node.X = i;
                   node.Y = j;

                   if (blocks[i, j].get_type() == 1){
                       node.Type=NodeType.brick;
                       node.Walkable = true;
                   }
                   else  if (blocks[i, j].get_type() == 2){
                       node.Type=NodeType.stone;
                       node.Walkable=false;
                   }
                   else if (blocks[i, j].get_type() == 3) // if it is an obstacle (brick,water,stone)
                   {
                       node.Type = NodeType.water;
                       node.Walkable = false;
                   }
                   else if (blocks[i, j].get_type() == 4)
                   {
                       node.Type = NodeType.coinPile;
                   }
                   else if (blocks[i, j].get_type() == 5)
                   {
                       node.Type = NodeType.lifePack;
                   }
                   else if (blocks[i, j].get_type() == 6)
                   {
                       node.Type = NodeType.tank;
                   }
                   else
                   {
                       node.Type = NodeType.empty;
                       node.Walkable = true;
                   }
                   grid[i, j] = node;
                   get_our_tank_location();
               }
           }
       }

       public void generateGrid(Block[,] blocks)
       {
           
           this.blocks = blocks;
           // initializing grid 
           for (int i = 0; i < width; i++)
           {
               for (int j = 0; j < height; j++)
               {
                   Node node = new Node();
                   node.X = i;
                   node.Y = j;

                   if (blocks[i, j].get_type() == 1)
                   {
                       node.Type = NodeType.brick;
                       node.Walkable = true;
                   }
                   else if (blocks[i, j].get_type() == 2)
                   {
                       node.Type = NodeType.stone;
                       node.Walkable = false;
                   }
                   else if (blocks[i, j].get_type() == 3) // if it is an obstacle (brick,water,stone)
                   {
                       node.Type = NodeType.water;
                       node.Walkable = false;
                   }
                   else if (blocks[i, j].get_type() == 4)
                   {
                       node.Type = NodeType.coinPile;
                   }
                   else if (blocks[i, j].get_type() == 5)
                   {
                       node.Type = NodeType.lifePack;
                   }
                   else if (blocks[i, j].get_type() == 6)
                   {
                       node.Type = NodeType.tank;
                   }
                   else
                   {
                       node.Type = NodeType.empty;
                       node.Walkable = true;
                   }
                   grid[i, j] = node;
               }
           }

           get_our_tank_location();
       }

       // get current tank locaion
       public void get_our_tank_location()
       {
           for (int i = 0; i < grid_size; i++)
           {
               for (int j = 0; j < grid_size; j++)
               {
                   Block block = blocks[i, j];
                   if (block.get_type() == 6) // if it is a tank
                   {
                       if (String.Compare(this.player_id, block.get_tank_id()) == 0)   // if this is our tank
                       {
                           this.our_tank_location[0] = i;
                           this.our_tank_location[1] = j;
                           return; // exit

                       }
                   }
               }
           }


       }


       public Stack<Point> findShortestPath()
       {
           openList.Clear();
           closedList.Clear();
           path_calculated = false;
           Stack<Point> path;
           int  currentDirection = blocks[our_tank_location[0],our_tank_location[1]].direction;

         //  switch (mode)
           //{
               //revalue the node types in the context of each mode
             /*  case Game_AI.Mode.Offensive:
                   NodeType.tank = 1;
                   NodeType.coinPile = 2;
                   NodeType.brick = 3;
                   NodeType.lifePack = 4;
                   break; */
            //   case Game_AI.Mode.Greedy:
                   NodeType.coinPile = 1;
                   NodeType.lifePack = 2;
                   NodeType.brick = 3;
                   NodeType.tank = 4;
                 //  break;
             /*  case Game_AI.Mode.Defensive:
                   NodeType.lifePack = 1;
                   NodeType.coinPile = 2;
                   NodeType.brick = 3;
                   NodeType.tank = 4;
                   break;*/
           //}

           //generates the client map for path finding
          

           startX = (int) (this.blocks[our_tank_location[0],our_tank_location[1]].position.X);
           startY = (int)(this.blocks[our_tank_location[0], our_tank_location[1]].position.Y);

           openList.Add(grid[startX, startY]);

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
               Node neighbor = grid[currentNode.X - 1, currentNode.Y];
               analyzeNode(currentNode, neighbor);
           }
           if (currentNode.X + 1 <= width)
           {
               Node neighbor = grid[currentNode.X + 1, currentNode.Y];
               analyzeNode(currentNode, neighbor);
           }
           if (currentNode.Y - 1 >= 0)
           {
               Node neighbor = grid[currentNode.X, currentNode.Y - 1];
               analyzeNode(currentNode, neighbor);
           }
           if (currentNode.Y + 1 >= 0)
           {
               Node neighbor = grid[currentNode.X, currentNode.Y + 1];
               analyzeNode(currentNode, neighbor);
           }
           sortList(openList);
       }
       
        
       public void analyzeNode(Node current, Node neighbor)
       {
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


       public int RotationalDelay(Node current, Node next)
       {
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

           if (current.StartDirection == DirectionConstant.Right)
           {
               if (current.Y != next.Y || current.X > next.X)
               {
                   delay = NodeType.empty;
               }
           }
           if (current.StartDirection == DirectionConstant.Left)
           {
               if (current.Y != next.Y || current.X < next.X)
               {
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
               
               node.StartDirection = (DirectionConstant)currentDirection;
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
           Node node = grid[endX, endY];  //get target
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

       public Stack<Block> calculateShortestPath(Block target, Block current)
       {
           this.startX = (int)current.position.X;
           this.startY = (int)current.position.Y;
           this.endX = (int)current.position.X;
           this.endY = (int)current.position.Y;

           Stack<Point> point_path = findShortestPath();
           Stack<Block> temp = new Stack<Block>();
           foreach (Point p in point_path)
           {
               Block c = new Block();
               c.position.X = p.x;
               c.position.Y= p.y;
               temp.Push(c);
           }

           Stack<Block> cell_path = new Stack<Block>();      //to reverse the stack
           foreach (Block cell in temp)
           {
               cell_path.Push(cell);
           }

           return cell_path;
       }

       public void computeTarget(Block block)
       {
           //Cell Target = null;
           //  switch (mode)
           // {
           /*   case Mode.Offensive:
                  Target = getTankTarget(cell);
                  if (Target == null)
                  {
                      Target = getCoinPileTarget(cell);
                  }
                  if (Target == null)
                  {
                      Target = getBrickTarget(cell);
                  }
                  if (Target == null)
                  {
                      if (map.Client.getHealth() < 100)
                      {
                          Target = getLifePackTarget(cell);
                      }
                  }
                  break;
              case Mode.Greedy: */
           target = getCoinPileTarget(block);
         /*  if (target == null)
           {
               target = getTankTarget(cell);           //**check the prioroties
           }
           if (target == null)
           {
               target = getBrickTarget(cell);
           }
           if (target == null)
           {
               if (map.Client.getHealth() < 100)
               {
                   target = getLifePackTarget(cell);
               }
           }*/
           /*      break;
             case Mode.Defensive:
                 Target = getLifePackTarget(cell);
                 if (Target == null)
                 {
                     Target = getCoinPileTarget(cell);
                 }
                 if (Target == null)
                 {
                     Target = getBrickTarget(cell);           //**check the prioroties
                 }
                 if (Target == null)
                 {
                     Target = getTankTarget(cell);
                 }
                 break; */
           //  }
           if (target == null)
           {
               target = blocks[our_tank_location[0], our_tank_location[1]];
           }

       }

       public List<Block> getCoinCells()
       {
           List<Block> coins = new List<Block>();

           for (int i = 0; i < grid_size; i++)
           {
               for (int j = 0; j < grid_size; j++)
               {
                   Block block = blocks[i, j];
                   if (block.get_type() == 4) // if it is a tank
                   {
                            coins.Add(block);

                       }
                   }
               }
           return coins;
        }


        public Block getCoinPileTarget(Block block)
       {
           List<Block> coinPileTargets = getCoinCells();
           int distance = 1000;
           Block target = null;
           int calculated_distance;
           foreach (Block coinPile in coinPileTargets)
           {
               calculated_distance = getDistance(block, coinPile);           //client_tank_speed=(1/60)*1000
               if (calculated_distance < distance && (calculated_distance / (1000 / 60)) < coinPile.getRemaining_time())
               {
                   distance = calculated_distance;
                   target = coinPile;
               }
           }
           return target;
       }


        public int getDistance(Block block_1, Block block_2)
        {
            int dx = (int)Math.Abs(block_2.position.X - block_1.position.X);
            int dy =(int) Math.Abs(block_2.position.Y - block_1.position.Y);
            return dx + dy;
        }


        #region messagepassingwithserver

        // accepting the server messages
        public void new_sever_msg(Object sender, ServerMessageArgs e)
        {
            this.server_msg = e.msg;
        }

        public void set_player_id(String id)
        {
            this.player_id = id;
        }

        public void send_message_to_server(String str)
        {

            try
            {
                serv = new TcpClient();
                serv.Connect(server_ip, 6000);
                stm = serv.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                stm.Close();
                serv.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
        #endregion

        #region tank_moves

        // move our tank one time left
        public void move_left()
        {
            String msg = "LEFT#";
            send_message_to_server(msg);
        }
        //move our tank one time right
        public void move_right()
        {
            String msg = "RIGHT#";
            send_message_to_server(msg);
        }
        // move our message one time dowm
        public void move_down()
        {
            String msg = "DOWN#";
            send_message_to_server(msg);
        }
        //move our tank one time up
        public void move_up()
        {
            String msg = "UP#";
            send_message_to_server(msg);
        }

        #endregion




    }
}
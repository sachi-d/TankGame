using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1
{
    public class Map
    {
        public List<Player> tanks
        {
            get;
            set;
        }
        public List<Brick> bricks
        {
            get;
            set;
        }
        public List<LifePack> lifePacks
        {
            get;
            set;
        }
        public List<Coin> coinPiles
        {
            get;
            set;
        }
        public List<Stone> stones
        {
            get;
            set;
        }
        public List<Water> waterPits
        {
            get;
            set;
        }
        public List<Bullet> bullets
        {
            get;
            set;
        }

        public List<Block> freeCells
        {
            get;
            set;
        }
        public const int MAP_WIDTH = 10, MAP_HEIGHT = 10;
        private Player client;
       // private String clientID;
       // private int clientX, clientY;
        //DirectionConstant clientDirection;
        bool joined;

        public Player Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }

        public Map()
        {
            tanks = new List<Player>();
            bricks = new List<Brick>();
            lifePacks = new List<LifePack>();
            coinPiles = new List<Coin>();
            stones = new List<Stone>();
            waterPits = new List<Water>();
            bullets = new List<Bullet>();
            freeCells = new List<Block>();
            Client = new Player();

        }
        /*public String ClientID
        {
            get
            {
                return clientID;
            }
            set
            {
                clientID = value;
            }
        }

        public int ClientX
        {
            get
            {
                return clientX;
            }
            set
            {
                clientX = value;
            }
        }

        public int ClientY
        {
            get
            {
                return clientY;
            }
            set
            {
                clientY = value;
            }
        }*/

        public Actor getActor(int x, int y)
        {
            foreach (Player p in tanks)
            {
                if (p.getxCordinate() == x && p.getyCordinate() == y)
                {
                    return p;
                }
            }
            foreach (Brick b in bricks)
            {
                if (b.getxCordinate() == x && b.getyCordinate() == y)
                {
                    return b;
                }
            }
            foreach (Coin c in coinPiles)
            {
                if (c.getxCordinate() == x && c.getyCordinate() == y)
                {
                    return c;
                }
            }
            foreach (Stone s in stones)
            {
                if (s.getxCordinate() == x && s.getyCordinate() == y)
                {
                    return s;
                }
            }
            foreach (LifePack l in lifePacks)
            {
                if (l.getxCordinate() == x && l.getyCordinate() == y)
                {
                    return l;
                }
            }
            foreach (Player p in tanks)
            {
                if (p.getxCordinate() == x && p.getyCordinate() == y)
                {
                    return p;
                }
            }
            foreach (Water w in waterPits)
            {
                if (w.getxCordinate() == x && w.getyCordinate() == y)
                {
                    return w;
                }
            }
            return null;
        }

         
    }
}

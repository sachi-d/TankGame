using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_1
{
    public class Player : Actor
    {
        String playerID;
        int coins;
        int points;
        int health;
        DirectionConstant cDirection;
        String whetherShot;
        long speed;

        public Player()
        {
            Type = Actors.ActorType.Player;
        }
  

        public void setId(String id)
        {
            playerID = id;
        }

        public String getid()
        {
            return playerID;
        }

        public void setHealth(int health)
        {
            this.health = health ;
        }

        public int getHealth()
        {
            return health;
        }

        public void setCoins(int coins)
        {
            this.coins=coins;
        }

        public int getCoins()
        {
            return coins;
        }

        public int getPoints()
        {
            return points;
        }

        public long Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

       

        public String getwhetherShot()
        {
            return whetherShot;
        }

        public void setPoints(int points)
        {
            this.points = points;
        }
        
        public void setwhetherShot(String shot)
        {
            this.whetherShot = shot;
        }

    }
}

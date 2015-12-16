using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_1
{
    public class Brick : Actor
    {
        private String LifeLevel = "100%";

        public Brick()
        {
            Type = Tank_1.Actors.ActorType.Brick;
        }
        

        public void setLife(String lifeamount)
        {
            this.LifeLevel = lifeamount;
        }

        public String getLife()
        {
            return this.LifeLevel;
        }

        
    }
}

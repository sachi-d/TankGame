using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_1
{
    public class LifePack : Actor
    {
        
        int time;

        public LifePack()
        {
            Type = Tank_1.Actors.ActorType.LifePack;
        }
        
        public int getTime()
        {
            return time;
        }

       

        public void setTime(int t)
        {
            time = t;
        }

    }
}

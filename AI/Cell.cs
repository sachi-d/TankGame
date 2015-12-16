using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1.AI
{
    public class Cell
    {
        public int x{
            get;
            set;
        }
        public int y{
            get;
            set;
        }
        public DirectionConstant cellDirection{
            get;
            set;
        }
        public Actors.ActorType Type
        {
            get;
            set;
        }
        public int health
        {
            get;
            set;
        }

        public int points
        {
            get;
            set;
        }
        public int value
        {
            get;
            set;
        }
        public long life
        {
            get;
            set;
        }

        


       

        

    }
}

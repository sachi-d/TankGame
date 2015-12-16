using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1
{
    public class Actor
    {
        private Tank_1.Actors.ActorType type;
        private int xCordinate;
        private int yCordinate;
        private DirectionConstant cDirection;
        private bool exists;


        internal Tank_1.Actors.ActorType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public bool Exists
        {
            get
            {
                return exists;
            }
            set
            {
                exists = value;
            }
        }

        public void setCordinates(int x, int y)
        {
            this.xCordinate = x;
            this.yCordinate = y;
        }

        public int getxCordinate()
        {
            return this.xCordinate;
        }

        public int getyCordinate()
        {
            return this.yCordinate;
        }

        public DirectionConstant getcDirection()
        {
            return cDirection;
        }

        public void setCDirection(DirectionConstant d)
        {
            this.cDirection = d;
        }



        public void dissappear()
        {
            this.Exists = false;
        }
   
    }
}

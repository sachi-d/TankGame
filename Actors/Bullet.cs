using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1
{
   public class Bullet : Actor
    {
       int id;
       private DirectionConstant direction;
       public const int BULLET_RANGE = 3;

       public Bullet()
       {
           Type = Tank_1.Actors.ActorType.Bullet;

       }

       public int ID
       {
           get{
               return id;
           }
           set
           {
               id = value;
           }
         
       }

       public DirectionConstant Direction
       {
           get
           {
               return direction;
           }
           set
           {
               direction = value;
           }
       }
      
       public DirectionConstant getDirection(){
           return Direction;
       }
       public void Move()
       {
           int temp;
           switch (getDirection())
           {
               case DirectionConstant.Down:
                   temp = getyCordinate() + BULLET_RANGE;
                   if (temp > Map.MAP_HEIGHT)
                   {
                       dissappear();
                       break;
                   }
                   setCordinates(getxCordinate(), temp);
                   break;
                case DirectionConstant.Up:
                   temp = getyCordinate() - BULLET_RANGE;
                   if (temp< 0){
                       dissappear();
                       break;
                   }
                   setCordinates(getxCordinate(), temp);
                   break;
               case DirectionConstant.Left:
                   temp = getxCordinate() - BULLET_RANGE;
                   if (temp < 0)
                   {
                       dissappear();
                       break;
                   }
                   setCordinates(temp, getyCordinate());
                   break;
               case DirectionConstant.Right:
                   temp = getxCordinate() + BULLET_RANGE;
                   if (temp > Map.MAP_WIDTH)
                   {
                       dissappear();
                       break;
                   }
                   setCordinates(temp, getyCordinate());
                   break;
           }
       }
    }
}

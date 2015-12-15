using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Timers;

namespace TankGameClient
{
    class Block
    {
        public Vector2 position;
        public Texture2D texture;
        public Color color;
        private int remaining_time;
        public int type;
        int define_sec = 1000;
        ContentManager cont;
        Timer _timer;

        // variables for tanks if there is a tank in the block
        int direction;
        String id;
        int shooted;
        int health;
        int coins;
        int points;


        public Block()
        {
            color = Color.White;
            remaining_time = -100;
            type = 0;
            _timer = new Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(reduce_remaining_time);
            _timer.Enabled = true;
        }

        public Block(Vector2 position)
        {
            color = Color.White;
            this.position = position;
            remaining_time = -100; //set default value for remaining time variable for an empty block/bricks/stone/water
            type = 0;
            _timer = new Timer(1000);   //set a timer of eacb block to check in every 1 second whether it contains a lifepack/coin to reduce its time
            _timer.Elapsed += new ElapsedEventHandler(reduce_remaining_time);
            _timer.Enabled = true;
        }

        // reduce and reset object in lifepacks and coins
        public void reduce_remaining_time(object sender, ElapsedEventArgs e)
        {

            
            if (this.remaining_time != -100) //if this is a brick/stone/water/empty-block
            {
                if (this.remaining_time <= 0)  //if the remaining time of the coin or life pack is exceeded
                {
                    this.type = 0;   //make it a empty-block again
                    texture = this.cont.Load<Texture2D>("Sprites/block");
                }
                else  //if the remaining time of coin/life pack has not exceeded
                {

                    this.remaining_time--; //reduce the remaining time of the coin/life pack by 1 second
                }
            }
        }


        public virtual void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/block");
            this.cont = content;
        }

        // draw the object
        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }


        public virtual void change_type(int type, ContentManager content, int remaining_time)
        {
            remaining_time = remaining_time / define_sec;
            // changing the type of the block
            this.type = type;
            // changing the block texture
            switch (type)
            {
                case 0:
                    texture = content.Load<Texture2D>("Sprites/block");
                    break;
                case 1:
                    texture = content.Load<Texture2D>("Sprites/brick");
                    break;
                case 2:
                    texture = content.Load<Texture2D>("Sprites/stone");
                    break;
                case 3:
                    texture = content.Load<Texture2D>("Sprites/water");
                    break;
                case 4:
                    texture = content.Load<Texture2D>("Sprites/coin");
                     this.remaining_time = remaining_time;
                    break;
                case 5:
                    texture = content.Load<Texture2D>("Sprites/lifePack");
                    this.remaining_time = remaining_time;
                    break;
               
            }


        }

        public void change_type(String id, ContentManager content, int direction, int shooted, int health, int coins, int point, bool our)
        {
            if (our)
            {
                switch (direction)
                {
                    case 0:
                        texture = content.Load<Texture2D>("Sprites/tank_up");
                        break;
                    case 1:
                        texture = content.Load<Texture2D>("Sprites/tank_right");
                        break;
                    case 2:
                        texture = content.Load<Texture2D>("Sprites/tank_down");
                        break;
                    case 3:
                        texture = content.Load<Texture2D>("Sprites/tank_left");
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case 0:
                        texture = content.Load<Texture2D>("Sprites/enemy_up");
                        break;
                    case 1:
                        texture = content.Load<Texture2D>("Sprites/enemy_right");
                        break;
                    case 2:
                        texture = content.Load<Texture2D>("Sprites/enemy_down");
                        break;
                    case 3:
                        texture = content.Load<Texture2D>("Sprites/enemy_left");
                        break;
                }
            }

            this.type = 6;
            this.id = id;
            this.direction = direction;
            this.shooted = shooted;
            this.health = health;
            this.coins = coins;
            this.points = point;
            this.remaining_time = -100;
        }

        public string get_tank_id()
        {
            return this.id;
        }

        // get the type of the block
        public int get_type()
        {
            return this.type;
        }
        
    }
}

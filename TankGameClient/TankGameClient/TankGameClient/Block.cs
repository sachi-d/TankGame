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
        private int ttl;
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


        public Block(Vector2 position)
        {
            color = Color.White;
            this.position = position;
            ttl = -100;
            type = 0;
           // _timer = new Timer(1000);
           // _timer.Elapsed += new ElapsedEventHandler(reduce_ttl);
           // _timer.Enabled = true;
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


        public virtual void change_type(int type, ContentManager content, int ttl)
        {
            //ttl = ttl / define_sec;
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
                //    this.ttl = ttl;
                    break;
                case 5:
                    texture = content.Load<Texture2D>("Sprites/lifePack");
                  //  this.ttl = ttl;
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

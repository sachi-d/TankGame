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

namespace TankGameClient
{
    class Water: Block
    {

        public Water() :base()
        {
            
        }

        public Water(Vector2 position)
            : base(position)
        {

        }
        public override void loadContent(ContentManager content)
        {
            base.texture = content.Load<Texture2D>("Sprites/water");
        }
    }
}

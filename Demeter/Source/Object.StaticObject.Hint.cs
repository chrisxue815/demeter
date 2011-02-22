using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    class Hint : StaticObject
    {
        public Hint(Game1 game, Texture2D img, Vector2 pos)
            : base(game, pos)
        {
            texture = img;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public override void LoadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }
}

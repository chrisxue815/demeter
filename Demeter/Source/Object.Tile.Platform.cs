using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Platform : Tile
    {
        public Platform(Game1 game, Vector2 position, TileFrame tileFrame)
            : base(game, position, tileFrame)
        {
        }

        public Platform(Game1 game, Vector2 position, int width)
            : base(game, position, width)
        {
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Mirror.Mirror2");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            //throw new NotImplementedException();
        }
    }
}

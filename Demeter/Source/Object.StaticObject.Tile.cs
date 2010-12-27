using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Tile : StaticObject
    {
        public Tile(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        public override void LoadContent()
        {
            this.texture = game.Content.Load<Texture2D>("Object.StaticObject.Tile.Tile1");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }
}

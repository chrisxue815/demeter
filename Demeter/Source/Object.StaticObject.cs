using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public abstract class StaticObject : Object
    {
        public override Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)(position.X) + collisionOffset,
                    (int)(position.Y) + collisionOffset,
                    texture.Width - 2 * collisionOffset,
                    texture.Height - 2 * collisionOffset);
            }
        }

        public StaticObject(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            game.spriteBatch.Draw(this.texture, position, Color.White);
        }
    }
}

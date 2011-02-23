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
        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        public override int CollisionWidth
        {
            get { return texture.Width - LeftCollisionOffset - RightCollisionOffset; }
        }

        public override int CollisionHeight
        {
            get { return texture.Height - TopCollisionOffset - BottomCollisionOffset; }
        }
        
        public int HalfWidth
        {
            get { return texture.Width / 2; }
        }

        public int HalfHeight
        {
            get { return texture.Height / 2; }
        }

        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)(position.X) + TopCollisionOffset,
                    (int)(position.Y) + LeftCollisionOffset,
                    CollisionWidth, CollisionHeight);
            }
        }

        public StaticObject(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(this.texture, ScreenPosition, Color.White);
        }
    }
}

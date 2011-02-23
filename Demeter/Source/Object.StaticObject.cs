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
            get { return texture; }
            set { texture = value; }
        }

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        public override int CollisionWidth
        {
            get { return Texture.Width - LeftCollisionOffset - RightCollisionOffset; }
        }

        public override int CollisionHeight
        {
            get { return Texture.Height - TopCollisionOffset - BottomCollisionOffset; }
        }
        
        public int HalfWidth
        {
            get { return Texture.Width / 2; }
        }

        public int HalfHeight
        {
            get { return Texture.Height / 2; }
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
            Game.SpriteBatch.Draw(Texture,
                new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Width, Height),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
    }
}

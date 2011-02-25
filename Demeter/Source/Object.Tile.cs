using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public abstract class Tile : Object
    {
        public override int CollisionWidth
        {
            get { return width; }
        }
        protected int width;

        public override int CollisionHeight
        {
            get { return height; }
        }
        protected int height;

        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        Point tileFrame;

        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)(position.X),
                    (int)(position.Y), CollisionWidth, CollisionHeight);
            }
        }

        public Tile(Game1 game, Vector2 position, Point tileFrame)
            : base(game, position)
        {
            this.tileFrame = tileFrame;
        }

        public Tile(Game1 game, Vector2 position, int width)
            : base(game, position)
        {
            this.width = width;
            this.height = 0;
            int frameX = (int)Math.Ceiling((double)width / texture.Width);
            int frameY = 1;
            tileFrame = new Point(frameX, frameY);
        }

        public Tile(Game1 game, Vector2 position, int width, int height)
            : base(game, position)
        {
            this.width = width;
            this.height = height;
            int frameX = (int)Math.Ceiling((double)width / texture.Width);
            int frameY = (int)Math.Ceiling((double)height / texture.Height);
            tileFrame = new Point(frameX, frameY);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < tileFrame.X; i++)
            {
                for (int j = 0; j < tileFrame.Y; j++)
                {
                    Vector2 pos = new Vector2(position.X + texture.Width * i, position.Y + texture.Height * j);
                    Vector2 screenPos = Level.ScreenPosition(pos);
                    Game.SpriteBatch.Draw(texture,
                        new Rectangle((int)screenPos.X, (int)screenPos.Y, texture.Width, texture.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
                }
            }
        }
    }
}

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
        Texture2D texture;
        TileFrame tileFrame;
        int width;

        public override int CollisionWidth
        {
            get { return tileFrame.Width * texture.Width; }
        }

        public override int CollisionHeight
        {
            get { return tileFrame.Height * texture.Height; }
        }
        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)(position.X),
                    (int)(position.Y), CollisionWidth, CollisionHeight);
            }
        }

        public Tile(Game1 game, Vector2 position, TileFrame tileFrame)
            : base(game, position)
        {
            this.tileFrame = tileFrame;
        }

        public Tile(Game1 game, Vector2 position, int width)
            : base(game, position)
        {
            this.width = width;
        }

        public override void LoadContent()
        {
            this.texture = Game.Content.Load<Texture2D>("texture/Object.Tile.Tile1");
            if (tileFrame == null)
            {
                tileFrame = new TileFrame((int)width / texture.Width + 1, 1);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < tileFrame.Width; i++)
            {
                for (int j = 0; j < tileFrame.Height; j++)
                {
                    Vector2 pos = new Vector2(position.X + texture.Width * i, position.Y + texture.Height * j);
                    Game.SpriteBatch.Draw(this.texture, Level.ScreenPosition(pos), Color.White);
                }
            }
        }
    }

    public class TileFrame
    {
        public int Height
        {
            get { return height; }
        }
        int height;

        public int Width
        {
            get { return width; }
        }
        int width;

        public TileFrame(int width, int height)
        {
            this.height = height;
            this.width = width;
        }
    }
}

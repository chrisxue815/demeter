using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Tile : Object
    {
        Texture2D texture;
        TileFrame tileFrame;
        int width;

        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)(position.X),
                    (int)(position.Y),
                    tileFrame.Width * texture.Width,
                    tileFrame.Height * texture.Height);
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
            this.texture = game.Content.Load<Texture2D>("texture/Object.Tile.Tile1");
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
                    game.spriteBatch.Draw(this.texture, game.level.ScreenPosition(pos), Color.White);
                }
            }
        }

        public override void CollisionResponse(Object obj)
        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Block : Tile
    {
        public Block(Game1 game, Vector2 position, TileFrame tileFrame)
            : base(game, position, tileFrame)
        {
        }

        public Block(Game1 game, Vector2 position, int width)
            : base(game, position, width)
        {
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.Tile.Tile1");
            tileFrame = new TileFrame((int)width / texture.Width + 1, 1);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            if (obj is Player)
            {
                Player player = Level.Player;

                if (player.X + player.CollisionWidth < position.X)
                {
                    Level.Player.CanGoRight = false;
                }
                else if (player.X > position.X + CollisionWidth)
                {
                    Level.Player.CanGoLeft = false;
                }
                else if (player.Y > position.Y + CollisionHeight)
                {
                    Level.Player.CanGoUp = false;
                }
                else if (player.Y < position.Y)
                {
                    Level.Player.CanGoDown = false;
                }
            }
        }
    }
}

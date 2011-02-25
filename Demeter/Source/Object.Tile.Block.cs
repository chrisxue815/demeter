using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Demeter
{
    public class Block : Tile
    {
        public Block(Game1 game, Vector2 position, Point tileFrame)
            : base(game, position, tileFrame)
        {
        }

        public Block(Game1 game, Vector2 position, int width)
            : base(game, position, width)
        {
        }

        public Block(Game1 game, Vector2 position, int width, int height)
            : base(game, position, width, height)
        {
        }

        public Block(Game1 game, XmlTextReader reader)
            : base(game)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");
            string widthStr = reader.GetAttribute("width");
            string heightStr = reader.GetAttribute("height");
            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);
            int width = int.Parse(widthStr);
            int height = int.Parse(heightStr);

            this.game = game;
            this.position = new Vector2(px, py);
            this.Width = width;
            this.Height = height;
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.Tile.Block.Block1");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            if (obj is Player)
            {
                Player player = Level.Player;

                float horizontalDistance1 = Math.Abs(player.X + player.CollisionWidth - this.X);
                float horizontalDistance2 = Math.Abs(this.X + this.CollisionWidth - player.X);
                float verticalDistance1 = Math.Abs(player.Y + player.CollisionHeight - this.Y);
                float verticalDistance2 = Math.Abs(this.Y + this.CollisionHeight - player.Y);

                float horizontalDistance = Math.Min(horizontalDistance1, horizontalDistance2);
                float verticalDistance = Math.Min(verticalDistance1, verticalDistance2);

                if (horizontalDistance < verticalDistance)
                {
                    if (player.X < this.X)
                    {
                        player.CanGoRight = false;
                    }
                    else
                        player.CanGoLeft = false;
                }
                else
                {
                    if (player.Y < this.Y)
                    {
                        player.CanGoDown = false;
                        player.Y = this.Y - player.CollisionHeight + 1;
                    }
                    else
                        player.CanGoUp = false;
                }
            }
        }
    }
}

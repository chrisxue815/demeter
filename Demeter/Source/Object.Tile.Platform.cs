using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Platform : Tile, IControlledObject
    {
        public int MoveSpeed
        {
            get { return 2; }
        }

        public Platform(Game1 game, Vector2 position, Point tileFrame)
            : base(game, position, tileFrame)
        {
        }

        public Platform(Game1 game, Vector2 position, int width)
            : base(game, position, width)
        {
        }

        public Platform(Game1 game, Vector2 position, int width, int height)
            : base(game, position, width, height)
        {
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.Tile.Platform.Platform1");
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void CollisionResponse(Object obj)
        {
            if (obj is Player)
            {
                Player player = Level.Player;

                if ((int)player.PrePosition.Y + player.CollisionHeight <= this.Y)
                {
                    player.CanGoDown = false;
                    player.Y = this.Y - player.CollisionHeight + 1;
                }
            }
        }

        #region IControlledObject Members

        public void Control()
        {
            X += MoveSpeed;
            Level.Player.Speed = Vector2.Zero;
            Level.Player.X += MoveSpeed;
        }

        #endregion
    }
}

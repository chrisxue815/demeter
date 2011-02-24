using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    class Ladder : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 51; }
        }

        public override int CollisionHeight
        {
            get { return height; }
        }
        private int height;

        private int frame;

        public Ladder(Game1 game, Vector2 position, int height)
            :base(game, position)
        {
            this.height = height;
            frame = (int)Math.Ceiling((double)height / texture.Height);
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>(@"texture/Object.StaticObject.Ladder.Ladder1");
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < frame; i++)
            {
                Rectangle screenRect = new Rectangle((int)ScreenPosition.X,
                    (int)ScreenPosition.Y + i * texture.Height, texture.Width, texture.Height);
                Game.SpriteBatch.Draw(texture, screenRect, null, Color.White,
                    0, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
        }

        public override void CollisionResponse(Object obj)
        {
            if (obj is Player)
            {
                Level.Player.LadderUsed = this;
                Level.Player.CollidedWithLadder = true;
            }
        }
    }
}

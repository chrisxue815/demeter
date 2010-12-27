using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public abstract class Sprite : Object
    {
        protected Point frameSize;
        Point sheetSize;
        Point currentFrame;

        static readonly Point DEFAULT_FRAME = Point.Zero;

        Rectangle CurrentSourceRectangle
        {
            get
            {
                return new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y);
            }
        }

        public override Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)(position.X) + collisionOffset,
                    (int)(position.Y) + collisionOffset,
                    frameSize.X - 2 * collisionOffset,
                    frameSize.Y - 2 * collisionOffset);
            }
        }

        public Sprite(Game1 game, Vector2 position,
            Point frameSize, Point sheetSize)
            : base(game, position)
        {
            this.frameSize = frameSize;
            this.sheetSize = sheetSize;
            this.currentFrame = DEFAULT_FRAME;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            if (texture != null)
                game.spriteBatch.Draw(texture, position, CurrentSourceRectangle, Color.White);
        }
    }
}

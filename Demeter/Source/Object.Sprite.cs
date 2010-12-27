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
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        protected Animation currentAnimation;
        public Animation CurrentAnimation
        {
            get { return this.currentAnimation; }
        }

        /// <summary>
        /// The amount of time in milliseconds that the current frame has been shown for.
        /// </summary>
        protected float time;

        /// <summary>
        /// Gets the collision rectangle.
        /// </summary>
        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)(position.X) + collisionOffset,
                    (int)(position.Y) + collisionOffset,
                    currentAnimation.FrameSize.X - 2 * collisionOffset,
                    currentAnimation.FrameSize.Y - 2 * collisionOffset);
            }
        }

        public Sprite(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        public Sprite(Game1 game, Vector2 position,
            int collisionOffset, float scale)
            : base(game, position, collisionOffset, scale)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.Milliseconds;
            while (time > currentAnimation.FrameTime)
            {
                time -= currentAnimation.FrameTime;
                currentAnimation.Update();
            }
        }
    }
}

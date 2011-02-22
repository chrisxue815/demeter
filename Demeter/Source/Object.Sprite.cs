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

        public override int CollisionWidth
        {
            get { return currentAnimation.FrameSize.X - LeftCollisionOffset - RightCollisionOffset; }
        }

        public override int CollisionHeight
        {
            get { return currentAnimation.FrameSize.Y - TopCollisionOffset - BottomCollisionOffset; }
        }
        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)(position.X) + LeftCollisionOffset,
                    (int)(position.Y) + TopCollisionOffset, CollisionWidth, CollisionHeight);
            }
        }

        public Sprite(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        public Sprite(Game1 game, Vector2 position,
            int collisionOffset, float scale)
            : base(game, position, scale)
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

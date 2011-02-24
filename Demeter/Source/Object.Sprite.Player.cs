using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Demeter
{
    public class Player : Sprite
    {
        #region Logical
        public override int CollisionWidth
        {
            get { return 45; }
        }
        public override int CollisionHeight
        {
            get { return 90; }
        }
        #endregion

        #region Drawing

        public override int TopCollisionOffset
        {
            get { return 0; }
        }

        public override int LeftCollisionOffset
        {
            get { return 0; }
        }

        static readonly Point DEFAULT_FRAME_SIZE = new Point(45, 90);

        // animations
        Animation idleAnimation;
        Animation runAnimation;
        Animation jumpAnimation;
        Animation dieAnimation;
        Animation celebrateAnimation;

        #endregion

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 150.0f;
        private const float MaxMoveSpeed = 5.0f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.65f;

        // Constants for controlling vertical movement
        private const float GravityAcceleration = 20.0f;
        private const float MaxFallSpeed = 8.0f;
        private const float jumpStartSpeed = -8f;
        private const float speedOnLadder = 2f;

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        Vector2 speed = Vector2.Zero;

        /// <summary>
        /// Gets whether or not the player is alive
        /// </summary>
        bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        private bool isJumping;

        private bool isLeaving;
        public bool IsLeaving
        {
            get { return isLeaving; }
            set { isLeaving = value; }
        }

        string comingLevel;
        public string ComingLevel
        {
            get { return comingLevel; }
            set { comingLevel = value; }
        }

        #region movement
        bool canGoUp = true;
        public bool CanGoUp
        {
            get { return canGoUp; }
            set { canGoUp = value; }
        }
        bool canGoDown = true;
        public bool CanGoDown
        {
            get { return canGoDown; }
            set { canGoDown = value; }
        }
        bool canGoLeft = true;
        public bool CanGoLeft
        {
            get { return canGoLeft; }
            set { canGoLeft = value; }
        }
        bool canGoRight = true;
        public bool CanGoRight
        {
            get { return canGoRight; }
            set { canGoRight = value; }
        }
        private bool collidedWithLadder;
        public bool CollidedWithLadder
        {
            get { return collidedWithLadder; }
            set { collidedWithLadder = value; }
        }

        private bool collidedWithDoor;
        public bool CollidedWithDoor
        {
            get { return collidedWithDoor; }
            set { collidedWithDoor = value; }
        }
        #endregion

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private int horizontalMovement = 0;
        private int verticalMovement = 0;
        private bool isLadderUsed = false;
        private Ladder ladderUsed;
        internal Ladder LadderUsed
        {
            set { ladderUsed = value; }
        }

        /// <summary>
        /// Gets the sprite effects.
        /// </summary>
        SpriteEffects spriteEffects;
        public SpriteEffects SpriteEffects
        {
            get { return this.spriteEffects; }
        }

        public Player(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        /// <summary>
        /// Load resources
        /// </summary>
        public override void LoadContent()
        {
            idleAnimation = new Animation(Game.Content.Load<Texture2D>("texture/Object.Sprite.Player.Idle"),
                DEFAULT_FRAME_SIZE, 100, true);
            runAnimation = new Animation(Game.Content.Load<Texture2D>("texture/Object.Sprite.Player.Run"),
                DEFAULT_FRAME_SIZE, 100, true);
            jumpAnimation = new Animation(Game.Content.Load<Texture2D>("texture/Object.Sprite.Player.Jump"),
                DEFAULT_FRAME_SIZE, 100, false);
            dieAnimation = new Animation(Game.Content.Load<Texture2D>("texture/Object.Sprite.Player.Die"),
                DEFAULT_FRAME_SIZE, 100, false);
            celebrateAnimation = new Animation(Game.Content.Load<Texture2D>("texture/Object.Sprite.Player.Celebrate"),
                DEFAULT_FRAME_SIZE, 100, false);

            this.currentAnimation = idleAnimation;
            isAlive = true;
        }


        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            GetInput();

            ApplyPhysics(gameTime);

            if (!isAlive)
            {
                this.currentAnimation = dieAnimation;
            }
            else if (speed.Y != 0 && !isLadderUsed)
            {
                this.currentAnimation = jumpAnimation;
            }
            else if (horizontalMovement != 0)
            {
                this.currentAnimation = runAnimation;
            }
            else
            {
                this.currentAnimation = idleAnimation;
            }

            // Clear input.
            horizontalMovement = 0;
            verticalMovement = 0;
            canGoUp = true;
            canGoDown = true;
            canGoLeft = true;
            canGoRight = true;
            collidedWithLadder = false;
            isJumping = false;

            base.Update(gameTime);
        }

        /// <summary>
        /// Get input from the keyboard.
        /// If there is any controll-key pressed, return true.
        /// Otherwise, return false.
        /// </summary>
        public void GetInput()
        {
            bool tryJumping = false;
            bool tryLeaving = false;
            // Get input state.
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {   // player moves left
                horizontalMovement = -1;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {   // player moves right
                horizontalMovement = 1;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {   //player moves up
                verticalMovement = -1;
                tryLeaving = true;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {   //player moves down
                verticalMovement = 1;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {   // player jump
                tryJumping = true;
            }

            if (!canGoLeft && horizontalMovement == -1 || !canGoRight && horizontalMovement == 1)
            {
                horizontalMovement = 0;
            }
            if (verticalMovement != 0 && collidedWithLadder && this.Y >= ladderUsed.Y)
            {
                isLadderUsed = true;
            }
            if (tryJumping && !canGoDown && canGoUp)
            {
                isJumping = true;
            }
            if (!collidedWithLadder || tryJumping || horizontalMovement != 0)
            {
                isLadderUsed = false;
            }
            if (collidedWithDoor && tryLeaving)
            {
                isLeaving = true;
            }
        }

        /// <summary>
        /// Updates the player's speed and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            int elapsed = gameTime.ElapsedGameTime.Milliseconds;

            // Base speed is a combination of horizontal movement control and
            // acceleration downward due to gravity.

            speed.X = horizontalMovement * MoveAcceleration * elapsed / 1000f;

            // Apply pseudo-drag horizontally.
            if (!CanGoDown)
                speed.X *= GroundDragFactor;
            else
                speed.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.
            speed.X = MathHelper.Clamp(speed.X, -MaxMoveSpeed, MaxMoveSpeed);

            if (isLadderUsed)
            {
                if (!canGoDown && verticalMovement == 1 ||
                    !canGoUp && verticalMovement == -1 ||
                    position.Y < ladderUsed.Y && verticalMovement == -1)
                {
                    speed.Y = 0;
                }
                else
                {
                    speed.Y = verticalMovement * speedOnLadder;
                }
                if (verticalMovement == -1 && position.Y + CollisionHeight < ladderUsed.Y)
                {
                    speed.Y = 0;
                }
            }
            else
            {
                speed.Y = MathHelper.Clamp(speed.Y + (GravityAcceleration * elapsed) / 1000f, -MaxFallSpeed, MaxFallSpeed);
                if (!CanGoDown && speed.Y > 0 || !CanGoUp && speed.Y < 0)
                {
                    speed.Y = 0;
                }
            }

            if (isJumping)
            {
                speed.Y = jumpStartSpeed;
            }

            // Out of Border
            if ((speed.X < 0 && position.X < 3) || 
                (speed.X > 0 && position.X > Level.Width - 50))
                speed.X = 0;
            if ((speed.Y < 0 && position.Y < 3) ||
                (speed.Y > 0 && position.Y > Level.Height - 50))
                speed.Y = 0;

            // Apply speed.
            position += speed;
        }

        public override void Draw(GameTime gameTime)
        {
            if (currentAnimation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Flip the sprite to face the way we are moving.
            if (speed.X > 0)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else if (speed.X < 0)
                spriteEffects = SpriteEffects.None;

            // Draw the current frame.
            Game.SpriteBatch.Draw(currentAnimation.Texture, ScreenPosition,
                currentAnimation.CurrentSourceRectangle, Color.White,
                0.0f, currentAnimation.Origin, 1.0f, spriteEffects, 0.9f);

            Game.SpriteBatch.DrawString(Game.font, collidedWithLadder.ToString(),
                Vector2.Zero, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }
}

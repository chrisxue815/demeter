using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Demeter
{
    public class Player : Sprite
    {
        // animations
        Animation idleAnimation;
        Animation runAnimation;
        Animation jumpAnimation;
        Animation dieAnimation;
        Animation celebrateAnimation;

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

        public override float X
        {
            get { return position.X + LeftCollisionOffset; }
        }
        public override float Y
        {
            get { return position.Y + TopCollisionOffset; }
        }
        public override int TopCollisionOffset
        {
            get { return 5; }
        }
        public override int BottomCollisionOffset
        {
            get { return -3; }
        }
        public override int LeftCollisionOffset
        {
            get { return 14; }
        }
        public override int RightCollisionOffset
        {
            get { return 14; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        Vector2 speed;

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
        bool canGoUp;
        public bool CanGoUp
        {
            get { return canGoUp; }
            set { canGoUp = value; }
        }
        bool canGoDown;
        public bool CanGoDown
        {
            get { return canGoDown; }
            set { canGoDown = value; }
        }
        bool canGoLeft;
        public bool CanGoLeft
        {
            get { return canGoLeft; }
            set { canGoLeft = value; }
        }
        bool canGoRight;
        public bool CanGoRight
        {
            get { return canGoRight; }
            set { canGoRight = value; }
        }

        Vector2 prePosition;
        public Vector2 PrePosition
        {
            get { return prePosition; }
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
        private int horizontalMovement;
        private int verticalMovement;
        private bool isLadderUsed;
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

        static readonly Point DEFAULT_FRAME_SIZE = new Point(48, 48);

        public Player(Game1 game, Vector2 position)
            : base(game, position)
        {
            this.speed = Vector2.Zero;
            // initialize collision offset

            this.canGoUp = true;
            this.canGoDown = true;
            this.canGoLeft = true;
            this.canGoRight = true;
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

            if (!canGoRight && horizontalMovement == 1)
            {
                horizontalMovement = 0;
            }
            if (!canGoLeft && horizontalMovement == -1)
            {
                horizontalMovement = 0;
            }
            if (verticalMovement == -1 && collidedWithLadder)
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
                if (!(!canGoDown && verticalMovement == 1))
                {
                    speed.Y = verticalMovement * speedOnLadder;
                }
                else
                {
                    speed.Y = 0;
                }
                if (verticalMovement == -1 && position.Y + CollisionHeight < ladderUsed.Y)
                {
                    speed.Y = 0;
                }
            }
            else
            {
                if (CanGoDown)
                {
                    if (CanGoUp)
                    {
                        speed.Y = MathHelper.Clamp(speed.Y + (GravityAcceleration * elapsed) / 1000f, -MaxFallSpeed, MaxFallSpeed);
                    }
                    else
                    {
                        speed.Y = 0;
                        position = prePosition;
                    }
                }
                else
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
            prePosition = position;
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
                0.0f, currentAnimation.Origin, 1.0f, spriteEffects, 0.0f);
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }
}

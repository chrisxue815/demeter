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
        AnimationPlayer animationPlayer;

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 60.0f;
        private const float MaxMoveSpeed = 8.0f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.65f;

        // Constants for controlling vertical movement
        private const int MaxJumpTime = 350;
        private const float JumpLaunchVelocity = -4.0f;
        private const float GravityAcceleration = 80.0f;
        private const float MaxFallSpeed = 16.0f;
        private const float JumpControlPower = 0.14f;

        // time since player is in the air
        float elapsed;

        /// <summary>
        /// Gets whether or not the player is alive
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private int movement;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private int jumpTime;

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        Vector2 speed;

        static readonly Point DEFAULT_FRAME_SIZE = new Point(48, 48);
        static readonly Point DEFAULT_SHEET_SIZE = new Point(10, 1);

        public Player(Game1 game, Vector2 position)
            : base(game, position, DEFAULT_FRAME_SIZE, DEFAULT_SHEET_SIZE)
        {
            this.speed = Vector2.Zero;
        }
        /// <summary>
        /// load resources
        /// </summary>
        public override void LoadContent()
        {
            idleAnimation = new Animation(Game.Content.Load<Texture2D>("Object.Sprite.Player.Idle"), 100 , true);
            runAnimation = new Animation(Game.Content.Load<Texture2D>("Object.Sprite.Player.Run"), 100, true);
            jumpAnimation = new Animation(Game.Content.Load<Texture2D>("Object.Sprite.Player.Jump"), 100, false);
            dieAnimation = new Animation(Game.Content.Load<Texture2D>("Object.Sprite.Player.Die"), 100, false);
            celebrateAnimation = new Animation(Game.Content.Load<Texture2D>("Object.Sprite.Player.Celebrate"), 100 ,false);
            animationPlayer = new AnimationPlayer();
            animationPlayer.Player = this;

            this.texture = null;
            isOnGround = true;
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

            if (IsAlive && IsOnGround)
            {
                if (speed.X != 0 || Game.Level.BackgroundMove == true)
                {
                    animationPlayer.PlayAnimation(runAnimation);
                }
                else
                {
                    animationPlayer.PlayAnimation(idleAnimation);
                }
            }

            if (Math.Abs(position.Y - 200) < 0.02)
                isOnGround = true;

            // Clear input.
            movement = 0;
           // isJumping = false;
            base.Update(gameTime);
        }

        /// <summary>
        /// get input from the keyboard,if there is any controll-key pressed,return true
        /// otherwise, return false
        /// </summary>
        public void GetInput()
        {
            // Get input state.
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) && Game.Level.BackgroundMove == false)
            {   // player moves left
                movement = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && Game.Level.BackgroundMove == false)
            {   // player moves right
                movement = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Space) ||
                     keyboardState.IsKeyDown(Keys.Up))
            {   // player jump
                isJumping = true;
            }
        }

        /// <summary>
        /// Updates the player's speed and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;

            Vector2 previousPosition = this.position;

            // Base speed is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            speed.X += movement * MoveAcceleration * elapsed / 1000f;
            //speed.Y = MathHelper.Clamp((speed.Y + GravityAcceleration * elapsed) / 1000f, -MaxFallSpeed, MaxFallSpeed);

            //speed.Y = DoJump(speed.Y, gameTime);

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                speed.X *= GroundDragFactor;
            else
                speed.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            //speed.X = MathHelper.Clamp(speed.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply speed.
            position += speed;

            // If the player is now colliding with the level, separate them.
            //HandleCollisions();

            // If the collision stopped us from moving, reset the speed to zero.
            //if (position.X == previousPosition.X)
                //speed.X = 0;

            //if (position.Y == previousPosition.Y)
                //speed.Y = 0;
        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                isOnGround = false;
                // Begin or continue a jump
                if (!wasJumping || jumpTime > 0)
                {
                    //if (jumpTime == 0)
                        //jumpSound.Play();

                    jumpTime += gameTime.ElapsedGameTime.Milliseconds;
                    animationPlayer.PlayAnimation(jumpAnimation);
                }

                // If we are in the ascent of the jump
                if (0 < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical speed with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0;
                    isJumping = false;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        public override void Draw(GameTime gameTime)
        {
            // Flip the sprite to face the way we are moving.
            SpriteEffects flip = new SpriteEffects();
            if (speed.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (speed.X < 0)
                flip = SpriteEffects.None;
            animationPlayer.Draw(gameTime, Game.spriteBatch, position, flip);
        }

        public override void CollisionResponse(Object obj)
        {
            throw new NotImplementedException();
        }
    }
}

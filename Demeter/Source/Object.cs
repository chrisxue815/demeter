using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public abstract class Object
    {
        protected Game1 game;
        public Game1 Game
        {
            get { return this.game; }
        }

        public Level Level
        {
            get { return this.Game.Level; }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
        }

        public virtual float X
        {
            get { return position.X; }
        }

        public virtual float Y
        {
            get { return position.Y; }
        }

        public Vector2 ScreenPosition
        {
            get { return new Vector2(position.X - Level.CameraOffset.X,
                position.Y - Level.CameraOffset.Y); }
        }

        protected float scale;
        protected static readonly float DEFAULT_SCALE = 1.0f;

        public virtual int TopCollisionOffset
        {
            get { return 0; }
        }
        public virtual int BottomCollisionOffset
        {
            get { return 0; }
        }
        public virtual int LeftCollisionOffset
        {
            get { return 0; }
        }
        public virtual int RightCollisionOffset
        {
            get { return 0; }
        }

        public abstract int CollisionWidth { get; }
        public abstract int CollisionHeight { get; }
        public abstract Rectangle CollisionRect { get; }

        public Object(Game1 game)
        {
            this.game = game;
            this.position = new Vector2();
            this.scale = DEFAULT_SCALE;
        }

        public Object(Game1 game, Vector2 position)
        {
            this.game = game;
            this.position = position;
            this.scale = DEFAULT_SCALE;
        }

        public Object(Game1 game, Vector2 position, float scale)
        {
            this.game = game;
            this.position = position;
            this.scale = scale;
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void CollisionResponse(Object obj);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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

        protected Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return this.position; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Vector2 ScreenPosition
        {
            get
            {
                return new Vector2(position.X - Level.CameraOffset.X + TopCollisionOffset,
                    position.Y - Level.CameraOffset.Y + LeftCollisionOffset);
            }
        }

        protected float layerDepth = 0.5f;

        protected float scale = 1.0f;

        public virtual int TopCollisionOffset
        {
            get { return 0; }
        }
        public virtual int LeftCollisionOffset
        {
            get { return 0; }
        }

        public abstract int CollisionWidth { get; }
        public abstract int CollisionHeight { get; }
        public abstract Rectangle CollisionRect { get; }

        public Object(Game1 game)
        {
            this.game = game;
            LoadContent();
        }

        public Object(Game1 game, Vector2 position)
        {
            this.game = game;
            this.position = position;
            LoadContent();
        }

        public Object(Game1 game, Vector2 position, float scale)
        {
            this.game = game;
            this.position = position;
            this.scale = scale;
            LoadContent();
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void CollisionResponse(Object obj);
    }
}

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

        protected Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
        }

        public Vector2 ScreenPosition
        {
            get { return new Vector2(position.X - game.level.OffsetFromOrigin.X,
                position.Y - game.level.OffsetFromOrigin.Y); }
        }

        protected int topCollisionOffset;
        protected int bottomCollisionOffset;
        protected int leftCollisionOffset;
        protected int rightCollisionOffset;

        protected float scale;
        protected static readonly float DEFAULT_SCALE = 1.0f;

        public abstract Rectangle CollisionRect { get; }

        public Object(Game1 game, Vector2 position, float scale)
        {
            this.game = game;
            this.position = position;
            this.scale = scale;
        }

        public Object(Game1 game, Vector2 position)
            : this(game, position, DEFAULT_SCALE)
        {
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void CollisionResponse(Object obj);
    }
}

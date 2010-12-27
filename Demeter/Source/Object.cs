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

        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }
        protected Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
        }
        protected int collisionOffset;
        protected float scale;

        public abstract Rectangle collisionRect { get; }

        static readonly float DEFAULT_SCALE = 1.0f;
        const int DEFAULT_COLLISION_OFFSET = 0;

        public Object(Game1 game, Vector2 position,
            int collisionOffset, float scale)
        {
            this.game = game;
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.scale = scale;
        }

        public Object(Game1 game, Vector2 position)
            : this(game, position, DEFAULT_COLLISION_OFFSET, DEFAULT_SCALE)
        {
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void CollisionResponse(Object obj);
    }
}

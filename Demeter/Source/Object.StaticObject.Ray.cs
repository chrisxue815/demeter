using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Ray : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 0; }
        }
        public override int CollisionHeight
        {
            get { return 0; }
        }
        /// <summary>
        /// The angle between the normal and the x-axis
        /// </summary>
        public float Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        float direction;

        public Ray(Game1 game, Vector2 position, float direction)
            : base(game, position)
        {
            this.direction = direction;
            LoadContent();
        }

        public override void LoadContent()
        {
            Line line = new Line(position, direction);
            texture = new Texture2D(Game.GraphicsDevice, 200, 100);
            Color[] color = new Color[200 * 100];
            for (int i = 0; i < 200 * 100; i++)
                color[i] = new Color(0, 0, 0, 100);
            texture.SetData(color);
        }

        public override void Update(GameTime gameTime)
        {
            Object obj = Level.FindObject(position, direction);
            if (obj != null)
            {
                if (obj is Mirror)
                    ((Mirror)obj).Lighted = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }
}

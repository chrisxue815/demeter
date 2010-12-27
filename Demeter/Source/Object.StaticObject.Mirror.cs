using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    class Mirror : StaticObject, IControlledObject
    {
        float rotation = 0;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
            }
        }

        public Mirror(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            position = pos;
        }

        public override void LoadContent()
        {
            this.texture = Game.Content.Load<Texture2D>("Object.StaticObject.Mirror.Mirror1");
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public void Control()
        {
            this.rotation += 0.05f;
        }

        public override void Draw(GameTime gameTime)
        {
            Game.spriteBatch.Draw(texture,
                new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height),
                null, Color.White, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2),
                SpriteEffects.None, 0);// position, Color.White);
        }

        public override void CollisionResponse(Object obj)
        {
            //throw new NotImplementedException();
        }
    }
}

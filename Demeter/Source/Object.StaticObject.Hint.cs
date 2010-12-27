using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    class Hint : StaticObject
    {
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        Point frameSize;
        //float rotation = 0;
        //public float Rotation
        //{
        //    get { return rotation; }
        //    set
        //    {
        //        rotation = value;
        //    }
        //}
       
        public Hint(Game1 game, Texture2D img, Vector2 pos)
            : base(game, pos)
        {
            texture = img;
            position = pos;
        }
        public void Update(Rectangle clientBounds)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);// new Rectangle((int)position.X, (int)position.Y, 90, 90), null, Color.White, rotation , new Vector2(45, 45), SpriteEffects.None, 0);//
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void CollisionResponse(Object obj)
        {
            throw new NotImplementedException();
        }
    }
}

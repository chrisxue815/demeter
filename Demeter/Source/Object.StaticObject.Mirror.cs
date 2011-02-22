using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        }

        public void Control()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.rotation += 0.05f;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.rotation -= 0.05f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.spriteBatch.Draw(texture,
                new Vector2((int)ScreenPosition.X + HalfWidth, (int)ScreenPosition.Y + HalfHeight),
                null, Color.White, rotation,
                new Vector2(HalfWidth, HalfHeight),
                scale, SpriteEffects.None, 0);
        }

        public override void CollisionResponse(Object obj)
        {
            //throw new NotImplementedException();
        }
    }
}

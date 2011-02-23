using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Demeter
{
    public class Light : StaticObject, IControlledObject
    {
        /// <summary>
        /// The angle between the normal and the x-axis
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        float rotation = 0;

        private const float RotationSpeed = 0.03f;

        public Light(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            position = pos;
        }

        public override void LoadContent()
        {
            this.texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Light.Light1");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(texture,
                new Vector2((int)ScreenPosition.X + HalfWidth, (int)ScreenPosition.Y + HalfHeight),
                null, Color.White, rotation,
                new Vector2(HalfWidth, HalfHeight),
                scale, SpriteEffects.None, 0);
        }

        public override void CollisionResponse(Object obj)
        {
            //throw new NotImplementedException();
        }

        #region IControlledObject Members

        #endregion

        #region IControlledObject Members

        void IControlledObject.Control()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.rotation += RotationSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.rotation -= RotationSpeed;
            }
        }

        #endregion
    }
}

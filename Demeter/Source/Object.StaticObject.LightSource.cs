using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace Demeter
{
    public class LightSource : StaticObject, IControlledObject
    {
        public override int CollisionWidth
        {
            get { return 46; }
        }
        public override int CollisionHeight
        {
            get { return 23; }
        }
        /// <summary>
        /// The angle between the normal and the x-axis
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        float rotation = 0;

        private const float RotationSpeed = 0.01f;

        LightRay lightRay;

        public LightSource(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            this.lightRay = new LightRay(game, pos, rotation);
            this.lightRay.LightSource = this;
        }

        public LightSource(Game1 game, XmlTextReader reader)
            : base(game, reader)
        {
            string pxStr2 = reader.GetAttribute("px");
            string pyStr2 = reader.GetAttribute("py");
            string rotationStr = reader.GetAttribute("rotation");

            float px2 = float.Parse(pxStr2);
            float py2 = float.Parse(pyStr2);
            if (rotationStr != null)
                this.rotation = float.Parse(rotationStr);

            this.game = game;
            this.position = new Vector2(px2, py2);
            this.lightRay = new LightRay(game, position, rotation);
            this.lightRay.LightSource = this;
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Light.Light1");
        }

        public override void Update(GameTime gameTime)
        {
            lightRay.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(texture,
                new Vector2((int)ScreenPosition.X + HalfWidth, (int)ScreenPosition.Y + HalfHeight),
                null, Color.White, rotation,
                new Vector2(HalfWidth, HalfHeight),
                scale, SpriteEffects.None, 1);
            lightRay.Draw(gameTime);
        }

        public override void CollisionResponse(Object obj)
        {
            //throw new NotImplementedException();
        }

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

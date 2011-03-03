using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Xml;

namespace Demeter
{
    class Mirror : StaticObject, IControlledObject
    {
        public override int CollisionWidth
        {
            get { return 400; }
        }
        public override int CollisionHeight
        {
            get { return 400; }
        }
        /// <summary>
        /// The angle between the normal and the x-axis
        /// </summary>
        public float NormalRotation
        {
            get { return normalRotation; }
            set { normalRotation = value; }
        }
        float normalRotation = 0;

        private const float RotationSpeed = 0.003f;

        public Mirror(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            position = pos;
        }

        public Mirror(Game1 game, XmlTextReader reader)
            : base(game, reader)
        {
            string pxStr2 = reader.GetAttribute("px");
            string pyStr2 = reader.GetAttribute("py");
            string normalRotationStr = reader.GetAttribute("normalRotation");

            float px2 = float.Parse(pxStr2);
            float py2 = float.Parse(pyStr2);
            if (normalRotationStr != null)
                this.normalRotation = float.Parse(normalRotationStr);

            this.game = game;
            this.position = new Vector2(px2, py2);
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Mirror.Mirror2");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(texture,
                new Vector2((int)ScreenPosition.X + HalfWidth, (int)ScreenPosition.Y + HalfHeight),
                null, Color.White, normalRotation,
                new Vector2(HalfWidth, HalfHeight),
                scale, SpriteEffects.None, layerDepth);
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
                this.normalRotation += RotationSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.normalRotation -= RotationSpeed;
            }
        }

        #endregion
    }
}

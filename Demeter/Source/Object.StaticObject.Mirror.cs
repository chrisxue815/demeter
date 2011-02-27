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
            get { return 90; }
        }
        public override int CollisionHeight
        {
            get { return 0; }
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

        private const float RotationSpeed = 0.03f;

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
            float px2 = float.Parse(pxStr2);
            float py2 = float.Parse(pyStr2);

            this.game = game;
            this.position = new Vector2(px2, py2);
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Mirror.Mirror1");
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

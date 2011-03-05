﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Demeter
{
    class DeadlyObject : StaticObject
    {
        #region logical
        public override int CollisionWidth
        {
            get { return 150 * frameX; }
        }

        public override int CollisionHeight
        {
            get { return 45; ; }
        }
        #endregion

        int frameX = 1;

        public DeadlyObject(Game1 game, XmlTextReader reader)
            : base(game,reader)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");
            string widthStr = reader.GetAttribute("width");

            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);
            frameX = int.Parse(widthStr) / 150;
            this.position = new Vector2(px, py);
        }
        public override void LoadContent()
        {
            this.texture = game.Content.Load<Texture2D>(@"texture/Object.StaticObject.DeadlyObject");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            if (obj is Player)
            {
                ((Player)obj).IsAlive = false;
            }
            else if (obj is Enemy)
            {
                ((Enemy)obj).IsAlive = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 p = new Vector2(ScreenRectangle.Left, ScreenRectangle.Top);

            for (int i = 0; i < frameX; i++)
            {
                Game.SpriteBatch.Draw(texture, new Rectangle((int)(p.X + i * 150), (int)p.Y, texture.Width, texture.Height),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
        }
    }
}

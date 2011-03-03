using System;
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
            get { return 150; }
        }

        public override int CollisionHeight
        {
            get { return 45; ; }
        }
        #endregion

        public DeadlyObject(Game1 game, XmlTextReader reader)
            : base(game,reader)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");

            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);
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
    }
}

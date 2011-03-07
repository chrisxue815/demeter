using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Demeter
{
    class Treasure : StaticObject
    {
        #region logical
        public override int CollisionWidth
        {
            get { return 50; }
        }

        public override int CollisionHeight
        {
            get { return 50; }
        }
        #endregion


        bool isGotten;
        Texture2D textureOn;
        Texture2D textureOff;

        public Treasure(Game1 game,string id, bool isGotten)
            : base(game)
        {
            this.id = id;
            this.isGotten = isGotten;
        }

        public Treasure(Game1 game, XmlTextReader reader, bool isGotten)
            : base(game, reader)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");
            this.id = reader.GetAttribute("id");

            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);
            this.position = new Vector2(px, py);

            this.isGotten = isGotten;
            if (isGotten)
            {
                this.Texture = textureOff;
            }
            else
            {
                this.Texture = textureOn;
            }
        }

        public override void LoadContent()
        {
            textureOn = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Treasure.Treasure1-On");
            textureOff = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Treasure.Treasure1-Off");
        }
        
        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            if (!isGotten)
            {
                if (obj is Player && ((Player)obj).IsAlive)
                {
                    isGotten = true;
                    Level.TreasureMgr.GetTreasure(this.id);
                    this.Texture = textureOff;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Demeter
{
    public class Door : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 64; }
        }
        public override int CollisionHeight
        {
            get { return 48; }
        }
        string LevelFileName
        {
            get { return levelFileName; }
        }
        string levelFileName;


        public Door(Game1 game, Vector2 position, string levelFileName)
            : base(game, position)
        {
            this.levelFileName = levelFileName;
        }

        public Door(Game1 game, XmlTextReader reader)
            : base(game)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");
            string levelFileNameStr = reader.GetAttribute("levelFileName");
            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);

            this.game = game;
            this.position = new Vector2(px, py);
            this.levelFileName = levelFileNameStr;
        }

        public override void LoadContent()
        {
            this.texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Door.Door1");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            Level.Player.CollidedWithDoor = true;
            Level.Player.ComingLevel = levelFileName;
        }
    }
}

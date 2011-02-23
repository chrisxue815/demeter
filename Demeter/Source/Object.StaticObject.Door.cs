using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Door : StaticObject
    {
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

        public override void LoadContent()
        {
            this.texture = Game.Content.Load<Texture2D>(@"texture/Object.StaticObject.Door.Door1");
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

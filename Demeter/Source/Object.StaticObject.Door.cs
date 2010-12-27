using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class Door : StaticObject, IExit
    {
        #region IExit Members

        string IExit.LevelFileName
        {
            get { return levelFileName; }
        }

        bool IExit.Leave
        {
            get { return leave; }
        }

        #endregion

        string levelFileName;
        bool leave;

        public Door(Game1 game, Vector2 position, string levelFileName)
            : base(game, position)
        {
            this.levelFileName = levelFileName;
        }

        public override void LoadContent()
        {
            this.texture = game.Content.Load<Texture2D>("Object.StaticObject.Door.Door1");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            throw new NotImplementedException();
        }
    }
}

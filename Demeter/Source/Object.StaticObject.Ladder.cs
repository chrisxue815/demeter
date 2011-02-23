using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    class Ladder : StaticObject
    {
        public Ladder(Game1 game, Vector2 position)
            :base(game, position)
        {
        }

        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>(@"texture/Object.StaticObject.Ladder.Ladder1");
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void CollisionResponse(Object obj)
        {
            if (obj is Player)
            {
                Level.Player.LadderUsed = this;
                Level.Player.IsOnLadder = true;
            }
        }
    }
}

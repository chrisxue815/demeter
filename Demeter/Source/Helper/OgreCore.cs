using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Demeter
{
    public class OgreCore : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 45; }
        }

        public override int CollisionHeight
        {
            get { return 70; }
        }
        float maxOffset;
        float offset = 0;
        int bottomTime = 0;
        int topTime = 0;
        const float verticalSpeed = -3.5f;
        bool goUp = true;
        bool move = true;

        public OgreCore(Game1 game, Vector2 position, float Maxoffset,string id)
            : base(game)
        {
            this.game = game;
            this.position = new Vector2(position.X + 18, position.Y + 13);
            this.id = id;
            this.maxOffset = Maxoffset;

            Level.MovableObjects.Add(this);
            LoadContent();
        }

        public override void LoadContent()
        {
            this.texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Ogre.Core");
        }

        public override void Update(GameTime gameTime)
        {
            if (move == false && offset == 0)
            {
                bottomTime += gameTime.ElapsedGameTime.Milliseconds;
                if (bottomTime > 2000)
                {
                    move = true;
                    bottomTime = 0;
                }
            }
            else if (move == false && offset == maxOffset)
            {
                topTime += gameTime.ElapsedGameTime.Milliseconds;
                if (topTime > 500)
                {
                    move = true;
                    topTime = 0;
                }
            }
            if (offset > 0)
            {
                goUp = !goUp;
                offset = 0;
                move = false;
            }
            else if (offset < maxOffset)
            {
                goUp = !goUp;
                offset = maxOffset;
                move = false;
            }

            if (goUp && move)
            {
                position.Y += verticalSpeed;
                offset += verticalSpeed;
            }
            else if (!goUp && move)
            {
                position.Y -= verticalSpeed;
                offset -= verticalSpeed;
            }
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

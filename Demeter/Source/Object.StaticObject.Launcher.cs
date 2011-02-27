using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    class Launcher : StaticObject
    {
        #region logical
        public override int CollisionWidth
        {
            get { return 70; }
        }

        public override int CollisionHeight
        {
            get { return 50; }
        }
        #endregion

        //generate things
        int passingTime = 0;
        int generateTime;
        string generateType;

        Vector2 powerSpeed;

        public Launcher(Game1 game, Vector2 position)
            : base(game, position)
        {
        }

        public Launcher(Game1 game, XmlTextReader reader)
            : base(game, reader)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");
            string generateTimeStr = reader.GetAttribute("generate_time");
            string speedXStr = reader.GetAttribute("speed_x");
            string speedYStr = reader.GetAttribute("speed_y");

            this.generateType = reader.GetAttribute("generate_type");
            this.generateTime = int.Parse(generateTimeStr);
            this.game = game;

            float speedX = float.Parse(speedXStr);
            float speedY = float.Parse(speedYStr);
            this.powerSpeed = new Vector2(speedX, speedY);

            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);
            this.position = new Vector2(px, py);
        }
        public override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Launcher"); ;
        }

        public override void Update(GameTime gameTime)
        {
            passingTime += gameTime.ElapsedGameTime.Milliseconds;

            if (passingTime > generateTime)
            {
                passingTime -= generateTime;
                Generate();
            }
        }

        public override void CollisionResponse(Object obj)
        {
            ;
        }

        public void Generate()
        {
            if (generateType == "enemy")
            {
                Enemy enemy = new Enemy(game, this.position, powerSpeed);
                Level.MovableObjects.Add(enemy);
            }
        }
    }
}

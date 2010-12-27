using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Demeter
{
    class Switch : StaticObject
    {
        bool isON;

        List<IControlledObject> controlled;
        
        Texture2D switchOn;

        public Texture2D SwitchOn
        {
            get { return switchOn; }
            set
            {
                switchOn = value;
            }
        }

        Texture2D switchOff;
        public Texture2D SwitchOff
        {
            get { return switchOff; }
            set
            {
                switchOff = value;
            }
        }
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        Point frameSize;

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)(position.X + 30), (int)(position.Y + 30), 30, 30);
            }
        }

        public Switch(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            position = pos;
            isON = false;
            this.controlled = new List<IControlledObject>();
        }

        public void Update(Rectangle clientBounds)
        {
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public override void LoadContent()
        {
            this.switchOn = Game.Content.Load<Texture2D>("Object.StaticObject.Switch.SwitchOn");
            this.switchOff = Game.Content.Load<Texture2D>("Object.StaticObject.Switch.SwitchOff");
            this.texture = this.switchOff;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void CollisionResponse(Object obj)
        {
            foreach (IControlledObject controlledObj in controlled)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (isON)
                    {
                        this.texture = switchOn;
                    }
                    else
                    {
                        this.texture = switchOff;
                    }
                    isON = !isON;
                    controlledObj.Control();
                }
            }
        }

        public void Add(IControlledObject obj)
        {
            this.controlled.Add(obj);
        }
    }
}

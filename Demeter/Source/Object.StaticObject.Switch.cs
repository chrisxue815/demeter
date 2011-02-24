using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Demeter
{
    class Switch : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 90; }
        }
        public override int CollisionHeight
        {
            get { return 90; }
        }

        List<IControlledObject> controlled;
        
        Texture2D switchOn;
        public Texture2D SwitchOn
        {
            get { return switchOn; }
            set { switchOn = value; }
        }

        Texture2D switchOff;
        public Texture2D SwitchOff
        {
            get { return switchOff; }
            set { switchOff = value; }
        }

        public Switch(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            position = pos;
            this.controlled = new List<IControlledObject>();
        }

        public override void LoadContent()
        {
            switchOn = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Switch.SwitchOn");
            switchOff = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Switch.SwitchOff");
            texture = switchOff;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down))
            {
                texture = switchOn;
                foreach (IControlledObject controlledObj in controlled)
                {
                    controlledObj.Control();
                }
            }
            else
            {
                texture = switchOff;
            }
        }

        public void Add(IControlledObject obj)
        {
            this.controlled.Add(obj);
        }
    }
}

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

        public Switch(Game1 game, Vector2 pos)
            : base(game, pos)
        {
            position = pos;
            this.controlled = new List<IControlledObject>();
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public override void LoadContent()
        {
            this.switchOn = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Switch.SwitchOn");
            this.switchOff = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Switch.SwitchOff");
            this.texture = this.switchOff;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void CollisionResponse(Object obj)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            foreach (IControlledObject controlledObj in controlled)
            {
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down))
                {
                    this.texture = switchOn;
                    controlledObj.Control();
                }
                else
                {
                    this.texture = switchOff;
                }
            }
        }

        public void Add(IControlledObject obj)
        {
            this.controlled.Add(obj);
        }
    }
}

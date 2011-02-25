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
            get { return 45; }
        }
        public override int CollisionHeight
        {
            get { return 70; }
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

        public int MoveSpeed
        {
            get { return 2; }
        }

        private bool one_off;
        private bool moveable;
        private bool canBePressed;
        private int pressCD;

        public Switch(Game1 game, Vector2 pos, bool one_off, bool moveable)
            : base(game, pos)
        {
            position = pos;
            this.controlled = new List<IControlledObject>();
            this.one_off = one_off;
            this.moveable = moveable;
            this.pressCD = 0;
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
            if (texture == SwitchOn)
            {
                foreach (IControlledObject controlledObj in controlled)
                {
                    controlledObj.Control();
                }
                if (moveable)
                {
                    X += MoveSpeed;
                }
            }

            if (one_off)
            {
                texture = switchOff;
            }

            pressCD += gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void CollisionResponse(Object obj)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down))
            {
                if (one_off)
                {
                    texture = switchOn;
                }
            }
            if (pressCD > 100)
            {
                pressCD -= 100;
                canBePressed = true;
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down))
                {
                    if (!one_off)
                    {
                        if (canBePressed)
                        {
                            if (texture == switchOff)
                            {
                                texture = switchOn;
                            }
                            else
                            {
                                texture = switchOff;
                            }
                        }
                    }
                    canBePressed = false;
                }
            }
        }

        public void Add(IControlledObject obj)
        {
            this.controlled.Add(obj);
        }
    }
}

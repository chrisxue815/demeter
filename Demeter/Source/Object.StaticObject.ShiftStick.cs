using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Xml;

namespace Demeter
{
    class ShiftStick : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 45; }
        }
        public override int CollisionHeight
        {
            get { return 70; }
        }

        List<IControlledObject> controlled = new List<IControlledObject>();

        Texture2D switchOnTexture;
        Texture2D switchOffTexture;

        public int MoveSpeed
        {
            get { return 2; }
        }

        private bool canBePressed;
        private int pressCD = 0;

        public ShiftStick(Game1 game, Vector2 pos, bool one_off, bool moveable)
            : base(game, pos)
        {
            position = pos;
        }

        public ShiftStick(Game1 game, XmlTextReader reader)
            : base(game)
        {
            string pxStr = reader.GetAttribute("px");
            string pyStr = reader.GetAttribute("py");
            float px = float.Parse(pxStr);
            float py = float.Parse(pyStr);

            this.game = game;
            this.position = new Vector2(px, py);
        }

        public override void LoadContent()
        {
            switchOnTexture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Switch.SwitchOn");
            switchOffTexture = Game.Content.Load<Texture2D>("texture/Object.StaticObject.Switch.SwitchOff");
            texture = switchOffTexture;
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
                foreach (IControlledObject controlledObj in controlled)
                {
                    controlledObj.Control();
                }
                texture = switchOnTexture;
            }
            else
            {
                texture = switchOffTexture;
            }
        }

        public void Add(IControlledObject obj)
        {
            this.controlled.Add(obj);
        }
    }
}

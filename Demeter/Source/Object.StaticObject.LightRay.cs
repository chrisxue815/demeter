using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demeter
{
    public class LightRay : StaticObject
    {
        public override int CollisionWidth
        {
            get { return 0; }
        }
        public override int CollisionHeight
        {
            get { return 0; }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        float angle;

        LightSource lightSource;
        public LightSource LightSource
        {
            get { return lightSource; }
            set { lightSource = value; }
        }

        List<Vector2> reflectionPosition = new List<Vector2>();

        public LightRay(Game1 game, Vector2 position, float angle)
            : base(game, position)
        {
            this.angle = angle;
        }

        public override void LoadContent()
        {
            texture = new Texture2D(Game.GraphicsDevice, 10, 10);
            Color[] color = new Color[100];
            for (int i = 0; i < 100; i++)
                color[i] = new Color(250, 250, 250, 10);
            texture.SetData(color);
        }

        public override void Update(GameTime gameTime)
        {
            angle = lightSource.Angle;

            reflectionPosition.Clear();
            reflectionPosition.Add(position);

            float incidenceAngle = angle;

            Vector2? collidingPosition;
            Object obj = Level.FindObject(position, angle, out collidingPosition, lightSource);
            while (obj != null)
            {
                if (obj is Mirror)
                {
                    Mirror mirror = (Mirror)obj;
                    reflectionPosition.Add(collidingPosition.Value);
                    float reflectAngle = 2 * mirror.NormalAngle - incidenceAngle - (float)Math.PI;
                    obj = Level.FindObject(collidingPosition.Value, reflectAngle,
                        out collidingPosition, mirror);

                    incidenceAngle = reflectAngle;
                }
                else
                {
                    reflectionPosition.Add(collidingPosition.Value);
                    if (obj is Gate)
                    {
                        ((IControlledObject)obj).Control();
                    }
                    break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            List<Vector2>.Enumerator currentEnum = reflectionPosition.GetEnumerator();
            List<Vector2>.Enumerator nextEnum = reflectionPosition.GetEnumerator();
            nextEnum.MoveNext();
            while(nextEnum.MoveNext() && currentEnum.MoveNext())
            {
                Vector2 current = currentEnum.Current;
                Vector2 next = nextEnum.Current;
                Vector2 currentPoint = Level.ScreenPosition(current);
                Vector2 nextPoint = Level.ScreenPosition(next);
                LineSegment lineSegment = new LineSegment(currentPoint, nextPoint);
                lineSegment.Retrieve(DrawPoint);
            }
        }

        public void DrawPoint(Point point)
        {
            Game.SpriteBatch.Draw(texture, new Rectangle(point.X, point.Y, 10, 10),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }
}

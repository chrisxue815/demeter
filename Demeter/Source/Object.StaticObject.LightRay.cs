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

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        float rotation;

        LightSource lightSource;
        public LightSource LightSource
        {
            get { return lightSource; }
            set { lightSource = value; }
        }

        List<ReflectionPosition> reflectionPosition = new List<ReflectionPosition>();

        public LightRay(Game1 game, Vector2 position, float rotation)
            : base(game, position)
        {
            this.rotation = rotation;
        }

        public override void LoadContent()
        {
            texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            Color[] color = new Color[1];
            for (int i = 0; i < 1; i++)
                color[i] = new Color(0, 0, 0, 100);
            texture.SetData(color);
        }

        public override void Update(GameTime gameTime)
        {
            rotation = lightSource.Rotation;

            reflectionPosition.Clear();
            reflectionPosition.Add(new ReflectionPosition(position, rotation));

            Vector2? collidingPosition;
            Object obj = Level.FindObject(position, rotation, out collidingPosition, lightSource);
            while (obj != null)
            {
                if (obj is Mirror)
                {
                    Mirror mirror = (Mirror)obj;
                    reflectionPosition.Add(new ReflectionPosition(collidingPosition.Value, mirror.NormalRotation));
                    obj = Level.FindObject(collidingPosition.Value,
                        2 * mirror.NormalRotation - rotation - (float)Math.PI, out collidingPosition, mirror);
                }
                else
                {
                    reflectionPosition.Add(new ReflectionPosition(collidingPosition.Value, 0));
                    break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            List<ReflectionPosition>.Enumerator currentEnum = reflectionPosition.GetEnumerator();
            List<ReflectionPosition>.Enumerator nextEnum = reflectionPosition.GetEnumerator();
            nextEnum.MoveNext();
            while(nextEnum.MoveNext() && currentEnum.MoveNext())
            {
                ReflectionPosition current = currentEnum.Current;
                ReflectionPosition next = nextEnum.Current;
                Vector2 currentPoint = Level.ScreenPosition(current.Position);
                Vector2 nextPoint = Level.ScreenPosition(next.Position);
                LineSegment lineSegment = new LineSegment(currentPoint, nextPoint);
                lineSegment.Retrieve(DrawPoint);
            }
        }

        public void DrawPoint(Point point)
        {
            Game.SpriteBatch.Draw(texture, new Rectangle(point.X, point.Y, 1, 1),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public override void CollisionResponse(Object obj)
        {
        }
    }

    public class ReflectionPosition
    {
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        float normalRotation;
        public float NormalRotation
        {
            get { return normalRotation; }
            set { normalRotation = value; }
        }

        public ReflectionPosition(Vector2 position, float normalRotation)
        {
            this.position = position;
            this.normalRotation = normalRotation;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Demeter
{
    public class LineSegment
    {
        Vector2 p1;
        Vector2 p2;

        public LineSegment(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public LineSegment(Vector2 p1, float angle, Rectangle rect)
        {
            this.p1 = p1;
            Ray helperRay = new Ray(p1, angle);
            List<Vector2> intersection = helperRay.Intersects(rect, true);
            this.p2 = intersection.First();
        }

        public List<Vector2> Intersects(Rectangle rect)
        {
            if (p1.X > p2.X)
            {
                Vector2 temp = p1;
                p1 = p2;
                p2 = temp;
            }

            Line helperLine = new Line(p1, p2);
            List<Vector2> intersection1 = helperLine.Intersects(rect);
            List<Vector2> intersection = new List<Vector2>();

            foreach (Vector2 point in intersection1)
            {
                Vector2 p = point;
                if (p.X >= p1.X && p.X <= p2.X)
                {
                    intersection.Add(point);
                }
            }

            return intersection;
        }

        public void Retrieve(Retrieve retrieve)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y < p2.Y)
                {
                    for (int i = (int)p1.Y; i <= (int)p2.Y; i++)
                    {
                        retrieve(new Point((int)p1.X, i));
                    }
                }
                else
                {
                    for (int i = (int)p2.Y; i <= (int)p1.Y; i++)
                    {
                        retrieve(new Point((int)p1.X, i));
                    }
                }
            }
            else
            {
                float k = (p1.Y - p2.Y) / (p1.X - p2.X);
                float b = p1.Y - k * p1.X;
                float angle = (float)Math.Atan((double)k);

                if (angle >= -Math.PI / 4 && angle < Math.PI / 4)
                {
                    if (p1.X < p2.X)
                    {
                        for (int i = (int)p1.X; i <= (int)p2.X; i++)
                        {
                            retrieve(new Point(i, (int)(k * i + b)));
                        }
                    }
                    else
                    {
                        for (int i = (int)p2.X; i <= (int)p1.X; i++)
                        {
                            retrieve(new Point(i, (int)(k * i + b)));
                        }
                    }
                }
                else
                {
                    if (p1.Y < p2.Y)
                    {
                        for (int i = (int)p1.Y; i <= (int)p2.Y; i++)
                        {
                            retrieve(new Point((int)((i - b) / k), i));
                        }
                    }
                    else
                    {
                        for (int i = (int)p2.Y; i <= (int)p1.Y; i++)
                        {
                            retrieve(new Point((int)((i - b) / k), i));
                        }
                    }
                }
            }
        }
    }

    public delegate void Retrieve(Point point);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Demeter
{
    public class Ray
    {
        Vector2 p1;
        float rotation;

        public Ray(Vector2 p1, float rotation)
        {
            this.p1 = p1;
            rotation %= (float)(Math.PI * 2);
            if (rotation < 0)
                rotation += (float)(Math.PI * 2);
            this.rotation = rotation;
        }

        public List<Vector2> Intersects(Rectangle rect)
        {
            Line helperLine = new Line(p1, rotation);
            List<Vector2> intersection1 = helperLine.Intersects(rect);
            List<Vector2> intersection = new List<Vector2>();

            if (rotation >= 0 && rotation < Math.PI / 2 ||
                rotation >= Math.PI / 2 * 3)
            {
                foreach (Vector2 p in intersection1)
                {
                    if (p.X + 0.1 >= p1.X)  //note: 0.1
                        intersection.Add(p);
                }
            }
            else
            {
                foreach (Vector2 p in intersection1)
                {
                    if (p.X - 0.1 <= p1.X)  //note: 0.1
                        intersection.Add(p);
                }
            }

            return intersection;
        }

        public List<Vector2> Intersects(Rectangle rect, bool sorted)
        {
            if (sorted)
            {
                List<Vector2> intersection = Intersects(rect);
                intersection.Sort(VectorComparison);
                return intersection;
            }
            else
            {
                return Intersects(rect);
            }
        }

        public int VectorComparison(Vector2 v1, Vector2 v2)
        {
            if (v1.X == v2.X)
                    return 0;

            if (rotation >= 0 && rotation < Math.PI / 2 ||
                rotation >= Math.PI / 2 * 3)
            {
                if (v1.X < v2.X)
                    return -1;
                else
                    return 1;
            }
            else
            {
                if (v1.X > v2.X)
                    return -1;
                else
                    return 1;
            }
        }
    }
}

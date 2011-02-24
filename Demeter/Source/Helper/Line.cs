using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Demeter
{
    public class Line
    {
        private Point p1;
        private Point p2;

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public Line(Point p1, float direction)
        {
            if (direction == Math.PI / 2 || direction == Math.PI / 2 * 3)
            {
                p2.X = p1.X;
                p2.Y = 0;
            }
            else
            {
                float k = (float)Math.Tan((double)direction);
                float b = p1.Y - k * p1.X;
                p2.X = 0;
                p2.Y = (int)b;
            }
        }

        public Line(Vector2 p1, float direction)
            : this(new Point((int)p1.X, (int)p1.Y), direction)
        {
        }

        public bool Intersects(Rectangle rect)
        {
            if (p1.X == p2.X)
            {
                if (p1.X >= rect.Left && p1.X <= rect.Right)
                    return true;
            }
            else
            {
                float k = (float)(p1.Y - p2.Y) / (p1.X - p2.X);
                float b = p1.Y - k * p1.X;

                float intercept;

                intercept = (rect.Top - b) / k;
                if (intercept >= rect.Left && intercept < rect.Right)
                    return true;

                intercept = (rect.Bottom - b) / k;
                if (intercept >= rect.Left && intercept < rect.Right)
                    return true;

                intercept = k * rect.Left + b;
                if (intercept >= rect.Top && intercept < rect.Bottom)
                    return true;

                intercept = k * rect.Right + b;
                if (intercept >= rect.Top && intercept < rect.Bottom)
                    return true;
            }
            return false;
        }
    }
}

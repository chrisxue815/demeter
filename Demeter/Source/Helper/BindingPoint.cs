using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Demeter
{
    public class BindingPoint
    {
        List<Vector2> totalPoints;
        List<String> totalLevels;

        List<Vector2> points;
        List<String> levels;
        Vector2 binding;

        public BindingPoint()
        {
            points = new List<Vector2>();
            levels = new List<string>();
            totalPoints = new List<Vector2>();
            totalLevels = new List<string>();
        }

        public void Add(Vector2 bindingPoint,String level)
        {
            totalPoints.Add(bindingPoint);
            totalLevels.Add(level);
        }

        public void PassBindingPoint(string currentLevel, Vector2 playerPos)
        {
            for (int i = 0; i < totalLevels.Count; i++)
            {
                if (totalLevels[i] == currentLevel)
                {
                    if (playerPos.X >= totalPoints[i].X)
                    {
                        points.Add(totalPoints[i]);
                        levels.Add(totalLevels[i]);
                    }
                }
            }
        }

        public Vector2 JudgeBindingPoint(string currentLevel , Vector2 playerPos)
        {
            int minDistance = 10000;

            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i] == currentLevel)
                {
                    if (Math.Abs(points[i].X - playerPos.X) < minDistance)
                    {
                        minDistance = (int)Math.Abs(points[i].X - playerPos.X);
                        binding = points[i];
                    }
                }
            }
            return binding;
        }
    }
}

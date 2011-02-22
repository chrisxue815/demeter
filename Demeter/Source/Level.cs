using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace Demeter
{
    public class Level
    {
        #region fields

        string levelFileName;

        Game1 game;
        public Game1 Game
        {
            get { return this.game; }
        }

        Player player;
        public Player Player
        {
            get { return this.player; }
        }
       
        List<Object> Objects;

        string backgroundTextureAssetName;
        Texture2D backgroundTexture;

        int width;
        public int Width
        {
            get { return width; }
        }

        int height;
        public int Height
        {
            get { return height; }
        }
        int groundPositionY;

        Vector2 cameraOffset;
        public Vector2 CameraOffset
        {
            get { return this.cameraOffset; }
        }

        public Vector2 ScreenPosition(Vector2 pos)
        {
            return new Vector2(pos.X - CameraOffset.X,
               pos.Y - CameraOffset.Y);
        }

        const int gridSize = 50;
        GridManager gridManager;

        #endregion

        public Level(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            gridManager = new GridManager(width / gridSize + 1, height / gridSize + 1);
        }

        public void LoadContent()
        {
            backgroundTexture = Game.Content.Load<Texture2D>(backgroundTextureAssetName);
            player.LoadContent();
            foreach (Object obj in Objects)
            {
                obj.LoadContent();
            }
            SetGridManager();
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (player.Position.X > Game.HalfWidth
                && player.Position.X < width - Game.HalfWidth)
            {
                cameraOffset.X = player.Position.X - Game.HalfWidth;
            }
            if (player.Position.Y < Game.HalfHeight
                && player.Position.Y > Game.HalfHeight + Game.Height - height)
            {
                cameraOffset.Y = player.Position.Y - Game.HalfHeight;
            }

            foreach (Object obj in Objects)
            {
                obj.Update(gameTime);
            }

            CollisionDetection();
        }

        public void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            foreach (Object obj in Objects)
            {
                obj.Draw(gameTime);
            }
            player.Draw(gameTime);
            Game.SpriteBatch.End();
        }

        #region xml

        public static Level Load(Game1 game, string levelFileName)
        {
            Level level = new Level(game);
            level.Objects = new List<Object>();

            try
            {
                XmlTextReader reader = new XmlTextReader("Content/level/" + levelFileName);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "size")
                        {
                            string width = reader.GetAttribute("width");
                            string height = reader.GetAttribute("height");
                            level.width = int.Parse(width);
                            level.height = int.Parse(height);
                        }
                        else if (reader.Name == "ground")
                        {
                            string groundPositionY = reader.GetAttribute("py");
                            level.groundPositionY = int.Parse(groundPositionY);
                            level.Objects.Add(
                                new Block(game, new Vector2(0, level.groundPositionY), level.width));
                        }
                        else if (reader.Name == "background")
                        {
                            level.backgroundTextureAssetName = reader.GetAttribute("texture");
                        }
                        else if (reader.Name == "cameraoffset")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            level.cameraOffset = new Vector2(px, py);
                        }
                        else if (reader.Name == "player")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            level.player = new Player(game, new Vector2(px, py));
                        }
                        else if (reader.Name == "switch")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            Switch switch1 = new Switch(game, new Vector2(px, py));
                            XmlReader subtree = reader.ReadSubtree();
                            while (subtree.Read())
                            {
                                if (subtree.NodeType == XmlNodeType.Element && subtree.Name == "mirror")
                                {
                                    string pxStr2 = reader.GetAttribute("px");
                                    string pyStr2 = reader.GetAttribute("py");
                                    float px2 = float.Parse(pxStr2);
                                    float py2 = float.Parse(pyStr2);
                                    Mirror mirror1 = new Mirror(game, new Vector2(px2, py2));
                                    switch1.Add(mirror1);
                                    level.Objects.Add(mirror1);
                                }
                            }
                            level.Objects.Add(switch1);
                        }
                        else if (reader.Name == "ladder")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            Ladder ladder = new Ladder(game, new Vector2(px, py));
                            level.Objects.Add(ladder);
                        }
                        else if (reader.Name == "tile")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            //Tile tile = new Tile(game, new Vector2(px, py));
                            //level.staticObjects.Add(tile);
                        }
                    }
                }
                return level;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Save(string levelFileName)
        {
            return true;
        }

        #endregion

        #region collision detection

        public void SetGridManager()
        {
            foreach (Object obj in Objects)
            {
                Rectangle rect = obj.CollisionRect;
                Point topLeft = new Point(rect.Left, rect.Top);
                Point bottomRight = new Point(rect.Right, rect.Bottom);
                Point topLeftIndex = new Point(topLeft.X / gridSize + 1, topLeft.Y / gridSize + 1);
                Point bottomRightIndex = new Point(bottomRight.X / gridSize + 1, bottomRight.Y / gridSize + 1);

                for (int i = topLeftIndex.X; i <= bottomRightIndex.X; i++)
                {
                    for (int j = topLeftIndex.Y; j <= bottomRightIndex.Y; j++)
                    {
                        gridManager.Add(obj, i, j);
                    }
                }
            }
        }

        public void CollisionDetection()
        {
            Rectangle rect = player.CollisionRect;
            Point topLeft = new Point(rect.Left, rect.Top);
            Point bottomRight = new Point(rect.Right, rect.Bottom);
            Point topLeftIndex = new Point(topLeft.X / gridSize + 1, topLeft.Y / gridSize + 1);
            Point bottomRightIndex = new Point(bottomRight.X / gridSize + 1, bottomRight.Y / gridSize + 1);

            for (int i = topLeftIndex.X; i <= bottomRightIndex.X; i++)
            {
                for (int j = topLeftIndex.Y; j <= bottomRightIndex.Y; j++)
                {
                    try
                    {
                        List<Object> objectInside = gridManager[i, j];
                        foreach (Object obj in objectInside)
                        {
                            if (player.CollisionRect.Intersects(obj.CollisionRect))
                            {
                                obj.CollisionResponse(player);
                                player.CollisionResponse(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        player.IsAlive = false;
                    }
                }
            }
        }

        #endregion
    }

    public class Grid
    {
        private List<Object> objectInside;

        public List<Object> ObjectInside
        {
            get { return objectInside; }
        }

        public Grid()
        {
            objectInside = new List<Object>();
        }

        public void Add(Object obj)
        {
            objectInside.Add(obj);
        }
    }

    public class GridManager
    {
        private Grid[,] manager;
        private int width;
        private int height;

        public List<Object> this[int x, int y]
        {
            get
            {
                return manager[x, y].ObjectInside;
            }
        }

        public GridManager(int width, int height)
        {
            this.width = width;
            this.height = height;

            manager = new Grid[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    manager[i, j] = new Grid();
                }
            }
        }

        public void Add(Object obj, int x, int y)
        {
            if (x < width && y < height)
            {
                manager[x, y].Add(obj);
            }
        }
    }
}

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
       
        List<Object> objects;

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
        List<Object> objectsDetecting;

        #endregion

        public Level(Game1 game)
        {
            this.game = game;
            objects = new List<Object>();
            objectsDetecting = new List<Object>();
        }

        public void Initialize()
        {
            gridManager = new GridManager(width / gridSize + 1, height / gridSize + 1);
        }

        public void LoadContent()
        {
            backgroundTexture = Game.Content.Load<Texture2D>(backgroundTextureAssetName);
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

            foreach (Object obj in objects)
            {
                obj.Update(gameTime);
            }

            CollisionDetection();
        }

        public void Draw(GameTime gameTime)
        {
            /*Game.SpriteBatch.Draw(backgroundTexture,
                new Rectangle(0, 0, Game.Width, Game.Height), null,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);*/
            foreach (Object obj in objects)
            {
                obj.Draw(gameTime);
            }
            player.Draw(gameTime);
        }

        #region xml

        public static Level Load(Game1 game, string levelFileName)
        {
            Level level = new Level(game);
            level.levelFileName = levelFileName;

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
                            level.objects.Add(
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
                            level.objectsDetecting.Add(level.player);
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
                                if (subtree.NodeType == XmlNodeType.Element)
                                {
                                    if (subtree.Name == "light")
                                    {
                                        string pxStr2 = reader.GetAttribute("px");
                                        string pyStr2 = reader.GetAttribute("py");
                                        float px2 = float.Parse(pxStr2);
                                        float py2 = float.Parse(pyStr2);
                                        Light light1 = new Light(game, new Vector2(px2, py2));
                                        switch1.Add(light1);
                                        level.objects.Add(light1);
                                    }
                                    else if (subtree.Name == "mirror")
                                    {
                                        string pxStr2 = reader.GetAttribute("px");
                                        string pyStr2 = reader.GetAttribute("py");
                                        float px2 = float.Parse(pxStr2);
                                        float py2 = float.Parse(pyStr2);
                                        Mirror mirror1 = new Mirror(game, new Vector2(px2, py2));
                                        switch1.Add(mirror1);
                                        level.objects.Add(mirror1);
                                    }
                                }
                            }
                            level.objects.Add(switch1);
                        }
                        else if (reader.Name == "ladder")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            string heightStr = reader.GetAttribute("height");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            int height = int.Parse(heightStr);
                            Ladder ladder = new Ladder(game, new Vector2(px, py), height);
                            level.objects.Add(ladder);
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
                        else if (reader.Name == "door")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            string levelFileNameStr = reader.GetAttribute("levelFileName");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            Door door = new Door(game, new Vector2(px, py), levelFileNameStr);
                            level.objects.Add(door);
                        }
                        else if (reader.Name == "platform" || reader.Name == "block")
                        {
                            string pxStr = reader.GetAttribute("px");
                            string pyStr = reader.GetAttribute("py");
                            string widthStr = reader.GetAttribute("width");
                            string heightStr = reader.GetAttribute("height");
                            float px = float.Parse(pxStr);
                            float py = float.Parse(pyStr);
                            int width = int.Parse(widthStr);
                            int height = int.Parse(heightStr);

                            if (reader.Name == "platform")
                            {
                                Platform platform = new Platform(game, new Vector2(px, py), width, height);
                                level.objects.Add(platform);
                            }
                            else if (reader.Name == "block")
                            {
                                Block block = new Block(game, new Vector2(px, py), width, height);
                                level.objects.Add(block);
                            }
                        }
                    }
                }

                level.Initialize();
                level.LoadContent();

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
            foreach (Object obj in objects)
            {
                Rectangle rect = obj.CollisionRect;
                Point topLeftIndex = new Point(rect.Left / gridSize, rect.Top / gridSize);
                Point bottomRightIndex = new Point(rect.Right / gridSize, rect.Bottom / gridSize);

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
            foreach (Object objDetecting in objectsDetecting)
            {
                Rectangle rect = objDetecting.CollisionRect;
                Point topLeftIndex = new Point(rect.Left / gridSize, rect.Top / gridSize);
                Point bottomRightIndex = new Point(rect.Right / gridSize, rect.Bottom / gridSize);

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
        }

        public Object FindObject(Vector2 pos, float direction)
        {
            Point position = new Point((int)pos.X, (int)pos.Y);
            Line line = new Line(position, direction);

            int x = position.X / gridSize;
            int y = position.Y / gridSize;

            int xIncrement;
            int yIncrement;
            bool isXIncrement;

            if (direction < 0)
                direction = -direction;
            direction = (float)(direction % Math.PI);

            if (direction >= Math.PI / 4 * 7 && direction < Math.PI / 4
                || direction >= Math.PI / 4 * 3 && direction < Math.PI / 4 * 5)
            {
                isXIncrement = true;
            }
            else
                isXIncrement = false;

            if (direction >= Math.PI / 2 * 3 && direction < Math.PI / 2)
            {
                xIncrement = 1;
            }
            else
                xIncrement = -1;

            if (direction >= 0 && direction < Math.PI)
            {
                yIncrement = 1;
            }
            else
                yIncrement = -1;

            while (true)
            {
                List<Object> objs = gridManager[x, y];
                if (objs == null)
                    return null;
                else
                {
                    foreach (Object obj in objs)
                    {
                        if (line.Intersects(obj.CollisionRect))
                        {
                            return obj;
                        }
                    }
                }

                if (isXIncrement)
                    x += xIncrement;
                else
                    y += yIncrement;
                isXIncrement = !isXIncrement;
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
                if (x >= 0 && x < width && y >= 0 && y < height)
                    return manager[x, y].ObjectInside;
                else
                    return null;
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

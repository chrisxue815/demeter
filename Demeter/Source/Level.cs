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
       
        List<Object> objects = new List<Object>();
        public List<Object> Objects
        {
          get { return objects; }
          set { objects = value; }
        }

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
        List<Object> objectsDetecting = new List<Object>();

        #endregion

        public Level(Game1 game)
        {
            this.game = game;
        }

        #region xml
        public Level(Game1 game, string levelFileName)
            : this(game)
        {
            this.levelFileName = levelFileName;

            XmlTextReader reader = new XmlTextReader("Content/level/" + levelFileName);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "size")
                    {
                        string width = reader.GetAttribute("width");
                        string height = reader.GetAttribute("height");
                        this.width = int.Parse(width);
                        this.height = int.Parse(height);
                    }
                    else if (reader.Name == "ground")
                    {
                        string groundPositionY = reader.GetAttribute("py");
                        this.groundPositionY = int.Parse(groundPositionY);
                        this.objects.Add(
                            new Block(game, new Vector2(0, this.groundPositionY), this.width));
                    }
                    else if (reader.Name == "background")
                    {
                        this.backgroundTextureAssetName = reader.GetAttribute("texture");
                    }
                    else if (reader.Name == "cameraoffset")
                    {
                        string pxStr = reader.GetAttribute("px");
                        string pyStr = reader.GetAttribute("py");
                        float px = float.Parse(pxStr);
                        float py = float.Parse(pyStr);
                        this.cameraOffset = new Vector2(px, py);
                    }
                    else if (reader.Name == "player")
                    {
                        player = new Player(game, reader);
                        objectsDetecting.Add(player);
                    }
                    else if (reader.Name == "shiftstick")
                    {
                        ShiftStick stick = new ShiftStick(game, reader);
                        objects.Add(stick);

                        XmlReader subtree = reader.ReadSubtree();
                        while (subtree.Read())
                        {
                            if (subtree.NodeType == XmlNodeType.Element)
                            {
                                if (subtree.Name == "lightsource")
                                {
                                    LightSource light1 = new LightSource(game, reader);
                                    stick.Add(light1);
                                    objects.Add(light1);
                                }
                                else if (subtree.Name == "mirror")
                                {
                                    Mirror mirror1 = new Mirror(game, reader);
                                    stick.Add(mirror1);
                                    objects.Add(mirror1);
                                }
                            }
                        }
                    }
                    else if (reader.Name == "switch")
                    {
                        Switch switch1 = new Switch(game, reader);
                        objects.Add(switch1);

                        XmlReader subtree = reader.ReadSubtree();
                        while (subtree.Read())
                        {
                            if (subtree.NodeType == XmlNodeType.Element)
                            {
                                if (subtree.Name == "block")
                                {
                                    Block block = new Block(game, reader);
                                    switch1.Add(block);
                                    objects.Add(block);
                                }
                            }
                        }
                    }
                    else if (reader.Name == "ladder")
                    {
                        Ladder ladder = new Ladder(game, reader);
                        objects.Add(ladder);
                    }
                    else if (reader.Name == "door")
                    {
                        Door door = new Door(game, reader);
                        objects.Add(door);
                    }
                    else if (reader.Name == "platform")
                    {
                        Platform platform = new Platform(game, reader);
                        objects.Add(platform);
                    }
                    else if (reader.Name == "block")
                    {
                        Block block = new Block(game, reader);
                        objects.Add(block);
                    }
                }
            }

            this.Initialize();
            this.LoadContent();
        }
        #endregion

        public void Initialize()
        {
            SetGridManager();
        }

        public void LoadContent()
        {
            backgroundTexture = Game.Content.Load<Texture2D>(backgroundTextureAssetName);
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

            if (player.CanGoDown)
            {
                player.LastPosition = player.Position;
            }
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

        #region collision detection

        public void SetGridManager()
        {
            gridManager = new GridManager(width / gridSize + 1, height / gridSize + 1);
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

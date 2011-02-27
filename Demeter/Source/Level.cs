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
        public string LevelFileName
        {
            get { return levelFileName; }
        }

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

        List<Object> movableObjects = new List<Object>();
        public List<Object> MovableObjects
        {
            get { return movableObjects; }
            set { movableObjects = value; }
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

        #endregion

        public Level(Game1 game)
        {
            this.game = game;
        }

        #region xml_Load
        public void Load(string levelFileName)
        {
            this.levelFileName = levelFileName;

            List<string> controllerId = new List<string>();
            List<string> controlledId = new List<string>();

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
                    else if (reader.Name == "background")
                    {
                        backgroundTextureAssetName = reader.GetAttribute("texture");
                    }
                    else if (reader.Name == "cameraOffset")
                    {
                        string pxStr = reader.GetAttribute("px");
                        string pyStr = reader.GetAttribute("py");
                        float px = float.Parse(pxStr);
                        float py = float.Parse(pyStr);
                        cameraOffset = new Vector2(px, py);
                    }
                    else if (reader.Name == "ground")
                    {
                        string groundPositionY = reader.GetAttribute("py");
                        this.groundPositionY = int.Parse(groundPositionY);
                        Block ground = new Block(game, new Vector2(0, this.groundPositionY), width);
                        objects.Add(ground);
                    }
                    else if (reader.Name == "player")
                    {
                        player = new Player(game, reader);
                    }
                    else if (reader.Name == "enemy")
                    {
                        Enemy enemy = new Enemy(game, reader);
                    }
                    else if (reader.Name == "shiftStick")
                    {
                        ShiftStick stick = new ShiftStick(game, reader);

                        XmlReader subtree = reader.ReadSubtree();
                        while (subtree.Read())
                        {
                            if (subtree.NodeType == XmlNodeType.Element &&
                                subtree.Name == "controlled")
                            {
                                controllerId.Add(stick.Id);
                                controlledId.Add(subtree.GetAttribute("objId"));
                            }
                        }
                    }
                    else if (reader.Name == "lightsource")
                    {
                        LightSource light1 = new LightSource(game, reader);
                    }
                    else if (reader.Name == "mirror")
                    {
                        Mirror mirror1 = new Mirror(game, reader);
                    }
                    else if (reader.Name == "switch")
                    {
                        Switch switch1 = new Switch(game, reader);

                        XmlReader subtree = reader.ReadSubtree();
                        while (subtree.Read())
                        {
                            if (subtree.NodeType == XmlNodeType.Element &&
                                subtree.Name == "controlled")
                            {
                                controllerId.Add(switch1.Id);
                                controlledId.Add(subtree.GetAttribute("objId"));
                            }
                        }
                    }
                    else if (reader.Name == "ladder")
                    {
                        Ladder ladder = new Ladder(game, reader);
                    }
                    else if (reader.Name == "door")
                    {
                        Door door = new Door(game, reader);
                    }
                    else if (reader.Name == "platform")
                    {
                        Platform platform = new Platform(game, reader);
                    }
                    else if (reader.Name == "block")
                    {
                        Block block = new Block(game, reader);
                    }
                    else if (reader.Name == "launcher")
                    {
                        Launcher launcher = new Launcher(game, reader);
                    }
                }
            }

            List<string>.Enumerator controllerEnum = controllerId.GetEnumerator();
            List<string>.Enumerator controlledEnum = controlledId.GetEnumerator();
            while (controllerEnum.MoveNext() && controlledEnum.MoveNext())
            {
                string controllerStr = controllerEnum.Current;
                string controlledStr = controlledEnum.Current;
                IController controller = (IController)GetObjectById(controllerStr);
                IControlledObject controlled = (IControlledObject)GetObjectById(controlledStr);
                controller.Add(controlled);
            }

            Initialize();
            LoadContent();
        }
        #endregion

        public void Initialize()
        {
            InitializeGridManager();
        }

        public void LoadContent()
        {
            backgroundTexture = Game.Content.Load<Texture2D>(backgroundTextureAssetName);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < movableObjects.Count; i++)
            {
                movableObjects[i].Update(gameTime);
            }
            

            foreach (Object obj in objects)
            {
                obj.Update(gameTime);
            }

            SetCamera();
        }

        private void SetCamera()
        {
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
        }

        public void Draw(GameTime gameTime)
        {
            /*Game.SpriteBatch.Draw(backgroundTexture,
                new Rectangle(0, 0, Game.Width, Game.Height), null,
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);*/

            foreach (Object obj in movableObjects)
            {
                obj.Draw(gameTime);
            }

            foreach (Object obj in objects)
            {
                obj.Draw(gameTime);
            }
        }

        #region collision detection

        public void InitializeGridManager()
        {
            gridManager = new GridManager(width / gridSize + 1, height / gridSize + 1);
            foreach (Object obj in objects)
            {
                Rectangle rect = obj.CollisionRect;
                int leftIndex = rect.Left / gridSize;
                int rightIndex = rect.Right / gridSize;
                int topIndex = rect.Top / gridSize;
                int bottomIndex = rect.Bottom / gridSize;

                for (int i = leftIndex; i <= rightIndex; i++)
                {
                    for (int j = topIndex; j <= bottomIndex; j++)
                    {
                        gridManager.Add(obj, i, j);
                    }
                }
            }
        }

        public List<Object> CollidedWith(Object obj)
        {
            List<Object> collided = new List<Object>();

            Rectangle rect = obj.CollisionRect;
            int leftIndex = rect.Left / gridSize;
            int rightIndex = rect.Right / gridSize;
            int topIndex = rect.Top / gridSize;
            int bottomIndex = rect.Bottom / gridSize;

            for (int i = leftIndex; i <= rightIndex; i++)
            {
                for (int j = topIndex; j <= bottomIndex; j++)
                {
                    try
                    {
                        List<Object> objectsInside = gridManager[i, j];
                        foreach (Object objectInside in objectsInside)
                        {
                            if (obj != objectInside && rect.Intersects(objectInside.CollisionRect))
                            {
                                collided.Add(objectInside);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        player.IsAlive = false;//todo
                    }
                }
            }

            foreach (Object movableObject in movableObjects)
            {
                if(obj!=movableObject && rect.Intersects(movableObject.CollisionRect))
                {
                    collided.Add(movableObject);
                }
            }

            return collided;
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

        public Object GetObjectById(string id)
        {
            foreach (Object obj in objects)
            {
                if (obj.Id == id)
                    return obj;
            }
            foreach (Object obj in movableObjects)
            {
                if (obj.Id == id)
                    return obj;
            }
            return null;
        }
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

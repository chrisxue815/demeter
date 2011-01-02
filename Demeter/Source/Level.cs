using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Demeter
{
    public class Level
    {
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
       
        List<StaticObject> staticObjects;

        Texture2D backgroundTexture;

        int levelWidth;
        int levelHeight;

        TileSize tileSize;

        Vector2 offsetFromOrigin;
        public Vector2 OffsetFromOrigin
        {
            get { return this.offsetFromOrigin; }
        }

        public Level(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            levelWidth = 1500;
            levelHeight = 600;

            player = new Player(game, new Vector2(100, 400));

            Switch switch1 = new Switch(Game, new Vector2(300, 500));
            Mirror mirror1 = new Mirror(Game, new Vector2(400, 500));
            switch1.Add(mirror1);

            staticObjects = new List<StaticObject>();
            staticObjects.Add(switch1);
            staticObjects.Add(mirror1);

            tileSize = new TileSize(48,24);
            Tile[] tiles;
            tiles = new Tile[levelWidth / tileSize.Width + 1];
            for (int i = 0; i < levelWidth / tileSize.Width + 1; i++)
            {
                if (i == 5)
                    continue;
                tiles[i] = new Tile(game, new Vector2(i * tileSize.Width,
                    Game.Window.ClientBounds.Height - tileSize.Height));
                staticObjects.Add(tiles[i]);
            }

            backgroundTexture = Game.Content.Load<Texture2D>("Background.Background6");

            offsetFromOrigin = Vector2.Zero;
        }

        public void LoadContent()
        {
            player.LoadContent();
            foreach (StaticObject obj in staticObjects)
            {
                obj.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (player.Position.X > Game.HalfWidth
                && player.Position.X < levelWidth - Game.HalfWidth)
            {
                offsetFromOrigin.X = player.Position.X - Game.HalfWidth;
            }
            if (player.Position.Y < Game.HalfHeight
                && player.Position.Y > Game.HalfHeight + Game.Height - levelHeight)
            {
                offsetFromOrigin.Y = player.Position.Y - Game.HalfHeight;
            }

            foreach (StaticObject obj in staticObjects)
            {
                obj.Update(gameTime);
            }

            CollisionDetection();
        }

        public void CollisionDetection()
        {
            player.IsOnGround = false;
            foreach (StaticObject obj in staticObjects)
            {
                if (player.CollisionRect.Intersects(obj.CollisionRect))
                {
                    obj.CollisionResponse(player);
                    player.CollisionResponse(obj);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            game.spriteBatch.Begin();
            Game.spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            foreach (StaticObject obj in staticObjects)
            {
                obj.Draw(gameTime);
            }
            player.Draw(gameTime);
            game.spriteBatch.End();
        }

        public static Level Load(string levelFileName)
        {
            return null;
        }

        public bool Save(string levelFileName)
        {
            return false;
        }
    }

    interface IExit
    {
        string LevelFileName
        { get; }
        bool Leave
        { get; }
    }
}

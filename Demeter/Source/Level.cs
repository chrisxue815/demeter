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
        Game1 game;
        public Game1 Game
        {
            get { return this.game; }
        }

        string levelFileName;

        Player player;
        public Player Player
        {
            get { return this.player; }
        }
       
        List<StaticObject> staticObjects;

        Texture2D backgroundTexture;

        bool backgroundMove;
        public bool BackgroundMove
        {
            get { return this.backgroundMove; }
            set { this.backgroundMove = value; }
        }

        int levelWidth;

        public Level(Game1 game)
        {
            this.game = game;
            staticObjects = new List<StaticObject>();
        }

        public void Initialize()
        {
            backgroundTexture = Game.Content.Load<Texture2D>("Background.Background1");
            player = new Player(game, new Vector2(250, 200));
            Switch switch1 = new Switch(Game, new Vector2(300, 200));
            Mirror mirror1 = new Mirror(Game, new Vector2(400, 200));
            switch1.Add(mirror1);
            staticObjects.Add(switch1);
            staticObjects.Add(mirror1);

            backgroundMove = false;
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

            if (player.Position.X > 400 && player.Position.X < levelWidth - 400)
            {
                backgroundMove = true;
            }
            else
            {
                backgroundMove = false;
            }

            foreach (StaticObject obj in staticObjects)
            {
                obj.Update(gameTime);
            }

            CollisionDetection();
        }

        public void CollisionDetection()
        {
            foreach (StaticObject obj in staticObjects)
            {
                if (player.collisionRect.Intersects(obj.collisionRect))
                {
                    obj.CollisionResponse(player);
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

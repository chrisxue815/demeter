using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Collections;

namespace Demeter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }

        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        private Level level;
        public Level Level
        {
            get { return level; }
        }

        int width = 800;
        public int Width
        {
            get { return this.width; }
        }

        int height = 600;
        public int Height
        {
            get { return this.height; }
        }

        public int HalfWidth
        {
            get { return this.width / 2; }
        }

        public int HalfHeight
        {
            get { return this.height / 2; }
        }

        public SpriteFont font;

        private int dieTime;

        #region menu relative
        bool gotoMenu = false;
        bool wasUpKeydown = false;
        bool wasDownKeydown = false;

        int currentSelection = 1;
        const int maxItemsCount = 3;

        Vector2[] menuPos = new Vector2[maxItemsCount];
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            /*if (!graphics.IsFullScreen)
                graphics.ToggleFullScreen();*/

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            for (int i = 0; i < maxItemsCount; i++)
            {
                menuPos[i] = new Vector2(200 , i*50 + 200);
            }
                // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font/Hud");

            level = new Level(this);
            level.Load("level1-1.xml");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gotoMenu = true;
            }

            if (!gotoMenu)
            {
                level.Update(gameTime);

                if (level.Player.IsLeaving)
                {
                    string commingLevel = Level.Player.ComingLevel;
                    level = new Level(this);
                    level.Load(commingLevel);
                }

                if (!level.Player.IsAlive)
                {
                    dieTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (dieTime > level.Player.DieTime)
                    {
                        string current_levelFileName = level.LevelFileName;
                        level = new Level(this);
                        level.Load(current_levelFileName);
                        dieTime = 0;
                    }
                }
            }
            else
            {
                MenuUpdate();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState);
            if (!gotoMenu)
            {
                level.Draw(gameTime);
            }
            else
            {
                MenuDraw();
            }
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void MenuUpdate()
        {
            // select options
            KeyboardState keystate = Keyboard.GetState();
            if (keystate.IsKeyDown(Keys.Down))
            {
                if (!wasDownKeydown)
                {
                    currentSelection++;
                    if (currentSelection > maxItemsCount)
                    {
                        currentSelection = 1;
                    }
                }
                wasDownKeydown = true;
            }
            else
            {
                wasDownKeydown = false;
            }

            if (keystate.IsKeyDown(Keys.Up))
            {
                if (!wasUpKeydown)
                {
                    currentSelection--;
                    if (currentSelection < 1)
                    {
                        currentSelection = maxItemsCount;
                    }
                }
                wasUpKeydown = true;
            }
            else
            {
                wasUpKeydown = false;
            }

            // execute the selectioon
            if (currentSelection == 1 && keystate.IsKeyDown(Keys.Enter))
            {
                gotoMenu = false;
            }
            else if (currentSelection == 2 && keystate.IsKeyDown(Keys.Enter))
            {
                Level.TreasureMgr.ResetLevel();
                gotoMenu = false;
                LoadContent();
            }
            else if (currentSelection == 3 && keystate.IsKeyDown(Keys.Enter))
            {
                this.Exit();
            }
        }

        public void MenuDraw()
        {
            if (currentSelection != 1)
                spriteBatch.DrawString(font, "Resume Game", menuPos[0], Color.White);
            if (currentSelection != 2)
                spriteBatch.DrawString(font, "Restart Game", menuPos[1], Color.White);
            if (currentSelection != 3)
                spriteBatch.DrawString(font, "Leave", menuPos[2], Color.White);

            switch (currentSelection)
            {
                case 1:
                    spriteBatch.DrawString(font, "Resume Game", menuPos[0], Color.Red);
                    break;
                case 2:
                    spriteBatch.DrawString(font, "Restart Game", menuPos[1], Color.Red);
                    break;
                case 3:
                    spriteBatch.DrawString(font, "Leave", menuPos[2], Color.Red);
                    break;
            }
        }
    }
}

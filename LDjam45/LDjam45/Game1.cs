﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LDjam45
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    enum playerType
        {
            gunPlayer,
            swordPlayer,
            flailPlayer
        }
    enum gameState
    {
        menu,
        game,
        pause,
        gameOver
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState kbState;
        KeyboardState prevKbState;
        gameState gState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            gState = gameState.menu;
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

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            prevKbState = kbState;
            kbState = Keyboard.GetState();
            switch (gState)
            {
                case gameState.game:
                    break;
                case gameState.gameOver:
                    break;
                case gameState.menu:
                    if(SingleKeyPress(Keys.Enter))
                    {
                        gState = gameState.game;
                    }
                    break;
                case gameState.pause:
                    break;
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
            spriteBatch.Begin();
            switch (gState)
            {
                case gameState.game:
                    break;
                case gameState.gameOver:
                    break;
                case gameState.menu:
                    
                    break;
                case gameState.pause:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        private bool SingleKeyPress(Keys k)
        {
            if (prevKbState.IsKeyUp(k) && kbState.IsKeyDown(k))
            {
                return true;
            }
            else if (kbState.IsKeyDown(k) && prevKbState == null)
            {
                return true;
            }
            return false;
        }
    }
}

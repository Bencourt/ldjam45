using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

        //fps integer
        const int framesPerSecond = 30;
        int frameCount;

        //spritesheets
        Texture2D GunnerSpritesheet;
        Texture2D FlailSpritesheet;
        Texture2D SwordSpritesheet;

        //characters
        Character player0;
        Character player1;
        //Menu
        Rectangle r1;
        Rectangle r2;
        Rectangle r3;
        MouseState mState;
        bool choice1;
        bool choice2;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //set gamespeed to 30 fps
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1 / framesPerSecond);
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
            gState = gameState.game;
            mState = new MouseState();
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

            //loading spritesheets go here
            GunnerSpritesheet = Content.Load<Texture2D>("GunnerSpritesheet");
            FlailSpritesheet = Content.Load<Texture2D>("FlailSpritesheet");
            SwordSpritesheet = Content.Load<Texture2D>("SwordSpritesheet");
            player0 = new Character(playerType.gunPlayer, 0, new Rectangle(100, 100, 100, 100), GunnerSpritesheet);
            player1 = new Character(playerType.swordPlayer, 1, new Rectangle(400, 100, 100, 100), SwordSpritesheet);
            player0.GetOther(player1);
            player1.GetOther(player0);
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
            System.Console.WriteLine("update");
            // TODO: Add your update logic here
            
            prevKbState = kbState;
            kbState = Keyboard.GetState();
            switch (gState)
            {
                case gameState.game:
                    player1.Update(kbState, frameCount);
                    player0.Update(kbState, frameCount);
                    break;
                case gameState.gameOver:
                    break;
                case gameState.menu:
                    r1 = new Rectangle(100, 100, 100, 100);//Character selection boxes
                    r2 = new Rectangle(200, 100, 100, 100);
                    r3 = new Rectangle(300, 100, 100, 100);
                    if (mState.Y > 100 && mState.Y < 200)//If the mouse is within the y range
                    {
                        if (mState.X > 100 && mState.X < 200)//If the mouse is within the x ranges
                        {
                            if (mState.LeftButton == ButtonState.Pressed)
                            {
                                player0 = new Character(playerType.gunPlayer, 0, new Rectangle(100, 100, 100, 100), GunnerSpritesheet);
                                choice1 = true;//Checks if the players have chosen characters
                            }
                            else if (mState.RightButton == ButtonState.Pressed)
                            {
                                player1 = new Character(playerType.gunPlayer, 1, new Rectangle(400, 100, 100, 100), GunnerSpritesheet);
                                choice2 = true;
                            }
                        }
                        else if (mState.X > 200 && mState.X < 300)
                        {
                            if (mState.LeftButton == ButtonState.Pressed)
                            {
                                player0 = new Character(playerType.swordPlayer, 0, new Rectangle(100, 100, 100, 100), SwordSpritesheet);
                                choice1 = true;
                            }
                            else if (mState.RightButton == ButtonState.Pressed)
                            {
                                player1 = new Character(playerType.swordPlayer, 1, new Rectangle(400, 100, 100, 100), SwordSpritesheet);
                                choice2 = true;
                            }
                        }
                        else if (mState.X > 300 && mState.X < 400)
                        {
                            if (mState.LeftButton == ButtonState.Pressed)
                            {
                                player0 = new Character(playerType.flailPlayer, 0, new Rectangle(100, 100, 100, 100), FlailSpritesheet);
                                choice1 = true;
                            }
                            else if (mState.RightButton == ButtonState.Pressed)
                            {
                                player1 = new Character(playerType.flailPlayer, 1, new Rectangle(400, 100, 100, 100), FlailSpritesheet);
                                choice2 = true;
                            }
                        }
                    }
                    if(SingleKeyPress(Keys.Space))
                    {
                        if (choice1 != true)//If a character has not been chosen, the player is defaulted to gunner
                        {
                            player0 = new Character(playerType.gunPlayer, 0, new Rectangle(100, 100, 100, 100), GunnerSpritesheet);
                        }
                        if (choice2 != true)
                        {
                            player1 = new Character(playerType.gunPlayer, 1, new Rectangle(400, 100, 100, 100), GunnerSpritesheet);
                        }
                        gState = gameState.game;
                    }
                    break;
                case gameState.pause:
                    break;
            }


            frameCount++;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            System.Console.WriteLine("draw");
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (gState)
            {
                case gameState.game:
                    player0.draw(spriteBatch);
                    player1.draw(spriteBatch);
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
                prevKbState = kbState;
                return true;
            }
            else if (kbState.IsKeyDown(k) && prevKbState == null)
            {
                prevKbState = kbState;
                return true;
            }
            prevKbState = kbState;
            return false;
        }
    }
}

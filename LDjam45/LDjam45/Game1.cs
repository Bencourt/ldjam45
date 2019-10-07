using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        Texture2D titleScreen;
        Texture2D pointer;
        SpriteFont Text;

        //characters
        Character player0;
        Character player1;
        //Menu
        Rectangle r1;
        Rectangle r2;
        Rectangle r3;
        Rectangle background;
        Rectangle mouseImage;
        MouseState mState;
        bool choice1;
        bool choice2;

        //sounds
        SoundEffect jumpSound;
        SoundEffect pew1;
        SoundEffect pew2;
        SoundEffect pew3;
        SoundEffect whoosh1;
        SoundEffect whoosh2;
        SoundEffect whoosh3;

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
            gState = gameState.menu;
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

            titleScreen = Content.Load<Texture2D>("playfighting title screen");
            pointer = Content.Load<Texture2D>("pointer");

            Text = Content.Load<SpriteFont>("Text");

            //load sounds here
            jumpSound = Content.Load<SoundEffect>("JumpYelp1");
            pew1 = Content.Load<SoundEffect>("Pew1");
            pew2 = Content.Load<SoundEffect>("Pew2");
            pew3 = Content.Load<SoundEffect>("Pew3");
            whoosh1 = Content.Load<SoundEffect>("Whoosh1");
            whoosh2 = Content.Load<SoundEffect>("Whoosh2");
            whoosh3 = Content.Load<SoundEffect>("Whoosh3");

            //player0 = new Character(playerType.gunPlayer, 0, new Rectangle(100, 100, 100, 100), GunnerSpritesheet);
            //player1 = new Character(playerType.swordPlayer, 1, new Rectangle(400, 100, 100, 100), SwordSpritesheet);
            //player0.GetOther(player1);
            //player1.GetOther(player0);
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

            mState = Mouse.GetState(Window);
            prevKbState = kbState;
            kbState = Keyboard.GetState();
            switch (gState)
            {
                case gameState.game:
                    player0.Update(kbState, frameCount);
                    player1.Update(kbState, frameCount);
                    if (player0.Health <= 0 || player1.Health <= 0)
                        gState = gameState.gameOver;
                    break;
                case gameState.gameOver:
                    break;
                case gameState.menu:
                    r1 = new Rectangle(100, 300, 100, 200);//Character selection boxes
                    r2 = new Rectangle(200, 300, 100, 200);
                    r3 = new Rectangle(300, 300, 100, 200);
                    background = new Rectangle(0, 0, 800, 480);
                    mouseImage = new Rectangle(0, 0, 20, 20);
                    mouseImage.Location = mState.Position;
                    if (mState.Y > 300 && mState.Y < 400)//If the mouse is within the y range
                    {
                        if (mState.X > 400 && mState.X < 500)//If the mouse is within the x ranges
                        {
                            if (mState.LeftButton == ButtonState.Pressed)
                            {
                                player0 = new Character(playerType.gunPlayer, 0, new Rectangle(100, 300, 100, 100), GunnerSpritesheet, pew2, jumpSound);
                                choice1 = true;//Checks if the players have chosen characters
                            }
                            if (mState.RightButton == ButtonState.Pressed)
                            {
                                player1 = new Character(playerType.gunPlayer, 1, new Rectangle(600, 300, 100, 100), GunnerSpritesheet, pew2, jumpSound);
                                choice2 = true;
                            }
                        }
                        else if (mState.X > 500 && mState.X < 600)
                        {
                            if (mState.LeftButton == ButtonState.Pressed)
                            {
                                player0 = new Character(playerType.swordPlayer, 0, new Rectangle(100, 300, 100, 100), SwordSpritesheet, whoosh3, jumpSound);
                                choice1 = true;
                            }
                            if (mState.RightButton == ButtonState.Pressed)
                            {
                                player1 = new Character(playerType.swordPlayer, 1, new Rectangle(600, 300, 100, 100), SwordSpritesheet, whoosh3, jumpSound);
                                choice2 = true;                                                  
                            }                                                                    
                        }                                                                        
                        else if (mState.X > 600 && mState.X < 700)                               
                        {                                                                        
                            if (mState.LeftButton == ButtonState.Pressed)                        
                            {                                                                    
                                player0 = new Character(playerType.flailPlayer, 0, new Rectangle(100, 300, 100, 100), FlailSpritesheet, whoosh2, jumpSound);
                                choice1 = true;                                                  
                            }                                                                    
                            if (mState.RightButton == ButtonState.Pressed)                       
                            {                                                                    
                                player1 = new Character(playerType.flailPlayer, 1, new Rectangle(600, 300, 100, 100), FlailSpritesheet, whoosh2, jumpSound);
                                choice2 = true;
                            }
                        }
                    }
                    if(SingleKeyPress(Keys.Space))
                    {
                        if (choice1 != true)//If a character has not been chosen, the player is defaulted to gunner
                        {
                            player0 = new Character(playerType.gunPlayer, 0, new Rectangle(100, 300, 100, 100), GunnerSpritesheet, pew2, jumpSound);
                        }
                        if (choice2 != true)
                        {
                            player1 = new Character(playerType.gunPlayer, 1, new Rectangle(600, 300, 100, 100), GunnerSpritesheet, pew2, jumpSound);
                        }

                        //set the plyers to their other before game starts
                        player0.GetOther(player1);
                        player1.GetOther(player0);

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
                    spriteBatch.DrawString(Text, "player 1 health: " + player0.Health, new Vector2(20, 20), Color.White);
                    spriteBatch.DrawString(Text, "player 2 health: " + player1.Health, new Vector2(20, 40), Color.White);
                    break;
                case gameState.gameOver:
                    if (player0.Health <= 0)
                        spriteBatch.DrawString(Text, "Player 2 wins!", new Vector2(400, 240), Color.White);
                    else if (player1.Health <= 0)
                        spriteBatch.DrawString(Text, "Player 1 wins!", new Vector2(400, 240), Color.White);
                    else
                        spriteBatch.DrawString(Text, "no one won", new Vector2(400, 240), Color.White);
                    break;
                case gameState.menu:
                    Vector2 textVector;
                    textVector = new Vector2(410, 350);
                    spriteBatch.Draw(titleScreen, background.Location.ToVector2(), Color.White);
                    spriteBatch.Draw(pointer, mouseImage.Location.ToVector2(), Color.White);
                    spriteBatch.DrawString(Text, "Choose your fighter! (left click for player 1, right click for player 2)", new Vector2(300, 300), Color.White);
                    spriteBatch.DrawString(Text, "Press space to start!", new Vector2(300, 400), Color.White);
                    spriteBatch.DrawString(Text, "Player 1 wasd to move, f to attack", new Vector2(300, 420), Color.White);
                    spriteBatch.DrawString(Text, "Player 2 Arrow keys to move, m to attack", new Vector2(300, 440), Color.White);
                    if (mState.X > 400 && mState.X < 500 && mState.Y > 300 && mState.Y < 400) 
                    {
                        spriteBatch.DrawString(Text, "Gun", textVector, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(Text, "Gun", textVector, Color.Black);
                    }
                    textVector = new Vector2(510, 350);
                    if (mState.X > 500 && mState.X < 600 && mState.Y > 300 && mState.Y < 400)
                    {
                        spriteBatch.DrawString(Text, "Sword", textVector, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(Text, "Sword", textVector, Color.Black);
                    }
                    textVector = new Vector2(610, 350);
                    if (mState.X > 600 && mState.X < 700 && mState.Y > 300 && mState.Y < 400)
                    {
                        spriteBatch.DrawString(Text, "Flail", textVector, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(Text, "Flail", textVector, Color.Black);
                    }

                    
                    
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

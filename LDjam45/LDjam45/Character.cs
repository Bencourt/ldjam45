using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LDjam45
{
    enum playerState
    {
        moveState,
        attackState,
        blockState,
        dashState,
        knockBackState
    }

    enum moveFSM
    {
        idle,
        moveRight,
        moveLeft
    }

    enum jumpState
    {
        grounded,
        moveJump,
        falling
    }

    class Character
    {
        #region Fields and properties

        int health;     //player health
        int frameCount;     //total frame counter
        int frameWait;      //frames to wait
        bool frameWaitSet;  //determines if the frame wait was just set or not
        int spdScale;        //movement speed
        float xSpd;         //player x speed
        float ySpd;         //player y speed
        int dir;            //player facing direction
        bool attacked;      //has the player attacked
        bool isInvulnerable; //the player is either blocking or in knockback
        int groundY;


        // Texture and drawing
        Texture2D spriteSheet;  // The single image with all of the animation frames

        // Animation
        int imageWidth;
        int imageHeight;
        int imageYOffset;
        //changed walkframe count to currentFrameCount
        int currentFrameCount;
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        Rectangle playerRectangle;      //player rectangle

        SoundEffect attackSound;
        SoundEffect jumpSound;

        public Rectangle PlayerRectangle        //rectangle property
        {
            get { return playerRectangle; }
        }

        HitboxManager hitBox;           //hitbox manager

        Character Other;                //other character for collisions and hitboxes

        playerType pType;   //player type for finite state machine
        playerState pState; //player state for finite state machine within finite state machine
        moveFSM mState;     //player move state finites state machine
        jumpState jState;   //player jump state finite state machine

        int playerNumber;   //player 0 or player 1, for different control schemes

        public int Health       //health property for changing player health
        {
            get { return health; }
            set { health = value; }
        }

        #endregion

        //character constructor takes game information 
        public Character(playerType pType, int playerNumber, Rectangle playerRectangle, Texture2D spriteSheet, SoundEffect attackSound, SoundEffect jumpSound)
        {
            //setting preliminary values
            health = 20;
            spdScale = 4;
            xSpd = 0.0f;
            ySpd = 0.0f;
            dir = 1;
            attacked = false;
            isInvulnerable = false;
            frameWaitSet = false;
            currentFrameCount = 24;
            //animation preliminary values
            imageHeight = 64;
            imageWidth = 64;
            imageYOffset = 64;
            //twelve for fps looks good for now, can be changed later as needed
            fps = 12;
            timePerFrame = 1 / fps;
            //sound
            this.attackSound = attackSound;
            this.jumpSound = jumpSound;
            
            //set the spritesheet
            this.spriteSheet = spriteSheet;
            //set the rectangle
            this.playerRectangle = playerRectangle;
            //create the hitbox manager
            hitBox = new HitboxManager();
            //set the player type 
            this.pType = pType;

            groundY = playerRectangle.Y;

            //set the preliminary staes
            pState = playerState.moveState;
            mState = moveFSM.idle;
            jState = jumpState.grounded;
            //set the player number
            this.playerNumber = playerNumber;
        }

        //update method takes a keyboard state and frame count from the game1 update method
        public void Update(KeyboardState kbState, int frameCount)
        {
            //setting the kbstate and framecount
            this.frameCount = frameCount;
            kbState = Keyboard.GetState();

            //determine player direction based on the other player's position
            if (playerRectangle.X < Other.playerRectangle.X)
                dir = 1;
            if (playerRectangle.X > Other.playerRectangle.X)
                dir = -1;

            //player state main finite state machine
            switch (pState)
            {
                //move state
                case playerState.moveState:
                    #region PLAYER 0 MOVEMENT
                    if (playerNumber == 0)
                    {
                        //Movement state fsm
                        switch (mState)
                        {
                            //idle state does nothing and can switch between the move states and jumping state
                            case moveFSM.idle:
                                xSpd = 0;
                                if (kbState.IsKeyDown(Keys.A))
                                    //switch to left move 
                                    mState = moveFSM.moveLeft;
                                else if (kbState.IsKeyDown(Keys.D))
                                    //switch to right move
                                    mState = moveFSM.moveRight;

                                if (kbState.IsKeyDown(Keys.W))
                                    //jump if the player is on the ground
                                    if (jState == jumpState.grounded)
                                    {
                                        PlaySound(jumpSound, 1);
                                        jState = jumpState.moveJump;
                                    }
                                if (jState != jumpState.moveJump && jState != jumpState.falling)//Attack, but only on the ground
                                {
                                    if (kbState.IsKeyDown(Keys.F))
                                    {
                                        pState = playerState.attackState;
                                    }
                                }
                                break;

                                //move left state 
                            case moveFSM.moveLeft:
                                if (kbState.IsKeyDown(Keys.A))
                                    //move if the key is still held down
                                    xSpd = -spdScale;
                                else if (kbState.IsKeyDown(Keys.D))
                                    //if the d key is pressed instead, switch to the move right state
                                    mState = moveFSM.moveRight;
                                else
                                    //if nothing is pressed, switch to the idle state
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.W))
                                    //jump if the player is on the ground
                                    if (jState == jumpState.grounded)
                                    {
                                        PlaySound(jumpSound, 1);
                                        jState = jumpState.moveJump;
                                    }
                                if (jState != jumpState.moveJump && jState != jumpState.falling)
                                {
                                    if (kbState.IsKeyDown(Keys.F))
                                    {
                                        pState = playerState.attackState;
                                    }
                                }
                                break;

                                //move right
                            case moveFSM.moveRight:
                                if (kbState.IsKeyDown(Keys.D))
                                    //move right if still pressing right
                                    xSpd = spdScale;
                                else if (kbState.IsKeyDown(Keys.A))
                                    //change to left move state
                                    mState = moveFSM.moveLeft;
                                else
                                    //change to idle state
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.W))
                                    //jump if the player is on the ground
                                    if (jState == jumpState.grounded)
                                    {
                                        PlaySound(jumpSound, 1);
                                        jState = jumpState.moveJump;
                                    }
                                if (jState != jumpState.moveJump && jState != jumpState.falling)
                                {
                                    if (kbState.IsKeyDown(Keys.F))
                                    {
                                        pState = playerState.attackState;
                                    }
                                }                                
                                break;
                        }

                        //jump finite state machine
                        switch (jState)
                        {
                            case jumpState.grounded:
                                //the grounded state makes sure the player does not move
                                ySpd = 0;
                                break;

                            case jumpState.moveJump:
                                //the move jump state sets the player's y velocity to 10
                                ySpd = -10;
                                jState = jumpState.falling;
                                break;

                            case jumpState.falling:
                                //the falling state makes the player slow their jump and then fall
                                if (playerRectangle.Y < groundY)
                                    //if not back to original position, fall
                                    ySpd++;
                                else
                                {
                                    //if back to original position, set speed to 0 and set to grounded state
                                    ySpd = 0;
                                    playerRectangle.Y = groundY;
                                    jState = jumpState.grounded;
                                }
                                break;
                        }
                    }
                    #endregion

                    /*
                     * PLAYER 1 MOVEMENT IS THE EXACT SAME AS PLAYER 0, JUST WITH DIFFERENT BUTTON CHECKS
                     */
                    #region PLAYER 1 MOVEMENT
                    else if (playerNumber == 1)
                    {
                        switch (mState)
                        {
                            case moveFSM.idle:
                                xSpd = 0;
                                if (kbState.IsKeyDown(Keys.Left))
                                    mState = moveFSM.moveLeft;
                                else if (kbState.IsKeyDown(Keys.Right))
                                    mState = moveFSM.moveRight;

                                if (kbState.IsKeyDown(Keys.Up))
                                    if (jState == jumpState.grounded)
                                    {
                                        PlaySound(jumpSound, 1);
                                        jState = jumpState.moveJump;
                                    }
                                if (jState != jumpState.moveJump && jState != jumpState.falling)
                                {
                                    if (kbState.IsKeyDown(Keys.M))
                                    {
                                        pState = playerState.attackState;
                                    }
                                }
                                break;

                            case moveFSM.moveLeft:
                                if (kbState.IsKeyDown(Keys.Left))
                                    xSpd = -spdScale;
                                else if (kbState.IsKeyDown(Keys.Right))
                                    mState = moveFSM.moveRight;
                                else
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.Up))
                                    if (jState == jumpState.grounded)
                                    {
                                        PlaySound(jumpSound, 1);
                                        jState = jumpState.moveJump;
                                    }
                                if (jState != jumpState.moveJump && jState != jumpState.falling)
                                {
                                    if (kbState.IsKeyDown(Keys.M))
                                    {
                                        pState = playerState.attackState;
                                    }
                                }

                                if (playerRectangle.X < 20)
                                    xSpd = 0;
                                break;

                            case moveFSM.moveRight:
                                if (kbState.IsKeyDown(Keys.Right))
                                    xSpd = spdScale;
                                else if (kbState.IsKeyDown(Keys.Left))
                                    mState = moveFSM.moveLeft;
                                else
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.Up))
                                    if (jState == jumpState.grounded)
                                    {
                                        PlaySound(jumpSound, 1);
                                        jState = jumpState.moveJump;
                                    }
                                if (jState != jumpState.moveJump && jState != jumpState.falling)
                                {
                                    if (kbState.IsKeyDown(Keys.M))
                                    {
                                        pState = playerState.attackState;
                                    }
                                }

                                if(playerRectangle.X > 700)
                                {
                                    xSpd = 0;
                                }
                                break;

                        }
                        switch (jState)
                        {
                            case jumpState.grounded:
                                //the grounded state makes sure the player does not move
                                ySpd = 0;
                                break;

                            case jumpState.moveJump:
                                //the move jump state sets the player's y velocity to 10
                                ySpd = -10;
                                jState = jumpState.falling;
                                break;

                            case jumpState.falling:
                                //the falling state makes the player slow their jump and then fall
                                if (playerRectangle.Y < groundY)
                                    //if not back to original position, fall
                                    ySpd++;
                                else
                                {
                                    //if back to original position, set speed to 0 and set to grounded state
                                    ySpd = 0;
                                    playerRectangle.Y = groundY;
                                    jState = jumpState.grounded;
                                }
                                break;
                        }
                    }
                    #endregion
                    break;

                    //attack state
                case playerState.attackState:
                    #region PLAYER ATTACK
                    //if the player has not attacked
                    if (!attacked)
                    {
                        //player type fsm
                        switch (pType)
                        {
                            //sword player attack with arbitrary values
                            case playerType.swordPlayer:
                                PlaySound(attackSound, 1);
                                if(dir<0)
                                    hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (10 * dir) + (20 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(20, 20));
                                else
                                    hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (10 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(20, 20));
                                if (hitBox.CheckCollision(Other))
                                    Other.TakeDamage();
                                hitBox.DeleteHitbox();
                                break;

                                //flail player, arbitrary values
                            case playerType.flailPlayer:
                                PlaySound(attackSound, 1);
                                if (dir < 0)
                                    hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (150 * dir) + (30 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(30, 30));
                                else
                                    hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (150 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(30, 30)); if (hitBox.CheckCollision(Other))
                                    Other.TakeDamage();
                                hitBox.DeleteHitbox();
                                break;

                                //gun player arbitrary values
                            case playerType.gunPlayer:
                                PlaySound(attackSound, 1);
                                if (dir < 0)
                                    hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (10 * dir) + (500 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(500, 5));
                                else
                                    hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (10 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(500, 5)); if (hitBox.CheckCollision(Other))
                                    Other.TakeDamage();
                                hitBox.DeleteHitbox();
                                break;
                        }
                        //set attacked to true and set the number of frames to wait
                        attacked = true;

                    }
                    //set the frame timer
                    if (!frameWaitSet)
                    {
                        if(pType == playerType.gunPlayer)
                        {
                            frameWait = 16;
                        }
                        else if (pType == playerType.swordPlayer)
                        {
                            frameWait = 30;
                        }
                        else if (pType == playerType.flailPlayer)
                        {
                            frameWait = 48;
                        }
                        frameWaitSet = true;
                    }
                    //if the player has waited enough, set the player state back to move and reset the ability to attack
                    if (frameWait == 0)
                    {
                        attacked = false;
                        frameWaitSet = false;
                        pState = playerState.moveState;
                    }
                    else
                    {
                        //wait one less frame
                        frameWait--;
                    }
                    #endregion
                    break;

                    //block state
                case playerState.blockState:
                    if (kbState.IsKeyDown(Keys.B))
                    {
                        pState = playerState.blockState;
                        isInvulnerable = true;
                    }
                    else
                    {
                        pState = playerState.moveState;
                        isInvulnerable = false;
                    }
                    break;

                    //dash state (?)
                case playerState.dashState:
                    break;

                    //knockback state
                case playerState.knockBackState:
                    isInvulnerable = true;
                    if(!frameWaitSet)
                    {
                        frameWaitSet = true;

                        frameWait = 4;
                    }
                    if(frameWait > 0)
                    {
                        frameWait--;
                    }
                    else
                    {
                        if(playerRectangle.Y != Other.PlayerRectangle.Y)
                        {
                            ySpd = -1;
                            jState = jumpState.falling;
                        }
                        isInvulnerable = false;
                        frameWait = 4;
                        frameWaitSet = false;
                        pState = playerState.moveState;
                    }
                    break;
            }

            //update the player rectangle based on the fsm code
            playerRectangle.X += (int)xSpd;
            playerRectangle.Y += (int)ySpd;

        }

        public void UpdateAnimation()
        {
            //GameTime gameTime;
            //switch statement to determine what the YOffset should be
            //should just be based of the player state
            //change the walk frame count based on what state we are in (some frames have less counts)
            switch (pState)
            {
                case playerState.moveState:
                    if (mState != moveFSM.idle)
                    {
                        imageYOffset = imageHeight;
                        currentFrameCount = 15;
                    }
                    else
                    {
                        imageYOffset = 0*imageHeight;
                        currentFrameCount = 23;
                    }
                    break;
                case playerState.attackState:
                    xSpd = 0;
                    imageYOffset = 4 * imageHeight;
                    currentFrameCount = 17;
                    break;
                case playerState.blockState:
                    imageYOffset = 3 * imageHeight;
                    currentFrameCount = 17;
                    break;
                case playerState.knockBackState:
                    imageYOffset = 5 * imageHeight;
                    currentFrameCount = 7;
                    break;
            }

            frame += 1;                     // Adjust the frame to the next image

            if (frame > currentFrameCount)     // Check the bounds - have we reached the end of animation cycle?
                frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed? 
            /*
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame > currentFrameCount)     // Check the bounds - have we reached the end of animation cycle?
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
            */
        }

        //draw method
        public void draw(SpriteBatch sb)
        {
            SpriteEffects flipImage;
            Color imageColor;

            imageColor = Color.White;
            flipImage = SpriteEffects.None;

            if (dir<0)
            {
                flipImage = SpriteEffects.FlipHorizontally;
            }
            else
            {
                flipImage = SpriteEffects.None;
            }


            if (pState == playerState.knockBackState)
            {
                imageColor = Color.Red;
            }
            else
                imageColor = Color.White;

            UpdateAnimation();

            sb.Draw(
                    spriteSheet,                                            // - The texture to draw
                    playerRectangle.Location.ToVector2(),                   // - The location to draw on the screen
                    new Rectangle(                                          // - The "source" rectangle
                        new Point(frame * imageWidth,                       //   - This rectangle specifies
                        imageYOffset),                                      //	   where "inside" the texture
                        new Point(imageWidth,                               //     to get pixels (We don't want to
                        imageHeight)),                                      //     draw the whole thing)
                    imageColor,                                             // - The color
                    0,                                                      // - Rotation (none currently)
                    Vector2.Zero,                                           // - Origin inside the image (top left)
                    1.0f,                                                   // - Scale (100% - no change)
                    flipImage,                                              // - Can be used to flip the image
                    0);                                                     // - Layer depth (unused)
        }

        public void TakeDamage()
        {
            if (!isInvulnerable)
            {

                switch (Other.pType)
                {
                    case playerType.flailPlayer:
                        health -= 3;
                        break;

                    case playerType.swordPlayer:
                        health -= 2;
                        break;

                    case playerType.gunPlayer:
                        health--;
                        break;
                }
                pState = playerState.knockBackState;
            }
        }
        public void GetOther(Character Other)
        {
            //set the other player
            this.Other = Other;
        }

        private void PlaySound(SoundEffect sound, double i)
        {
            
            sound.CreateInstance().Play();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        int spdScale;        //movement speed
        float xSpd;         //player x speed
        float ySpd;         //player y speed
        int dir;            //player facing direction
        bool attacked;      //has the player attacked

        Rectangle playerRectangle;      //player rectangle

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
        public Character(playerType pType, int playerNumber, Rectangle playerRectangle, Character Other)
        {
            //setting preliminary values
            health = 20;
            spdScale = 5;
            xSpd = 0.0f;
            ySpd = 0.0f;
            dir = 1;
            attacked = false;

            //set the rectangle
            this.playerRectangle = playerRectangle;
            //create the hitbox manager
            hitBox = new HitboxManager();
            //set the other player
            this.Other = Other;
            //set the player type 
            this.pType = pType;

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
                                if (kbState.IsKeyDown(Keys.A))
                                    //switch to left move 
                                    mState = moveFSM.moveLeft;
                                else if (kbState.IsKeyDown(Keys.D))
                                    //switch to right move
                                    mState = moveFSM.moveRight;

                                if (kbState.IsKeyDown(Keys.W))
                                    //jump if the player is on the ground
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;
                                break;

                                //move left state 
                            case moveFSM.moveLeft:
                                if (kbState.IsKeyDown(Keys.A))
                                    //move if the key is still held down
                                    xSpd = spdScale;
                                else if (kbState.IsKeyDown(Keys.D))
                                    //if the d key is pressed instead, switch to the move right state
                                    mState = moveFSM.moveRight;
                                else
                                    //if nothing is pressed, switch to the idle state
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.W))
                                    //jump if the player is on the ground
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;
                                break;

                                //move right
                            case moveFSM.moveRight:
                                if (kbState.IsKeyDown(Keys.D))
                                    //move right if still pressing right
                                    xSpd = -spdScale;
                                else if (kbState.IsKeyDown(Keys.A))
                                    //change to left move state
                                    mState = moveFSM.moveLeft;
                                else
                                    //change to idle state
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.W))
                                    //jump if the player is on the ground
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;
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
                                if (ySpd != 10)
                                    //if not back to original position, fall
                                    ySpd--;
                                else
                                {
                                    //if back to original position, set speed to 0 and set to grounded state
                                    ySpd = 0;
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
                                if (kbState.IsKeyDown(Keys.Left))
                                    mState = moveFSM.moveLeft;
                                else if (kbState.IsKeyDown(Keys.Right))
                                    mState = moveFSM.moveRight;

                                if (kbState.IsKeyDown(Keys.Up))
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;

                                break;

                            case moveFSM.moveLeft:
                                if (kbState.IsKeyDown(Keys.Left))
                                    xSpd = spdScale;
                                else if (kbState.IsKeyDown(Keys.Right))
                                    mState = moveFSM.moveRight;
                                else
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.Up))
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;
                                break;

                            case moveFSM.moveRight:
                                if (kbState.IsKeyDown(Keys.Right))
                                    xSpd = -spdScale;
                                else if (kbState.IsKeyDown(Keys.Left))
                                    mState = moveFSM.moveLeft;
                                else
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.Up))
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;
                                break;

                        }
                        switch (jState)
                        {
                            case jumpState.grounded:
                                ySpd = 0;
                                break;

                            case jumpState.moveJump:
                                ySpd = -10;
                                jState = jumpState.falling;
                                break;

                            case jumpState.falling:
                                if (ySpd != 10)
                                    ySpd--;
                                else
                                {
                                    ySpd = 0;
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
                                hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (10 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(20, 20));
                                hitBox.CheckCollision(Other);
                                hitBox.DeleteHitbox();
                                break;

                                //flail player, arbitrary values
                            case playerType.flailPlayer:
                                hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (50 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(40, 40));
                                hitBox.CheckCollision(Other);
                                hitBox.DeleteHitbox();
                                break;

                                //gun player arbitrary values
                            case playerType.gunPlayer:
                                hitBox.ActivateHitbox(new Point(playerRectangle.X + playerRectangle.Width / 2 + (10 * dir), playerRectangle.Y + playerRectangle.Height / 2), new Point(20, 20));
                                hitBox.CheckCollision(Other);
                                hitBox.DeleteHitbox();
                                break;
                        }
                        //set attacked to true and set the number of frames to wait
                        attacked = true;
                        frameWait = 4;
                    }
                    //if the player has waited enough, set the player state back to move and reset the ability to attack
                    if(frameWait == 0)
                    {
                        attacked = false;
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
                    break;

                    //dash state (?)
                case playerState.dashState:
                    break;

                    //knockback state
                case playerState.knockBackState:
                    break;
            }

            //update the player rectangle based on the fsm code
            playerRectangle.X += (int)xSpd;
            playerRectangle.Y += (int)ySpd;

        }

        //draw method
        public void draw(SpriteBatch sb)
        {

        }
    }
}

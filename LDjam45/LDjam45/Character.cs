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

        int health;

        int spdScale;        //movement speed
        float xSpd;
        float ySpd;
        int dir;

        Rectangle playerRectangle;

        public Rectangle PlayerRectangle
        {
            get { return playerRectangle; }
        }

        HitboxManager hitBox;

        Character Other;

        playerType pType;   //player type for finite state machine
        playerState pState; //player state for finite state machine within finite state machine
        moveFSM mState;
        jumpState jState;

        int playerNumber;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        #endregion
        public Character(playerType pType, int playerNumber, Rectangle playerRectangle, Character Other)
        {
            health = 20;
            spdScale = 5;
            xSpd = 0.0f;
            ySpd = 0.0f;
            dir = 1;

            this.playerRectangle = playerRectangle;

            hitBox = new HitboxManager();

            this.Other = Other;

            this.pType = pType;
            pState = playerState.moveState;
            mState = moveFSM.idle;
            jState = jumpState.grounded;
            this.playerNumber = playerNumber;
        }

        public void Update(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();

            if (playerRectangle.X < Other.playerRectangle.X)
                dir = 1;
            if (playerRectangle.X > Other.playerRectangle.X)
                dir = -1;

            switch (pState)
            {
                case playerState.moveState:
                    #region PLAYER 0 MOVEMENT
                    if (playerNumber == 0)
                    {
                        switch (mState)
                        {
                            case moveFSM.idle:
                                if (kbState.IsKeyDown(Keys.A))
                                    mState = moveFSM.moveLeft;
                                else if (kbState.IsKeyDown(Keys.D))
                                    mState = moveFSM.moveRight;

                                if (kbState.IsKeyDown(Keys.W))
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;

                                break;

                            case moveFSM.moveLeft:
                                if (kbState.IsKeyDown(Keys.A))
                                    xSpd = spdScale;
                                else if (kbState.IsKeyDown(Keys.D))
                                    mState = moveFSM.moveRight;
                                else
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.W))
                                    if (jState == jumpState.grounded)
                                        jState = jumpState.moveJump;
                                break;

                            case moveFSM.moveRight:
                                if (kbState.IsKeyDown(Keys.D))
                                    xSpd = -spdScale;
                                else if (kbState.IsKeyDown(Keys.A))
                                    mState = moveFSM.moveLeft;
                                else
                                    mState = moveFSM.idle;

                                if (kbState.IsKeyDown(Keys.W))
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

                case playerState.attackState:
                    #region PLAYER 0 ATTACK
                    switch (pType)
                    {
                        case playerType.swordPlayer:
                            hitBox.ActivateHitbox(new Point(playerRectangle.Width/2, playerRectangle.Height/2), new Point(20, 20));
                            hitBox.CheckCollision(Other);
                            hitBox.DeleteHitbox();
                            break;

                        case playerType.flailPlayer:
                            hitBox.ActivateHitbox(new Point(playerRectangle.Width / 2, playerRectangle.Height / 2), new Point(20, 20));
                            hitBox.CheckCollision(Other);
                            hitBox.DeleteHitbox();
                            break;

                        case playerType.gunPlayer:
                            break;
                    }
                    #endregion

                    #region PLAYER 1 ATTACK
                    #endregion
                    break;

                case playerState.blockState:
                    break;

                case playerState.dashState:
                    break;

                case playerState.knockBackState:
                    break;
            }

            playerRectangle.X += (int)xSpd;
            playerRectangle.Y += (int)ySpd;

        }

        public void draw(SpriteBatch sb)
        {

        }

        public bool timing(double frames, GameTime gameTime)
        {
            double t = gameTime.ElapsedGameTime.TotalSeconds;
            frames -= t;
            if (frames <= 0)
                return true;
            else
                return false;
        }
    }
}

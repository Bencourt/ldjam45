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

    }

    class Character
    {
        #region Fields

        int health;
        int spd;        //movement speed
        playerType pType;   //player type for finite state machine
        playerState pState; //player state for finite state machine within finite state machine

        #endregion
        public Character(playerType pType)
        {
            this.pType = pType;
            pState = playerState.moveState;
        }

        public void Update(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();

            switch(pState)
            {
                case playerState.moveState:

                    break;

                case playerState.attackState:
                    break;

                case playerState.blockState:
                    break;

                case playerState.dashState:
                    break;

                case playerState.knockBackState:
                    break;
            }

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

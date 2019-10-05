using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using System.Drawing;

namespace LDjam45
{
    class HitboxManager
    {
        Rectangle hitBox;
        List<Rectangle> hitBoxActive;
        List<Rectangle> hitBoxInactive;

        public void ActivateHitbox(Rectangle hitbox, SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, hitbox,Color.White);//Fix
        }
        public void CheckCollision(Rectangle hitbox,Rectangle cRect,Character cObj)
        {
            if (hitbox.Intersects(cRect)==true)
            {
                cObj.health -= 1;
            }
        }
        public void DeleteHitbox(Rectangle hitbox)
        {
            hitbox.X = -50;
        }
    }
}

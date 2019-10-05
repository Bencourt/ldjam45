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
        Rectangle hitBox = new Rectangle();

        public HitboxManager()
        {
        }

        public void ActivateHitbox(Point location, Point scale)
        {
            //spriteBatch.Draw(texture, hitbox,Color.White);
            hitBox.Location = location;
            hitBox.Size = scale;
        }
        public bool CheckCollision(Character cObj)
        {
            if (hitBox.Intersects(cObj.PlayerRectangle) == true)
            {
                return true;
            }
            else
                return false;
        }
        public void DeleteHitbox()
        {
            hitBox.X = -50;
            hitBox.Size = new Point(2, 2);
        }
    }
}

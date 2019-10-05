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
        //the hitbox
        Rectangle hitBox = new Rectangle();

        public HitboxManager()
        {
            //nothing here
        }

        public void ActivateHitbox(Point location, Point scale)
        {
            //set the location and size of the hitbox
            hitBox.Location = location;
            hitBox.Size = scale;
        }
        public bool CheckCollision(Character cObj)
        {
            //determine if the other player was hit
            if (hitBox.Intersects(cObj.PlayerRectangle) == true)
            {
                return true;
            }
            else
                return false;
        }

        //remove the hit box
        public void DeleteHitbox()
        {
            hitBox.X = -50;
            hitBox.Size = new Point(2, 2);
        }
    }
}

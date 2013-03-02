/*
 * MoveableEntity: base class for every drawable in the game that has movement
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{
    public class MoveableEntity :GameEntity
    {

        protected Vector3 modelVelocity, up,right, look;
        protected float spinX;
        protected float spinY;
        protected float spinZ;
        protected bool alive;
        protected float health;


        public MoveableEntity()
        {
            right = new Vector3(0, 0, 1);
            up = new Vector3(0, 1, 0);
            look = new Vector3(0, 0, -1);
            spinX = 0.0f;
            spinY = 0.0f;
            spinZ = 0.0f;
            alive = true;
            
        }

        public bool isAlive()
        {
            return alive;
        }

    }
}

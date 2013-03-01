﻿using System;
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
     public class Bullet : MoveableEntity
    {
         public void setAlive(bool alive)
         {
             this.alive = alive;
         }
         private MoveableEntity creator;
         public MoveableEntity getCreator()
         {
             return creator;
         }

         public Bullet(MoveableEntity creator, Vector3 pos)
         {
                 if (creator is Bullet)
                 {
                     throw new BulletException();
                 }
                 this.creator = creator;
                 this.pos = pos;
                 spin = MathHelper.ToRadians(180);
                 bs.Radius *= 0.035f;
        }

         public override void LoadContent()
         {
             model = Game1.getInstance().Content.Load<Model>("models\\Bullet");
             base.LoadContent();
         }

         public override void Update(GameTime gameTime)
         {
             pos.Z -= 0.6f;
             if (pos.Z < 0)
             {
                 alive = false;
             }
             //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
             world = Matrix.CreateScale(0.035f, 0.035f, 0.035f) * Matrix.CreateRotationY(spin) * Matrix.CreateTranslation(pos);
             base.Update(gameTime);
         }

         public override void Draw(GameTime gameTime)
         {
             base.Draw(gameTime);
         }
        
    }//end of Bullet

    public class BulletException : Exception
    {
         public BulletException() : base("A bullet cannot create another bullet") { }
    }
}

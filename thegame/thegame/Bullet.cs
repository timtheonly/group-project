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
     public class Bullet : MoveableEntity
    {
         private MoveableEntity creator;
         public MoveableEntity getCreator()
         {
             return creator;
         }

         public Bullet(MoveableEntity creator)
         {
                 if (creator is Bullet)
                 {
                     throw new BulletException();
                 }
                 this.creator = creator;
        }

         public override void LoadContent()
         {
             base.LoadContent();
         }

         public override void Update(GameTime gameTime)
         {
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

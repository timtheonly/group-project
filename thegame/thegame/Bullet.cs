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
         public void setAlive(bool alive)
         {
             this.alive = alive;
         }
         private MoveableEntity creator;
         public MoveableEntity getCreator()
         {
             return creator;
         }

         public Bullet(MoveableEntity creator, Vector3 pos, Vector3 look)
         {
                 if (creator is Bullet)
                 {
                     throw new BulletException();
                 }
                 this.look = look;
                 this.creator = creator;
                 this.pos = pos;
                 spinY = MathHelper.ToRadians(180);
                 bs.Radius *= 0.035f;
        }

         public override void LoadContent()
         {
             model = Game1.getInstance().Content.Load<Model>("models\\Bullet");
             foreach (ModelMesh mesh in model.Meshes)
             {
                 if (bs.Radius == 0)
                     bs = mesh.BoundingSphere;
                 else
                     bs = BoundingSphere.CreateMerged(bs, mesh.BoundingSphere);
             }
             base.LoadContent();
         }

         public override void Update(GameTime gameTime)
         {
             forward(2);
             if (pos.Z < -100 || pos.Z > 500)
             {
                 alive = false;
             }
             //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
             world = Matrix.CreateScale(0.075f, 0.075f, 0.075f) * Matrix.CreateRotationY(spinY) * Matrix.CreateTranslation(pos);
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

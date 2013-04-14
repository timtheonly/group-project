/*
 * Bullet: class for bullet (both player and enemy)
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
     public class Bullet : MoveableEntity
    {
         private float theta;
         public void setAlive(bool alive)
         {
             this.alive = alive;
         }
         private MoveableEntity creator;
         public MoveableEntity getCreator()
         {
             return creator;
         }

         public Bullet(MoveableEntity creator, Vector3 pos, Vector3 dir)
         {
                 if (creator is Bullet)
                 {
                     throw new BulletException();
                 }
                 theta = (float)Math.Atan2(dir.X, dir.Z);
                 this.look = dir;
                 this.creator = creator;
                 this.pos = pos;
                 spinY = MathHelper.ToRadians(180);
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
             bs.Radius = 0.25f;
             base.LoadContent();
         }

         public override void Update(GameTime gameTime)
         {
             pos += 2 * look;
             if (pos.Z < -500 || pos.Z > 500)
             {
                 alive = false;
             }
             bs.Center = pos +(world.Right *3);
             //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
             world = Matrix.CreateScale(0.075f) * Matrix.CreateRotationY(theta) * Matrix.CreateTranslation(pos);
             
             base.Update(gameTime);
         }

         public override void Draw(GameTime gameTime)
         {
             #if DEBUG
             BoundingSphereRenderer.Render(bs, Game1.getInstance().getGraphics().GraphicsDevice, Game1.getInstance().getPlayer().getView(), Game1.getInstance().getPlayer().getProjection(), Color.Red);
             #endif
             base.Draw(gameTime);
         }
        
    }//end of Bullet

    public class BulletException : Exception
    {
         public BulletException() : base("A bullet cannot create another bullet") { }
    }
}

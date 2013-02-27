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

         public Bullet(MoveableEntity creator, Vector3 pos)
         {
                 if (creator is Bullet)
                 {
                     throw new BulletException();
                 }
                 this.creator = creator;
                 this.pos = pos;
        }

         public override void LoadContent()
         {
             model = Game1.getInstance().Content.Load<Model>("models\\Bullet");
             base.LoadContent();
         }

         public override void Update(GameTime gameTime)
         {
             //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
             world = Matrix.CreateScale(1.0f, 1.0f, 1.0f) * Matrix.CreateRotationY(spin) * Matrix.CreateTranslation(pos);
             base.Update(gameTime);
         }

         public override void Draw(GameTime gameTime)
         {
             //boiler plate code same for drawing all models
             foreach (ModelMesh mesh in model.Meshes)
             {
                 foreach (BasicEffect effect in mesh.Effects)
                 {
                     effect.EnableDefaultLighting();
                     effect.World = world;
                     effect.Projection = Game1.getInstance().getPlayer().getProjection();
                     effect.View = Game1.getInstance().getPlayer().getView();
                 }
                 mesh.Draw();
             }
             base.Draw(gameTime);
         }
        
    }//end of Bullet

    public class BulletException : Exception
    {
         public BulletException() : base("A bullet cannot create another bullet") { }
    }
}

/*
 * enemy: class for the enemy 
 * 
 * update logic to be added
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
    class Enemy : MoveableEntity
    {
        
        public Enemy(Vector3 pos):base()
        {
            this.pos = pos;
            //spin = MathHelper.ToRadians(90);
        }

        public override void LoadContent()
        {
            model = Game1.getInstance().Content.Load<Model>("models\\tank");
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (bs.Radius == 0)
                    bs = mesh.BoundingSphere;
                else
                    bs = BoundingSphere.CreateMerged(bs, mesh.BoundingSphere);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            //game state needed for testing should be removed when working on AI
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            bs.Center = pos;
            //uncomment the following and the model will spin::
            //float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //spin += timeDelta;

            //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
            world = Matrix.CreateScale(1.0f, 1.0f, 10.0f) * Matrix.CreateRotationY(spin) *Matrix.CreateTranslation(pos);

            //check for collisions with bullets
            foreach (Bullet bullet in Game1.getInstance().bullets)
            {
                if (collidesWith(bullet.getBoundingSphere(), bullet.getWorld()))
                {
                    alive = false;
                    bullet.setAlive(false);
                }
            }
        }
        
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}

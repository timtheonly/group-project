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
    public  class Enemy : MoveableEntity
    {
        float lastShot=  0.0f;
        Random randomSpeed = new Random();
        public Enemy(Vector3 pos):base()
        {
            this.pos = pos;
            //Do NOT CHANGE spinX 
            spinX = 29.86f;
            spinY = -89.5f;
            //spinZ = 90;
            look = new Vector3(0,0,1);
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
            bs.Radius = 3.894f;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (lastShot > 2f)
            {
                //add offset to bullet to position it near the barrell of the tank
                Bullet tempBullet = new Bullet(this, new Vector3(pos.X-3f, pos.Y+1.5f, pos.Z+15.0f), 1, look);
                tempBullet.LoadContent();
                Game1.getInstance().setBullet(tempBullet);
                Shoot.Play();
                lastShot = 0;
            }

            lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //center the bounding sphere on the tanks position
            bs.Center = pos;


            //uncomment the following and the model will spin::
            //float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //spinY += timeDelta;



            //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
            world = Matrix.CreateScale(3.894f, 0.753f, 0.078f) * Matrix.CreateRotationX(spinX) * Matrix.CreateRotationY(spinY)  * Matrix.CreateTranslation(pos);

            //check for collisions with bullets
            for (int i = 0; i < Game1.getInstance().getNumBullets(); i++)
            {
                Bullet tempBullet = Game1.getInstance().getBullet(i);
                if (collidesWith(tempBullet.getBoundingSphere(), tempBullet.getWorld()) && tempBullet.getCreator() is Player)
                {
                    alive = false;
                    tempBullet.setAlive(false);
                }
            }

            //check for collisions with obstacles
            for (int i = 0; i < Game1.getInstance().getNumObstacles(); i++)
            {
                Obstacle tempObstacle = Game1.getInstance().getObstacle(i);
                if (collidesWith(tempObstacle.getBoundingSphere(), tempObstacle.getWorld()))
                {
                    //do nothing for now
                }
            }

            //check for collisions with player
            if (collidesWith(Game1.getInstance().getPlayer().getBoundingSphere(), Game1.getInstance().getPlayer().getWorld()))
            {
                alive = false;
            }
        }
        
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}

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
        
        bool dying = false;
        
        public Enemy(Vector3 pos):base()
        {
            health = 3;
            this.pos = pos;
            //Do NOT CHANGE spinX 
            spinX = 29.86f;
            spinY = 89.5f;
            //spinZ = -50.0f;
            look = new Vector3(0,0,1);
            world = Matrix.Identity;
            
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
            bs.Radius = 3.5f;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 direction = Game1.getInstance().getPlayer().getPos() - pos;
            direction.Normalize();
            if (lastShot > 2f && !dying)
            {
                //add offset to bullet to position it near the barrell of the tank
                Bullet tempBullet = new Bullet(this, pos + (world.Left *-9.5f)+ (world.Forward * -22), (Game1.getInstance().getPlayer().Look()) * -1);
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

            world = Matrix.CreateScale(3.692f, 0.753f, 0.078f) * Matrix.CreateRotationX(spinX) * Matrix.CreateRotationY(spinY) * Matrix.CreateRotationZ(spinZ) *  Matrix.CreateWorld(pos, direction, up);


            //check for collisions with bullets
            for (int i = 0; i < Game1.getInstance().getNumBullets(); i++)
            {
                Bullet tempBullet = Game1.getInstance().getBullet(i);
                if (collidesWith(tempBullet.getBoundingSphere(), tempBullet.getWorld()) && tempBullet.getCreator() is Player)
                {
                    
                    health--;
                    Hit.Play();
                    if (health == 0)
                    {
                        
                        EnemyDying.Play();
                        dying = true;
                    }
                    tempBullet.setAlive(false);
                   
                }
                if (dying)
                {  
                    break;
                }

            }

            if (dying)
            {
                
                pos -= up;
            }

            //check for collisions with obstacles
            for(int i = 0; i < Game1.getInstance().getNumObstacles(); i++)
            {
                Obstacle tempObstacle = Game1.getInstance().getObstacle(i);
                if (collidesWith(tempObstacle.getBoundingSphere(), tempObstacle.getWorld()))
                {
                    backward(1.0f);
                }
            }

            //check for collisions with player
            if (collidesWith(Game1.getInstance().getPlayer().getBoundingSphere(), Game1.getInstance().getPlayer().getWorld()))
            {
                alive = false;
            }
        }
        int timesShown = 0;
        public override void Draw(GameTime gameTime)
        {
            
            if(dying)
            {
                timesShown++;
            }
            if (timesShown > 100)
            {
                alive = false;
            }
            base.Draw(gameTime);
        }
    }
}

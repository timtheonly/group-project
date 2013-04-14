/*
 * enemy: class for the enemy 
 * 
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
        private float lastShot=  0.0f;
        
        
        bool dying = false;
        
        //path stuff
        List<Vector3> pathPoints;
        int currentPoint =0;

        public Enemy(Vector3 pos):base()
        {
            health = 3;
            this.pos = pos;
            //Do NOT CHANGE spinX 
            spinX = 29.86f;
            spinY = 89.5f;
            look = new Vector3(0,0,1);
            world = Matrix.Identity;
            pathPoints = new List<Vector3>();
            pathPoints.Add(new Vector3(60f,0f,-60f));
            pathPoints.Add(new Vector3(20f, 0f, -65f));
            pathPoints.Add(new Vector3(120f, 0f, -40f));
            pathPoints.Add(new Vector3(90f, 0f, -75f));
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
            bs.Radius = 7f;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {     
            Vector3 direction = pathPoints[currentPoint] - pos;
            float distance = direction.LengthSquared();
            direction.Normalize();

            if (distance > 5)
            {
                spinY = (float)Math.Atan2(direction.X, direction.Z);
                pos += direction* 0.55f;
            }
            else
            {
                direction = Game1.getInstance().getPlayer().getPos() - pos;
                direction.Normalize();
                spinY = (float)Math.Atan2(direction.X, direction.Z);
                if (lastShot > 2.0f && !dying)
                {
                    //add offset to bullet to position it near the barrell of the tank
                    Bullet tempBullet = new Bullet(this, pos, direction);
                    tempBullet.LoadContent();
                    Game1.getInstance().setBullet(tempBullet);
                    Shoot.Play();
                    lastShot = 0;
                    if (currentPoint < (pathPoints.Count - 1))
                    {
                        currentPoint++;
                    }
                    else 
                    {
                        currentPoint = 0;
                    }
                }
                lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
            }
            
            
            
            
            //center the bounding sphere on the tanks position
            bs.Center = pos + (world.Left * 5);

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
                pos -= up * 2;
            }

            //check for collisions with obstacles
            for(int i = 0; i < Game1.getInstance().getNumObstacles(); i++)
            {
                Obstacle tempObstacle = Game1.getInstance().getObstacle(i);
                if (collidesWith(tempObstacle.getBoundingSphere(), tempObstacle.getWorld()))
                {
                    if (currentPoint < (pathPoints.Count - 1))
                    {
                        currentPoint++;
                    }
                    else
                    {
                        currentPoint = 0;
                    }
                }
            }

            //check for collisions with player
            if (collidesWith(Game1.getInstance().getPlayer().getBoundingSphere(), Game1.getInstance().getPlayer().getWorld()))
            {
                pos -= direction;
            }

            //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
            world = Matrix.CreateScale(1f, 0.2f, 5.5f) * Matrix.CreateRotationY(spinY) * Matrix.CreateTranslation(pos);
        }
        int timesDeathShown = 0;
        public override void Draw(GameTime gameTime)
        {
            #if DEBUG
            BoundingSphereRenderer.Render(bs, Game1.getInstance().getGraphics().GraphicsDevice, Game1.getInstance().getPlayer().getView(), Game1.getInstance().getPlayer().getProjection(),Color.Red);
            #endif
            if(dying)
            {
                timesDeathShown++;
            }
            if (timesDeathShown > 100)
            {
                alive = false;
            }
            base.Draw(gameTime);
        }
    }
}

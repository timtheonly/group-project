/*
 *  player: camera class for point of view
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
   public  class Player : MoveableEntity
    {
        //shot limiter
        float lastShot = 0;

        int hitCount;
        SpriteFont hitCountSF;
        SpriteFont collisionSF;
        SpriteFont scoreSF;

        //overlay for targeting system, health and radar
        Texture2D layer;
        Texture2D healthlayer1,healthlayer2,healthlayer3;

        private String collisionString ="";
        
        private Matrix projection;
        public Matrix getProjection()
        {
            return projection;
        }
       
        private Matrix view;
        public Matrix getView()
        {
            return view;
        }
        
        public Player(Vector3 pos)
        {
            this.pos = pos;
            health = 8;
            hitCount = 0;
            score = 0;
            bs = new BoundingSphere(pos, 5f);
        }

        public override void LoadContent()
        {
            scoreSF = Game1.getInstance().Content.Load<SpriteFont>("Score");
            hitCountSF = Game1.getInstance().Content.Load<SpriteFont>("hitcount");
            collisionSF = Game1.getInstance().Content.Load<SpriteFont>("collision");
            layer = Game1.getInstance().Content.Load<Texture2D>("textures\\normalaim");
            //layer = Game1.getInstance().Content.Load<Texture2D>("textures\\normalaim");
            healthlayer1 = Game1.getInstance().Content.Load<Texture2D>("textures\\1");
            healthlayer2 = Game1.getInstance().Content.Load<Texture2D>("textures\\2");
            healthlayer3 = Game1.getInstance().Content.Load<Texture2D>("textures\\3");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyState = Keyboard.GetState();

            // move camera left and right
            if (currentState.ThumbSticks.Right.X < 0 || keyState.IsKeyDown(Keys.Left))
            {
                yaw(MathHelper.ToRadians(0.5f));
            }

            if (currentState.ThumbSticks.Right.X > 0 || keyState.IsKeyDown(Keys.Right))
            {
                yaw(MathHelper.ToRadians(-0.5f));

            }

            //move camera forward and back
            if (currentState.ThumbSticks.Left.Y > 0 || keyState.IsKeyDown(Keys.Up))
            {
                forward(1);
            }

            if (currentState.ThumbSticks.Left.Y < 0 || keyState.IsKeyDown(Keys.Down))
            {
                backward(0.35f);
            }
            
            // limit the amount of bullets that can be spawned with last shot
            if ((currentState.Triggers.Right >0||keyState.IsKeyDown(Keys.Space)) && lastShot > 0.9)
            {
                //add offset to the bullet vector3 to center it in the crosshairs
                Bullet tempBullet = new Bullet(this, new Vector3(pos.X+0.06f, pos.Y-0.12f, pos.Z-1.5f), look);
                tempBullet.LoadContent();
                Game1.getInstance().setBullet(tempBullet);
                lastShot = 0;
                Shoot.Play();
            }
            lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //collision detection stuff
            bs.Center = pos;

            if (collidesWith(Game1.getInstance().getEnemy().getBoundingSphere(), Game1.getInstance().getEnemy().getWorld()) && Game1.getInstance().getEnemy().isAlive())
            {
                hitCount++;
            }

            //check for collisions with bullets
            for (int i = 0; i < Game1.getInstance().getNumBullets();i++)
            {
                Bullet tempBullet = Game1.getInstance().getBullet(i);
                if (collidesWith(tempBullet.getBoundingSphere(), tempBullet.getWorld()) && tempBullet.getCreator() is Enemy)
                {
                    GettingHit.Play();
                    
                    tempBullet.setAlive(false);
                    hitCount++;
                    health -=2;
                    Game1.getInstance().CheckForRumble(gameTime);
                    if (health <= 0)
                    {
                        alive = false;
                    }
                }
            }

            //check for collisions with obstacles
            for (int i = 0; i < Game1.getInstance().getNumObstacles(); i++)
            {
                Obstacle tempObstacle = Game1.getInstance().getObstacle(i);
                if (collidesWith(tempObstacle.getBoundingSphere(), tempObstacle.getWorld()))
                {
                    Crash.Play();
                    //player cant drive through obstaclesgit 
                    collisionString = "collision: x: " + tempObstacle.getPos().X + " y: " + tempObstacle.getPos().Y + " Z: " + tempObstacle.getPos().Z;
                    if (currentState.ThumbSticks.Left.Y > 0 || keyState.IsKeyDown(Keys.Up))
                    {
                        backward(1.0f);
                    }

                    if (currentState.ThumbSticks.Left.Y < 0 || keyState.IsKeyDown(Keys.Down))
                    {
                        forward(1);
                    }
                }
            }
            view = Matrix.CreateLookAt(pos, pos + look, up);
            world = Matrix.Identity;
            //projection is the view space, anything out of this range is not drawn
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game1.getInstance().getGraphics().GraphicsDevice.Viewport.AspectRatio, 1.0f, 1000.0f);
        }

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            BoundingSphereRenderer.Render(bs, Game1.getInstance().getGraphics().GraphicsDevice, Game1.getInstance().getPlayer().getView(), Game1.getInstance().getPlayer().getProjection(), Color.Red);
#endif
            if (health <= 6 && health >= 4)
            {
                Game1.getInstance().getSpriteBatch().Draw(healthlayer1, new Vector2(0, 0), Color.White);
            }
            if (health <= 4 && health >= 2)
            {
                Game1.getInstance().getSpriteBatch().Draw(healthlayer2, new Vector2(0, 0), Color.White);
            }
            if (health <= 2 && health >= 0)
            {
                Game1.getInstance().getSpriteBatch().Draw(healthlayer3, new Vector2(0, 0), Color.White);
            }

#if DEBUG
            Game1.getInstance().getSpriteBatch().DrawString(hitCountSF, "location: x: " + pos.X + " y: "+ pos.Y + "Z: " + pos.Z + "hit" + hitCount, new Vector2(0, 0), Color.White);
            Game1.getInstance().getSpriteBatch().DrawString(collisionSF, collisionString, new Vector2(0, 10), Color.White);
#endif
            Game1.getInstance().getSpriteBatch().Draw(layer, new Vector2(0, -60), Color.White);

            Game1.getInstance().getSpriteBatch().DrawString(scoreSF, "Score: " + score, new Vector2(570, 10), Color.Red);
        }

       public Vector3 Look()
       {
           return look;
       }

       public float getYSpin()
       {
           return spinY;
       }

       protected static int score;

       public void Score(int value)
       {
           score += 100 * value;

       }

       public int getScore()
       {
           return score; 
       }

       public void resetScore()
       {
           score = 0;
       }



    }
}

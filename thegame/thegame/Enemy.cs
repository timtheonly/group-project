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
        
        public Enemy(Vector3 pos):base()
        {
            this.pos = pos;
            //Do NOT CHANGE spinX 
            spinX = 29.86f;
            spinY = -89.5f;
            //spinZ = 90;
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
            
        }

        public override void Update(GameTime gameTime)
        {
            //game state needed for testing should be removed when working on AI
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyState = Keyboard.GetState();

            if (currentState.Buttons.A == ButtonState.Pressed ||keyState.IsKeyDown(Keys.A))
            {
                //add offset to bullet to position it near the barrell of the tank
                Bullet tempBullet = new Bullet(this, new Vector3(pos.X-3f, pos.Y+1.0f, pos.Z+15.0f), 1);
                tempBullet.LoadContent();
                Game1.getInstance().bullets.Add(tempBullet);
                //Shoot.Play();
            }

            //center the bounding sphere on the tanks position
            bs.Center = pos;


            //uncomment the following and the model will spin::
            //float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //spinY += timeDelta;



            //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
            world = Matrix.CreateScale(3.894f, 0.753f, 0.078f) * Matrix.CreateRotationX(spinX) * Matrix.CreateRotationY(spinY)  * Matrix.CreateTranslation(pos);


            //check for collisions with bullets
            foreach (Bullet bullet in Game1.getInstance().bullets)
            {
                if (collidesWith(bullet.getBoundingSphere(), bullet.getWorld()) && bullet.getCreator() is  Player)
                {
                    alive = false;
                    bullet.setAlive(false);
                }
            }

            if (collidesWith(Game1.getInstance().getPlayer().getBoundingSphere(), Game1.getInstance().getPlayer().getWorld()) && Game1.getInstance().getEnemy().isAlive())
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

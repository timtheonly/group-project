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

        float speed;
        //shot limiter
        float lastShot = 0;
        //overlay for targeting system, health and radar
        Texture2D layer;
        Texture2D healthlayer1,healthlayer2,healthlayer3;
        SoundEffect Shoot;
        
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
            speed = 0.2f;
            health = 100;
        }

        public override void LoadContent()
        {
            Shoot = Game1.getInstance().Content.Load<SoundEffect>("sounds\\shotbetter"); //load sound

            layer = Game1.getInstance().Content.Load<Texture2D>("textures\\normalaim");
            //layer = Game1.getInstance().Content.Load<Texture2D>("textures\\normalaim");
            healthlayer1 = Game1.getInstance().Content.Load<Texture2D>("textures\\1");
            healthlayer2 = Game1.getInstance().Content.Load<Texture2D>("textures\\2");
            healthlayer3 = Game1.getInstance().Content.Load<Texture2D>("textures\\3");
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyState = Keyboard.GetState();

            // move camera left and right
            if (currentState.ThumbSticks.Right.X < 0 || keyState.IsKeyDown(Keys.Left))
            {
                pos -= Vector3.Cross(up, right) * speed;
            }

            if (currentState.ThumbSticks.Right.X > 0 || keyState.IsKeyDown(Keys.Right))
            {
                pos += Vector3.Cross(up, right) * speed;

            }

            //move camera forward and back
            if (currentState.ThumbSticks.Right.Y > 0 || keyState.IsKeyDown(Keys.Up))
            {
                pos -= right * speed;
            }

            if (currentState.ThumbSticks.Right.Y < 0 || keyState.IsKeyDown(Keys.Down))
            {
                pos += right * speed;
            }
            
            // limit the amount of bullets that can be spawned with last shot
            if ((currentState.Triggers.Left >0||keyState.IsKeyDown(Keys.Space)) && lastShot > 0.9)
            {
                //add offset to the bullet vector3 to center it in the crosshairs
                Bullet tempBullet = new Bullet(this, new Vector3(pos.X+0.06f, pos.Y-0.12f, pos.Z-1.5f));
                tempBullet.LoadContent();
                Game1.getInstance().bullets.Add(tempBullet);
                lastShot = 0;
                Shoot.Play();
            }
            lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            view = Matrix.CreateLookAt(pos, pos + look, up);
            //projection is the view space, anything out of this range is not drawn
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game1.getInstance().getGraphics().GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f);
            
        
        }

        public override void Draw(GameTime gameTime)
        {
            if (health < 80 && health > 50)
            {
                Game1.getInstance().getSpriteBatch().Draw(healthlayer1, new Vector2(0, 0), Color.White);
            }
            if (health < 50 &&  health > 30)
            {
                Game1.getInstance().getSpriteBatch().Draw(healthlayer2, new Vector2(0, 0), Color.White);
            }
            if (health < 30)
            {
                Game1.getInstance().getSpriteBatch().Draw(healthlayer3, new Vector2(0, 0), Color.White);
            }
            //Game1.getInstance().getSpriteBatch().Draw(healthlayer3, new Vector2(0, 0), Color.White);
            
            Game1.getInstance().getSpriteBatch().Draw(layer, new Vector2(0, -60), Color.White);
        }
    }
}

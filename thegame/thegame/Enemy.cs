/*
 enemy: class for the enemy 
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
            //Do NOT CHANGE spinX 
            spinX = 29.86f;
            spinY = -89.5f;
            //spinZ = 90;
        }

        public override void LoadContent()
        {
            model = Game1.getInstance().Content.Load<Model>("models\\tank");
        }

        public override void Update(GameTime gameTime)
        {
            //game state needed for testing should be removed when working on AI
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            //uncomment the following and the model will spin::
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //spin += timeDelta;

            //each model has a world matrix for scale rotation and translation  NB: Translation MUST BE LAST
            world = Matrix.CreateScale(3.894f, 0.753f, 0.078f) * Matrix.CreateRotationX(spinX) * Matrix.CreateRotationY(spinY) * Matrix.CreateRotationZ(spinZ) * Matrix.CreateTranslation(pos);

            
        }
        
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}

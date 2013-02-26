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
        Matrix world;
        public Enemy(Vector3 pos)
        {
            this.pos = pos;
            
        }

        public override void LoadContent()
        {
            model = Game1.getInstance().Content.Load<Model>("models\\tank");
        }

        public override void Update(GameTime gameTime)
        {
            //game state needed for testing should be removed when working on AI
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            //float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //spin += timeDelta;
            world = Matrix.CreateScale(1.0f, 1.0f, 25.0f) * Matrix.CreateRotationY(spin);

            
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
        }
    }
}

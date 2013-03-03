/*
 * obstacle: class for all obsticales
 * 
 * static so no need for movement
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
    class Obstacle : GameEntity
    {
        public Obstacle(Vector3 pos)
        {
            this.pos = pos;
        }

        public override void LoadContent()
        {
            model = Game1.getInstance().Content.Load<Model>("models\\square");
        }

        public override void Draw(GameTime gameTime)
        {
            world = Matrix.CreateScale(2.961f, 2.961f, 2.961f) * Matrix.CreateTranslation(pos);
            base.Draw(gameTime);
        }
    }
}

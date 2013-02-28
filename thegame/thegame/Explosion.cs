using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{
    public class Explosion:MoveableEntity
    {
        public Explosion(Vector3 pos)
        {
            this.pos = pos;
        }

        public override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float width = 2;
            float height = 2;
            float speed = 50.0f;
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((pos.X < -(width / 2)) || (pos.X > width / 2) || (pos.Z < -(height / 2)) || (pos.Z > height / 2) || (pos.Y < 0) || (pos.Y > 100))
            {
                alive = false;
            }
            pos += look * speed * timeDelta;
        }

        public override void Draw(GameTime gameTime)
        {
            Line.DrawLine(pos, pos + look * 5, Color.Green);
            //Line.DrawLine(pos, pos + look * 5, Color.Green);
            //Line.DrawLine(pos, pos + look * 5, Color.Green);
        }
    }
}

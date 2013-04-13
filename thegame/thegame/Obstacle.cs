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
    public class Obstacle : GameEntity
    {
        public Obstacle(Vector3 pos)
        {
            this.pos = pos;
        }

        public override void LoadContent()
        {
            model = Game1.getInstance().Content.Load<Model>("models\\square");
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (bs.Radius == 0)
                    bs = mesh.BoundingSphere;
                else
                    bs = BoundingSphere.CreateMerged(bs, mesh.BoundingSphere);
            }
            bs.Radius = 5f;
            bs.Center = pos;
        }

        public override void Update(GameTime gameTime)
        {
            //check for collisions with bullets
            for (int i = 0; i < Game1.getInstance().getNumBullets(); i++)
            {
                Bullet tempBullet = Game1.getInstance().getBullet(i);
                if (collidesWith(tempBullet.getBoundingSphere(), tempBullet.getWorld()) && tempBullet.getCreator() is Player)
                {
                    tempBullet.setAlive(false);
                }
                if (collidesWith(tempBullet.getBoundingSphere(), tempBullet.getWorld()) && tempBullet.getCreator() is Enemy)
                {
                    tempBullet.setAlive(false);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            world = Matrix.CreateScale(5.961f, 5.961f, 5.961f) * Matrix.CreateTranslation(pos);
            #if DEBUG
            BoundingSphereRenderer.Render(bs, Game1.getInstance().getGraphics().GraphicsDevice, Game1.getInstance().getPlayer().getView(), Game1.getInstance().getPlayer().getProjection(), Color.Blue);
            #endif
            base.Draw(gameTime);
        }
    }
}
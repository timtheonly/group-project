/*
 * GameEntity: base class for every drawable in the game
 */

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
    public class GameEntity
    {
        protected Vector3 pos;
        protected Model model;
        public Model getModel()
        {
            return model;
        }

        protected Matrix world;
        public Matrix getWorld()
        {
            return world;
        }
        
        public virtual void LoadContent()
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }

        public bool collidesWith(Model otherModel,Matrix otherWorld)
        {
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(ModelMesh otherMesh in otherModel.Meshes)
                {
                    if(mesh.BoundingSphere.Transform(world).Intersects(otherMesh.BoundingSphere.Transform(otherWorld)))
                    {
                        return true;
                    }
                }
            }
            return false;            
        }
        public virtual void Draw(GameTime gameTime)
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
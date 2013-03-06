﻿/*
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
        protected BoundingSphere bs;
        protected SoundEffect Shoot;

        public BoundingSphere getBoundingSphere()
        {
            return bs;
        }
        public GameEntity()
        {
            bs = new BoundingSphere();
        }
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
            Shoot = Game1.getInstance().Content.Load<SoundEffect>("sounds\\shotbetter"); //load sound
        }
        public virtual void Update(GameTime gameTime)
        {
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

        public bool collidesWith(BoundingSphere otherBS, Matrix otherWorld)
        {
            if (bs.Transform(world).Intersects(otherBS.Transform(otherWorld)))
            {
                return true;
            }
            return false;
        }
    }


}
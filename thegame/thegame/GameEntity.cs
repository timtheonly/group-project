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
        protected Matrix world;
        
        public virtual void LoadContent()
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(GameTime gameTime)
        {
        }
    }


}
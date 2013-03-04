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
    public class Radar: GameEntity
    {
        private Texture2D playerDot;
        private Texture2D enemyDot;
        private Texture2D radarLayer;



        // Distance that the radar can
        private const float RadarRange = 500.0f;
        private const float RadarRangeSquared = RadarRange * RadarRange;

        private const float RadarScreenRadius = 40.0f;
        private static Vector2 RadarCenterPos = new Vector2(750, 400);
        private Vector2 imageCenter;
        private Vector2 differanceVect;

        public Radar()
        { 
            
        }

        public override void LoadContent()
        {
            radarLayer = Game1.getInstance().Content.Load<Texture2D>("textures\\radar");
            playerDot = Game1.getInstance().Content.Load<Texture2D>("textures\\playerLoc");
            enemyDot = Game1.getInstance().Content.Load<Texture2D>("textures\\enemyLoc");
            base.LoadContent();

            imageCenter = new Vector2(radarLayer.Width * 0.5f, radarLayer.Height * 0.5f);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            differanceVect = new Vector2(Game1.getInstance().getEnemy().getPos().X - Game1.getInstance().getPlayer().getPos().X, Game1.getInstance().getEnemy().getPos().Z - Game1.getInstance().getPlayer().getPos().Z);
            float distance = differanceVect.LengthSquared();
            if (distance < RadarRangeSquared)
            {
                // Scale the distance from world coords to radar coords
                differanceVect *= RadarScreenRadius / RadarRange;

                // We rotate each point on the radar so that the player is always facing UP on the radar
               differanceVect = Vector2.Transform(differanceVect, Matrix.CreateRotationZ(MathHelper.ToRadians(0)));

                // Offset coords from radar's center
                differanceVect += RadarCenterPos;
                
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.getInstance().getSpriteBatch().Draw(enemyDot, differanceVect, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), 1.0f, SpriteEffects.None, 0.0f);
            Game1.getInstance().getSpriteBatch().Draw(playerDot, RadarCenterPos, Color.White);
            Game1.getInstance().getSpriteBatch().Draw(radarLayer, RadarCenterPos,null, Color.White, 0.0f, imageCenter, RadarScreenRadius / (radarLayer.Height * 0.5f), SpriteEffects.None, 0.0f);
            //base.Draw(gameTime);
        } 
    }
}

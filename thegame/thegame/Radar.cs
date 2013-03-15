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
        private const float radarRange = 500.0f;
        private const float radarRangeSquared = radarRange * radarRange;

		private float rotation;

        private const float radarScreenRadius = 61.0f;
        private static Vector2 radarCenterPos = new Vector2(700, 400);
        private Vector2 playerPos;
		private Vector2 playerLook;
        private Vector2 imageCenter;
        private Vector2 differanceVect;
		private Vector2 playerCenter;

        public Radar()
        { 
        }

        public override void LoadContent()
        {
            radarLayer = Game1.getInstance().Content.Load<Texture2D>("textures\\radar");
            playerDot = Game1.getInstance().Content.Load<Texture2D>("textures\\playerLoc");
            enemyDot = Game1.getInstance().Content.Load<Texture2D>("textures\\enemyLoc");

			//get the center of the images
			playerCenter = new Vector2(playerDot.Width *0.5f,playerDot.Height *0.5f);
            imageCenter = new Vector2(radarLayer.Width * 0.5f, radarLayer.Height * 0.5f);
            base.LoadContent();


        }

        public void Update(GameTime gameTime, Vector3 look)
        {
			//transform the 3d vectors to 2d vectors
			playerLook = new Vector2(-look.X,-look.Z);
            differanceVect = new Vector2(Game1.getInstance().getEnemy().getPos().X - Game1.getInstance().getPlayer().getPos().X, Game1.getInstance().getEnemy().getPos().Z - Game1.getInstance().getPlayer().getPos().Z);
            float distance = differanceVect.LengthSquared();
            if (distance < radarRangeSquared)
            {
                // Scale the distance from world coords to radar coords
                differanceVect *= radarScreenRadius / radarRange;

                // We rotate each point on the radar so that the player is always facing UP on the radar
               	differanceVect = Vector2.Transform(differanceVect, Matrix.CreateRotationZ(MathHelper.ToRadians(0)));

                // Offset coords from radar's center
                differanceVect += radarCenterPos;
                playerPos = new Vector2(Game1.getInstance().getPlayer().getPos().X, Game1.getInstance().getPlayer().getPos().Z);
                playerPos *= radarScreenRadius / radarRange;
                playerPos += radarCenterPos;
            }
			rotation = -(float)Math.Atan2(playerLook.X,playerLook.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.getInstance().getSpriteBatch().Draw(radarLayer, radarCenterPos, null, Color.White, 0.0f, imageCenter, radarScreenRadius / (radarLayer.Height * 0.5f), SpriteEffects.None, 0.0f);
            if (Game1.getInstance().getEnemy().isAlive())
            {
                Game1.getInstance().getSpriteBatch().Draw(enemyDot, new Vector2(differanceVect.X - (enemyDot.Width * 0.5f), differanceVect.Y - (enemyDot.Width * 0.5f)), null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), 1.0f, SpriteEffects.None, 0.0f);
            }
			Game1.getInstance().getSpriteBatch().Draw(playerDot, playerPos,null,Color.White,rotation,playerCenter,1,SpriteEffects.None,1);
        } 
    }
}
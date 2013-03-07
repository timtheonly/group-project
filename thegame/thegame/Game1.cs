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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        RasterizerState WIREFRAME_RASTERIZER_STATE = new RasterizerState() { CullMode = CullMode.CullClockwiseFace, FillMode = FillMode.WireFrame};
        private GraphicsDeviceManager graphics;
        private Texture2D background;
        private int level = 1;
        private int v = 1;
        public GraphicsDeviceManager getGraphics()
        {
            return graphics;
        }

        private SpriteBatch spriteBatch;
        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch;
        }

        protected Player plyr;
        public Player getPlayer()
        {
            return plyr;
        }

        private Enemy enemy;
        public Enemy getEnemy()
        {
            return enemy;
        }

        private Radar radar;
        private List<Bullet> _bullets;
        public Bullet getBullet(int loc)
        {
            return _bullets[loc];
        }
        public void setBullet(Bullet bullet)
        {
            _bullets.Add(bullet);
        }
        public int getNumBullets()
        {
            return _bullets.Count;
        }

        public List<Obstacle> _obstacles;
        public Obstacle getObstacle(int loc)
        {
            return _obstacles[loc];
        }
        public void setBullet(Obstacle obstacle)
        {
            _obstacles.Add(obstacle);
        }
        public int getNumObstacles()
        {
            return _obstacles.Count;
        }

        private static Game1 instance;
        public  static Game1 getInstance()
        {
            return instance;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _bullets = new List<Bullet>();
            _obstacles = new List<Obstacle>();
            enemy = new Enemy(new Vector3(0, -3f, -30));
            plyr = new Player(new Vector3(0,0,50));
            radar = new Radar();
            //boom = new Explosion(new Vector3(0,0,-10));

            //obstacles spawn in random locations
            Random rand = new Random();

          
            
           
            for (int num = 0; num < 20; num++)
            {
                int x = rand.Next(-100, 500);
                int z = rand.Next(-100, 500);
                
                //draw obstacle if it does not spawn on an enemy or the player
                Obstacle tempObstacle = (new Obstacle(new Vector3(x, 0, z)));
                if (!tempObstacle.collidesWith(enemy.getBoundingSphere(), enemy.getWorld()) && !tempObstacle.collidesWith(plyr.getBoundingSphere(), plyr.getWorld()))
                {
                    _obstacles.Add(tempObstacle);
                }
                
            }
        

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.LoadContent();
            }
            foreach(Bullet bullet in _bullets)
            {
                bullet.LoadContent();
            }

            radar.LoadContent();
            enemy.LoadContent();
            plyr.LoadContent();
            background = Game1.getInstance().Content.Load<Texture2D>("textures\\background");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (enemy.isAlive())
            {
                enemy.Update(gameTime);
            }
            else
            {
                if (level == 1)
                {
                    v = v + plyr.health;
                }

                level++;
                plyr.health +=2;
                plyr.Score(v);
                enemy = new Enemy(new Vector3(0, -3f, -30));
                enemy.LoadContent();
                
                if (level == 2)
                {

                    v = 2;
                    if (plyr.health > 0)
                    {
                        v = plyr.health + v;
                    }
                    enemy.health += 3;
                }
                if (level == 3)
                {

                    v = 4;
                    if (plyr.health > 0)
                    {
                        v = plyr.health + v;
                    }
                    enemy.health += 3;
                }
            }
            
            // TODO: Add your update logic here
            radar.Update(gameTime);
            plyr.Update(gameTime);
            for (int i = 0; i < _bullets.Count;i++ )
            {
                if (_bullets[i].isAlive())
                {
                    _bullets[i].Update(gameTime);
                }
                else
                {
                    _bullets.RemoveAt(i);
                }
            }

            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 	40, 	40));
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            GraphicsDevice.RasterizerState = WIREFRAME_RASTERIZER_STATE;    // draw in wireframe
            GraphicsDevice.BlendState = BlendState.Opaque;                  // no alpha this time

            //TODO: Add your drawing code here
            

            foreach (Bullet bullet in _bullets)
            {
                bullet.Draw(gameTime);
            }
            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.Draw(gameTime);
            }
            if (enemy.isAlive())
            {
                enemy.Draw(gameTime);
            }

            spriteBatch.Begin();
            plyr.Draw(gameTime);
            radar.Draw(gameTime);
            spriteBatch.End();
         
           
            base.Draw(gameTime);
        }
    }
}

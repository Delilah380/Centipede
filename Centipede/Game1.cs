using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Centipede
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        bool startGame = false;
        bool lifeLost = false;
        bool youLost = false;
        bool youWin = false;
        int speed = 1;
        int score = 0;
        int newBody = 0;
        int highScore = 0;
        int asteroidCount = 7;
        int number = 0;
        Random random = new Random();
        Player player;
        Sprite flameSE;
        Sprite flameSW;
        Sprite flameNW;
        Sprite flameNE;
        private List<Centipede> head = new List<Centipede>();
        //private List<Body> body = new List<Body>();
        private List<Sprite> asteroids = new List<Sprite>();
        private List<Player> projectiles = new List<Player>();
        private List<Spider> spiders = new List<Spider>();
        private List<Sprite> stars = new List<Sprite>();
        private List<Sprite> lives = new List<Sprite>();
        SpriteFont titleFont;
        SpriteFont textFont;
        SpriteFont scoreFont;
        Texture2D headImage;
        Texture2D headDImage;
        Texture2D bodyFImage;
        Texture2D bodyDImage;
        Texture2D projectileImage;
        Texture2D spiderImage;
        Texture2D flameImage;
        Texture2D flameFlipImage;
        Texture2D lifeImage;
        Texture2D asteroidImage;
        TimeSpan zero = TimeSpan.Zero;
        TimeSpan zeroProjectile = TimeSpan.Zero;
        TimeSpan up = TimeSpan.Zero;
        TimeSpan down = TimeSpan.Zero;
        TimeSpan frames = new TimeSpan(0, 0, 0, 0, 100);
        TimeSpan framesProjectile = new TimeSpan(0, 0, 0, 0, 100);

        //TimeSpan example = new TimeSpan(0, 0, 0, 5, 0);
        //TimeSpan exampleWaiter = TimeSpan.Zero;

        //2-25-24: Work on centipede movement
        //3-10-24: Work on projectiles
        //3-17-24: Work on spider movement + player movement bugs
        //4-7-24: Work on asteroids/centipede
        //4-14-24: WOrk on asteroids/centipede (worked on player life lost)
        //4-21-24: Work on bit of code starting at line 177 & work on centipede movement
        //5-5-24: Work on centipede movement
        //5-26-24: Work on centipede body movement/rotating the image
        //6-2-24: Work on slowing down the centipede and damaging the centipede
        //6-9-24: Work on creating new centipedes
        //6-23: intersecting
        //7-21: missing body parts
        //8-11-24: centipede still bugged
        //8-18-24: hitting centipede head
        //8-25-24: work on killing all centipedes, end screen, points, and new levels
        //9-8-24: fix game not speeding up afte heads destroyed :( 
        //9-29-24: Done?

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            graphics.PreferredBackBufferWidth = 1100;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            titleFont = Content.Load<SpriteFont>("title");
            textFont = Content.Load<SpriteFont>("text");
            scoreFont = Content.Load<SpriteFont>("score");

            //load player rocket
            Texture2D playerImage = Content.Load<Texture2D>("rocket-centipede");
            player = new Player(playerImage, 525, 500, 50, 50, Color.White, 10);

            //load centipede head
            headImage = Content.Load<Texture2D>("head-centipede");
            headDImage = Content.Load<Texture2D>("headD-centipede");
            head.Add(new Centipede(headImage, 525, 0, 25, 25, Color.White, new Vector2((float)2.5, (float)25)));

            //load centipede body
            bodyFImage = Content.Load<Texture2D>("bodyF-centipede");
            bodyDImage = Content.Load < Texture2D>("bodyD-centipede");
            head[0].AddBody(bodyDImage, 25);

            //load asteroid
            asteroidImage = Content.Load<Texture2D>("asteroid-centipede");
            for(int i = 0; i < asteroidCount; i++)
            {
                int X = random.Next(20);
                asteroids.Add(new Sprite(asteroidImage, (X * 50) + 50, (i*50) + 50, 50, 50, Color.White));
                for(int j = 0; j < asteroids.Count - 1; j++)
                {
                    if (asteroids[i].GetX() == asteroids[j].GetX() && asteroids[i].GetY() == asteroids[j].GetY())
                    {
                        asteroids[i].ChangeCord();
                        j = 0;
                    }
                }
            }

            //load projectile
            projectileImage = Content.Load<Texture2D>("projectile-centipede");
            projectiles.Add(new Player(projectileImage, -100, -100, 15, 15, Color.White, 25));

            //load spider
            spiderImage = Content.Load<Texture2D>("spiders-centipede");
            spiders.Add(new Spider(spiderImage, 100, 100, 50, 50, Color.White, 2 + number, 15));

            //load star/barrer
            Texture2D starImage = Content.Load<Texture2D>("stars-centipede");
            for(int i = 0; i < 22; i++)
            {
                stars.Add(new Sprite(starImage, 0 + i * 50, 400, 50, 50, Color.White));
            }

            //load lives
            lifeImage = Content.Load<Texture2D>("life-centipede");
            for (int i = 0; i < 3; i++)
            {
                lives.Add(new Sprite(lifeImage, 575 + i * 25, 25, 25, 25, Color.White));
            }

            //load fire
            flameImage = Content.Load <Texture2D>("flame-centipede");
            flameFlipImage = Content.Load<Texture2D>("flameFlip-centipede");
            flameSE = new Sprite(flameImage, 525, 275, 250, 250, Color.White);
            flameSW = new Sprite(flameFlipImage, 525, 275, 250, 250, Color.White);
            flameNW = new Sprite(flameFlipImage, 525, 275, 250, 250, Color.White);
            flameNE = new Sprite(flameImage, 525, 275, 250, 250, Color.White);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            zero += gameTime.ElapsedGameTime;
            zeroProjectile += gameTime.ElapsedGameTime;
            up += gameTime.ElapsedGameTime;
            down += gameTime.ElapsedGameTime;
            

            // TODO: Add your update logic here

            //start game
            if (!startGame && Keyboard.GetState().IsKeyDown(Keys.Enter) && !youLost)
            {
                startGame = true;
            }
            if(!startGame && youLost && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                startGame = true;
                head.Clear();
                head.Add(new Centipede(headImage, 525, 0, 25, 25, Color.White, new Vector2((float)2.5, (float)25)));
                head[0].AddBody(bodyDImage, 25);
                for (int i = 0; i < 3; i++)
                {
                    lives.Add(new Sprite(lifeImage, 575 + i * 25, 25, 25, 25, Color.White));
                }
                score = 0;
                asteroids.Clear();
                for (int i = 0; i < asteroidCount; i++)
                {
                    int X = random.Next(20);
                    asteroids.Add(new Sprite(asteroidImage, (X * 50) + 50, (i * 50) + 50, 50, 50, Color.White));
                    for (int j = 0; j < asteroids.Count - 1; j++)
                    {
                        if (asteroids[i].GetX() == asteroids[j].GetX() && asteroids[i].GetY() == asteroids[j].GetY())
                        {
                            asteroids[i].ChangeCord();
                            j = 0;
                        }
                    }
                }
                startGame = true;
                youLost = false;
            }

            //play game
            if (startGame && !lifeLost && !youLost)
            {
                //player controls
                player.Move();
                player.Shoot(gameTime, zeroProjectile, framesProjectile, projectiles, projectileImage);

                //centipede move
                for (int i = 0; i < head.Count; i++)
                {
                    if (head[i].body.Count >= 1)
                    {
                        if (zero >= frames && head[i].body.Count >= 1)
                        {
                            head[i].body[0].Move(head, head[i].body, newBody);
                            zero = TimeSpan.Zero;
                        }
                    }
                    head[i].Move(asteroids, startGame, head, head[i].body, i);
                    head[i].Damage(projectiles, head, headImage, projectileImage, bodyDImage, newBody, i, score);
                }
                if (head.Count >= 2)
                {
                    for (int j = 1; j < head.Count; j++)
                    {
                        head[j].NewCentipede(head, j - 1);
                    }
                }
                /*for (int i = 1; i < body.Count; i++)
                {
                    body[i].MoveBody(head, body, i);
                }*/

                //spider move
                for (int i = 0; i < spiders.Count; i++)
                {
                    spiders[i].Move(player, gameTime, ref up, ref down);
                }

                //spider kill
                for (int i = 0; i < projectiles.Count; i++)
                {
                    for (int j = 0; j < spiders.Count; j++)
                    {
                        if (projectiles[i].hitbox.Intersects(spiders[j].hitbox))
                        {
                            //spider hit
                            spiders[j].Damage(number);
                            score = score + 600;
                            projectiles.RemoveAt(i);
                        }
                    }
                }

                //asteroid hit
                for(int j = 0; j < asteroids.Count; j++)
                {
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        bool asteroidDead = false;

                        if (projectiles[i].hitbox.Intersects(asteroids[j].hitbox))
                        {                            
                            asteroidDead = asteroids[j].AsteroidDamage();
                            projectiles.RemoveAt(i);
                            if (asteroidDead)
                            {
                                asteroids.RemoveAt(j);
                            }
                        }
                    }
                }

                //player damage
                for(int i = 0; i < spiders.Count; i++)
                {
                    if (player.hitbox.Intersects(spiders[i].hitbox))
                    {
                        if (lives.Count > 0)
                        {
                            lives.RemoveAt(lives.Count - 1);
                            lifeLost = true;
                            spiders[i].Damage(number);
                            head.Clear();
                            head.Add(new Centipede(headImage, 525, 0, 25, 25, Color.White, new Vector2((float)2.5, (float)25)));
                            head[0].AddBody(bodyDImage, 25);
                        }
                    }
                }
                for(int i = 0; i < head.Count; i++)
                {
                    if (player.hitbox.Intersects(head[i].hitbox))
                    {
                        lives.RemoveAt(lives.Count - 1);
                        lifeLost = true;
                        head.Clear();
                        head.Add(new Centipede(headImage, 525, 0, 25, 25, Color.White, new Vector2((float)2.5, (float)25)));
                        head[0].AddBody(bodyDImage, 25);
                    }
                }
            }

            if (head.Count == 0)
            {
                number++;
                for (int i = asteroids.Count; i < asteroidCount; i++)
                {
                    int X = random.Next(20);
                    asteroids.Add(new Sprite(asteroidImage, (X * 50) + 50, (i * 50) + 50, 50, 50, Color.White));
                    for (int j = 0; j < asteroids.Count - 1; j++)
                    {
                        if (asteroids[i].GetX() == asteroids[j].GetX() && asteroids[i].GetY() == asteroids[j].GetY())
                        {
                            asteroids[i].ChangeCord();
                            j = 0;
                        }
                    }
                }
                head.Add(new Centipede(headImage, 525, 0, 25, 25, Color.White, new Vector2((float)2.5, (float)25)));
                head[0].AddBody(bodyDImage, 25 + (5*number));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            //if (example <= exampleWaiter)
            //{
            GraphicsDevice.Clear(Color.MidnightBlue);

            //}

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //start screen
            if(startGame == false && lifeLost == false)
            {
                spriteBatch.DrawString(titleFont, "CENTIPEDE", new Vector2(175, 100), Color.LimeGreen);
                spriteBatch.DrawString(textFont, "Press the ENTER key to start.", new Vector2(425, 300), Color.Fuchsia);

                spriteBatch.Draw(headImage, new Vector2(637, 400), Color.White);

                for(int i = 0; i < 15; i++)
                {
                    spriteBatch.Draw(bodyFImage, new Vector2(620 - 17 * i, 400), Color.White);
                }
            }

            //lost life
            if (lifeLost == true && lives.Count > 0)
            {
                flameSE.DrawReflect(spriteBatch, SpriteEffects.None);
                flameSW.DrawReflect(spriteBatch, SpriteEffects.None);
                flameNW.DrawReflect(spriteBatch, SpriteEffects.FlipVertically);
                flameNE.DrawReflect(spriteBatch, SpriteEffects.FlipVertically);
                if (lifeLost == true)
                {
                    flameSE.MoveFlame(flameSE, flameSW, flameNW, flameNE);
                }

                spriteBatch.DrawString(titleFont, "YOU DIED", new Vector2(250, 200), Color.White);
                spriteBatch.DrawString(textFont, $"You have {lives.Count} lives left. Press ENTER to continue.", new Vector2(400, 400), Color.White);

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    lifeLost = false;
                }
            }

            //dead
            if(lives.Count == 0)
            {
                if(score > highScore)
                {
                    highScore = score;
                }
                youLost = true;
                startGame = false;
                spriteBatch.DrawString(titleFont, "GAME OVER", new Vector2(200, 200), Color.White);
                spriteBatch.DrawString(textFont, $"Your high score is {highScore}. Press ENTER to play again.", new Vector2(400, 400), Color.White);
            }

            //during game
            if (startGame && !lifeLost && !youLost) 
            {
                //draw border
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].Draw(spriteBatch);
                }

                //draw player
                player.Draw(spriteBatch);

                //draw centipede
                //1 = left, 2 = right, 3 = other

                for(int i = 0; i < head.Count; i++)
                {
                    if (head[i].Direction() == 1)
                    {
                        for (int j = 0; j < head[i].body.Count; j++)
                        {
                            head[i].body[j].Rotate(spriteBatch, bodyFImage, SpriteEffects.FlipHorizontally);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < head[i].body.Count; j++)
                        {
                            head[i].body[j].Rotate(spriteBatch, bodyFImage, SpriteEffects.None);
                        }
                    }

                    if (head[i].Direction() == 1)
                    {
                        head[i].Rotate(spriteBatch, headImage, SpriteEffects.FlipHorizontally);
                    }
                    else if (head[i].Direction() == 2)
                    {
                        head[i].Rotate(spriteBatch, headImage, SpriteEffects.None);
                    }
                    else if (head[i].Direction() == 3)
                    {
                        head[i].Rotate(spriteBatch, headDImage, SpriteEffects.None);
                    }
                }

                //draw asteroids
                for(int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i].Draw(spriteBatch);
                }

                //draw projectiles
                for(int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Draw(spriteBatch);
                }

                //draw spiders
                for(int i = 0; i < spiders.Count; i++)
                {
                    spiders[i].Draw(spriteBatch);
                }
                

                //draw lives
                for(int i = 0; i < lives.Count; i++)
                {
                    lives[i].Draw(spriteBatch);
                }

                //draw score
                spriteBatch.DrawString(scoreFont, $"{score}", new Vector2(525, 25), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
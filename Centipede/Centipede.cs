using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Centipede
{
    internal class Centipede : Sprite
    {
        public Vector2 speed;
        int direction = 0; //0 is down, 1 is left, 2 is right
        public int segmentNum = 25;
        public List<Body> body = new List<Body>();

        public Centipede(Texture2D texture, int x, int y, int width, int height, Color color, Vector2 speed) : base(texture, x, y, width, height, color)
        {
            this.speed = speed;
        }

        public void AddBody(Texture2D image, int size)
        {
            for(int i = 0; i < size; i++)
            {
                body.Add(new Body(image, -100, -100, 25, 25, Color.White, new Vector2((float)2.5, (float)2.5), 0));
            }
        }

        public void Rotate(SpriteBatch spriteBatch, Texture2D Texture, SpriteEffects effect)
        {
            spriteBatch.Draw(Texture, new Rectangle(x, y, width, height), null, Color.White, 0, new Vector2(0, 0), effect, 0);
        }

        /*for (int i = snakeBody.Count - 1; i > 0; i--)
            {
                snakeBody[i].moveSnakeBody(snakeBody[i - 1].xPosition, snakeBody[i - 1].yPosition);
            }
            if (snakeBody.Count > 0)
            {
                snakeBody[0].moveSnakeBody(xPosition, yPosition);
            }*/

        public int Direction()
        {
            if(speed.X <= 0)
            {
                return 1;
            }
            else if(speed.X >= 0)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public void Move(List<Sprite> asteroids, bool startGame, List<Centipede> head, List<Body> body, int number)
        {
            for (int j = 0; j < asteroids.Count; j++)
            {
                if (hitbox.Intersects(asteroids[j].hitbox))
                {
                    speed.X = speed.X * -1;
                    y = y + (int)speed.Y;
                }
            }
            if (x <= 0)
            {
                x = 1;
                speed.X = (float)2.5;
                y = y + (int)speed.Y;
            }
            else if(x + width >= 1100)
            {
                x = 1074;
                speed.X = (float)-2.5;
                y = y + (int)speed.Y;
            }

            if(y < 0)
            {
                y = 26;
                speed.Y = speed.Y * -1;
                y = y + (int)speed.Y;
            }
            else if(y + height > 600)
            {
                y = 574;
                speed.Y = speed.Y * -1;
                y = y + (int)speed.Y;
            }
            for (int i = 0; i < head.Count; i++)
            {
                /*if (head[i].x == x && head[i].y == y && i != number)
                {
                    head[i].x = x + (int)speed.X;
                    x = x - (2 * (int)speed.X);
                }*/
                if (head[i].hitbox.Intersects(hitbox) && i != number)
                {
                    /*speed.X = speed.X * -1;
                    x = x + (2 * (int)speed.X);
                    if (x < 0)
                    {
                        x = 0;
                    }
                    else if (x + width > 1100)
                    {
                        x = 1075;
                    }
                    y = y + (int)speed.Y;*/
                    speed.X = speed.X * -1;
                    x = x + (int)speed.X;
                    head[i].speed.X = head[i].speed.X * -1;
                    head[i].x = head[i].x + (int)head[i].speed.X;
                }
                for (int j = 0; j < head[i].body.Count; j++)
                {
                    if (head[i].body[j].hitbox.Intersects(hitbox) && i != number)
                    {
                        y = y + (int)speed.Y;
                    }
                }
            }

            x = x + (int)speed.X;
        }

        public void Damage(List<Player> projectile, List<Centipede> head, Texture2D image, Texture2D projectImage, Texture2D bodyImage, int bodyHit, int l, int score)
        {
            for(int i = 0; i < projectile.Count; i++)
            {
                for (int j = 0; j < body.Count; j++)
                {
                    if (projectile[i].hitbox.Intersects(body[j].hitbox))
                    {
                        head.Add(new Centipede(image, body[j].x, body[j].y + 25, 25, 25, Color.White, new Vector2((float)2.5, (float)25)));
                        head[^1].AddBody(bodyImage, body.Count - j - 1);
                        body.RemoveRange(j, body.Count - j);
                        projectile.RemoveAt(i);
                        score = score + 750;
                    }
                }
            }
            for(int i = 0; i < projectile.Count; i++)
            {
                if (projectile[i].hitbox.Intersects(head[l].hitbox))
                {
                    if (body.Count >= 1)
                    {
                        x = body[0].x;
                        y = body[0].y;
                        body.RemoveAt(0);
                        projectile.RemoveAt(i);
                        score = score + 1000;
                    }
                    else
                    {
                        x = -100;
                        y = -100;
                        if(x <= -50 || x >= 1100 || y <= -50 || y >= 600)
                        {
                            head[l].body.Clear();
                            head.RemoveAt(l);
                        }
                        projectile.RemoveAt(i);
                        score = score + 1500;
                    }
                }
            }
            for (int i = 0; i < head.Count; i++)
            {
                if (head[i].x <= -50 || head[i].x >= 1100 || head[i].y <= -50 || head[i].y >= 600)
                {
                    head.RemoveAt(i);
                }
            }
        }

        public bool Hit(List<Player> projectile, List<Centipede> head)
        {
            for (int i = 0; i < projectile.Count; i++)
            {
                for (int j = 0; j < body.Count; j++)
                {
                    if (projectile[i].hitbox.Intersects(body[j].hitbox))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public void NewCentipede(List<Centipede> head, int previous)
        {
            if (head[previous].direction == 1)
            {
                direction = 2;
            }
            else if (head[previous].direction == 2)
            {
                direction = 1;
            }
        }

        public void faster()
        {
            speed.X = speed.X + 1;
        }
    }
}

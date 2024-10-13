using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centipede
{
    internal class Spider : Sprite
    {
        float speed;
        int health;

        public Spider(Texture2D texture, int x, int y, int width, int height, Color color, int speed, int health) : base(texture, x, y, width, height, color)
        {
            this.speed = speed;
            this.health = health;
        }

        public void Move(Player player, GameTime gameTime, ref TimeSpan up, ref TimeSpan down)
        {
            int playerX = player.GetX();
            int playerY = player.GetY();
            TimeSpan upFrame = new TimeSpan(0, 0, 0, 1);
            TimeSpan downFrame = new TimeSpan(0, 0, 0, 4);

            if (up <= upFrame)
            {
                y = y - (int)speed;
            }
            else if (x != playerX && y != playerY && down <= downFrame)
            {
                if (x <= playerX && y <= playerY)
                {
                    x = x + (int)speed;
                    y = y + (int)speed;
                }
                else if(x <= playerX && y > playerY + 50)
                {
                    x = x + (int)speed;
                }
                else if (x >= playerX + 50 && y < playerY)
                {
                    x = x - (int)speed;
                    y = y + (int)speed;
                }
                else if (x >= playerX + 50 && y > playerY + 50)
                {
                    x = x - (int)speed;
                }
                else
                {
                    y = y + (int)speed;
                }
            }
            else
            {
                up = TimeSpan.Zero;
                down = TimeSpan.Zero;
            }

            /*if(x != playerX && y != playerY)
            {
                if (x < playerX)
                {
                    x = x + speed;
                }
                else if(x > playerX + 50)
                {
                    x = x - speed;
                }
                else
                {
                    x = x + speed;
                }

                if (y < playerY)
                {
                    y = y + speed;
                }
                else if (y > playerY + 50)
                {
                    y = y - speed;
                }
                else if (y == playerY)
                {
                    y = y + 10;
                    y = y - 10;
                }
            }*/

        }

        public void Damage(int number)
        {
            health = 0;
            if (health == 0)
            {
                Random random = new Random();
                int side = random.Next(10);
                if(side > 5)
                {
                    x = -100;
                }
                else if(side <= 5)
                {
                    x = 1200;
                }
                y = -100;
                health = 15;
            }
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }
    }
}

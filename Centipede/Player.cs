using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centipede
{
    internal class Player : Sprite
    {
        int speed;
        bool projectileMove = true;
        TimeSpan timeSpan;

        public Player(Texture2D texture, int x, int y, int width, int height, Color color, int speed) : base(texture, x, y, width, height, color)
        {
            this.speed = speed;
        }

        public void Move()
        {
            if(x > -10 && x < 1060 && y > 440 && y < 560)
            {
                x = Mouse.GetState().X - 25;
                y = Mouse.GetState().Y - 25;
            }
            if(x < 0)
            {
                x = 1;
            }
            else if (x > 1050)
            {
                x = 1049;
            }
            if(y < 450)
            {
                y = 451;
            }
            else if (y > 550)
            {
                y = 549;
            }
        }
        public void Shoot(GameTime gameTime, TimeSpan zero, TimeSpan frames, List<Player> projectile, Texture2D image)
        {
            bool moveProjectile = true;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && projectile[projectile.Count - 1].y <= -100)
            {
                projectile.Add(new Player(image, -100, -100, 15, 15, Color.White, 25));
                projectile[projectile.Count - 1].x = x + 17;
                projectile[projectile.Count - 1].y = y + 17;
                moveProjectile = true;
            }

            if(moveProjectile == true && zero >= frames)
            {
                projectile[projectile.Count - 1].y = projectile[projectile.Count - 1].y - projectile[projectile.Count - 1].speed;
                zero = TimeSpan.Zero;
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

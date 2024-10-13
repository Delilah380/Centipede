using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Centipede
{
    internal class Sprite
    {
        protected Texture2D texture;
        protected int x;
        protected int y;
        protected int width;
        protected int height;
        protected Color color;
        public Rectangle hitbox { get { return new Rectangle(x, y, width, height); } }

        public Sprite(Texture2D texture, int x, int y, int width, int height, Color color)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        //draw sprites
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(x, y, width, height), color);
        }

        public void DrawReflect(SpriteBatch spriteBatch, SpriteEffects effects)
        {
            spriteBatch.Draw(texture, hitbox, null, Color.White, 0, new Vector2(0, 0), effects, 0);
        }

        //move x and y
        public void Move(int newx, int newy)
        {
            x = newx;
            y = newy;
        }

        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }

        //move flame in death screen
        public void MoveFlame(Sprite flameSE, Sprite flameSW, Sprite flameNW, Sprite flameNE)
        {
            flameSE.x = flameSE.x + 10;
            flameSE.y = flameSE.y - 5;

            flameSW.x = flameSW.x - 10;
            flameSW.y = flameSW.y - 5;

            flameNW.x = flameNW.x - 10;
            flameNW.y = flameNW.y + 5;

            flameNE.x = flameNE.x + 10;
            flameNE.y = flameNE.y + 5;
        }

        public bool AsteroidDamage()
        {
            width = width - 5;
            height = height - 5;
            if(width == 5 && height == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeCord()
        {
            Random random = new Random();
            x = (random.Next(20) * 50) + 50;
            y = random.Next(8) * 50;
        }
    }
}

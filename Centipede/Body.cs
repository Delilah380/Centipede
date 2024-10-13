using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Centipede
{
    internal class Body : Centipede
    {
        Centipede centipede;
        public int headNum;
        public Body(Texture2D texture, int x, int y, int width, int height, Color color, Vector2 speed, int headnum) : base(texture, x, y, width, height, color, speed)
        {
            
        }

        public void Move(List<Centipede> head, List<Body> body, int newBody)
        {
            if(body.Count > 0)
            {
                for (int i = 0; i < head.Count; i++)
                {
                    if (head[i].body.Count == 1)
                    {
                        head[i].body[0].x = head[i].GetX();
                        head[i].body[0].y = head[i].GetY();
                    }
                    else if (head[i].body.Count > 1)
                    {
                        for (int j = head[i].body.Count - 1; j > 0; j--)
                        {
                            head[i].body[j].x = head[i].body[j - 1].x;
                            head[i].body[j].y = head[i].body[j - 1].y;
                        }
                        head[i].body[0].x = head[i].GetX();
                        head[i].body[0].y = head[i].GetY();
                    }
                }
            }
        }
    }
}

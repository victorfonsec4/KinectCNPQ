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

namespace KinectCNPQ
{
    class Zombie
    {
        int vida;
        Vector3 posicao;
        Texture2D textura;

        public Zombie(int vida, Vector3 posicao)
        {
            this.vida = vida;
            this.posicao = posicao;
        }

        public void LoadTexture(ContentManager Content)
        {
            textura = Content.Load<Texture2D>("black");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, new Vector2(posicao.X, posicao.Y), Color.White); 
        }

        public bool isHit(Point mousePos)
        {
            Rectangle zombieBounds = new Rectangle( (int)posicao.X, (int)posicao.Y, (int)textura.Width, (int)textura.Height);
            return zombieBounds.Contains(mousePos);
        }

        public void Morrer()
        {
        }
    }
}

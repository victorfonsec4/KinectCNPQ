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
        Vector3 posicao;//a posicao Z é a distância do zombie até a cerca que o jogador deve defender varia de 10-0
        Texture2D textura;
        float velocidade;
        float tam;//usado para redimensionar o tamanho da textura e dar a impressão que o zombie está se aproximando(fazendo ele ficar maior) varia de 0.5-5
        float altura, largura;//dimensoes da textura do zombie apos ser multiplicada por tam

        public Zombie(int vida, Vector3 posicao, float velocidade)
        {
            this.vida = vida;
            this.posicao = posicao;
            this.velocidade = velocidade;
            tam = (float)0.1;
        }

        public void Update()
        {
            if(posicao.Z > 0)
                posicao.Z -= velocidade;
            tam = (float)(0.5 + 5*(10-posicao.Z)/10);
            altura = textura.Height*tam;
            largura = textura.Width*tam;
        }

        public void LoadTexture(ContentManager Content)
        {
            textura = Content.Load<Texture2D>("black");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura,new Rectangle( (int)(posicao.X - altura/2), (int)(posicao.Y - largura/2), (int)altura, (int)largura), null, Color.White); 
        }

        public bool isHit(Point mousePos)
        {
            Rectangle zombieBounds = new Rectangle((int)(posicao.X - altura / 2), (int)(posicao.Y - largura / 2), (int)altura, (int)largura);
            return zombieBounds.Contains(mousePos);
        }

        public void Morrer()
        {
        }
    }
}

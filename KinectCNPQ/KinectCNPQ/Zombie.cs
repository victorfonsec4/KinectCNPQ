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
        public int dano;//dano que o zombie causa na vida da barreira qnd ele esta com posicao.z = 0
        Vector3 posicao;//a posicao Z é a distância do zombie até a cerca que o jogador deve defender varia de 10-0
        Texture2D textura;
        float velocidade;
        float tam;//usado para redimensionar o tamanho da textura e dar a impressão que o zombie está se aproximando(fazendo ele ficar maior) varia de 0.5-5
        float altura, largura;//dimensoes da textura do zombie apos ser multiplicada por tam
        public bool marcado;//o jogador vai passar a mao por cima daonde vai atirar e depois fazer alguma acao pra atirar e todos os zombies q foram marcados vao levar dano

        public Zombie(int vida, int dano, Vector3 posicao, float velocidade)
        {
            this.vida = vida;
            this.dano = dano;
            this.posicao = posicao;
            this.velocidade = velocidade;
            marcado = false;
        }

        public void Update()
        {
            if (posicao.Z > 0)
                posicao.Z -= velocidade;
            tam = (float)(0.5 * (1 + (10 - posicao.Z)));
            altura = 100 * tam;
            largura = 100 * tam;
        }

        public void LoadTexture(ContentManager Content)
        {
            textura = Content.Load<Texture2D>("zombie");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura,new Rectangle( (int)(posicao.X - altura/2), (int)(posicao.Y - largura/2), (int)altura, (int)largura), null, Color.White); 
        }

        public bool isMarcado(Point mousePos)
        {
            Rectangle zombieBounds = new Rectangle((int)(posicao.X - altura / 2), (int)(posicao.Y - largura / 2), (int)altura, (int)largura);
            return zombieBounds.Contains(mousePos);
        }

        public void Morrer()
        {
        }

        public int DistanciaAteJogador()
        {
            return ((int)posicao.Z);
        }
    }
}

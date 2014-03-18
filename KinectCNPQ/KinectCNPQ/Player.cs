using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KinectCNPQ
{
    class Player
    {
        public int vida;
        public bool vivo;
        public Vector2 pos;
        public Vector2 quadrado;

        public Player(int vida)
        {
            this.vida = vida;
            vivo = true;
        }

        public void LevarDano(int dano)
        {
            vida -= dano;
            if (vida <= 0)
                vivo = false;
        }

        public void atualizarQuadrado(Queue<Vector2> ultimasPos)
        {
            float xmin = 10, xmax = -10, ymin = 10, ymax = -10;
            foreach (Vector2 posicao in ultimasPos)
            {
                xmin = Math.Min(xmin, posicao.X);
                ymin = Math.Min(ymin, posicao.Y);
                xmax = Math.Max(xmax, posicao.X);
                ymax = Math.Max(ymax, posicao.X);
            }
            xmax += xmin;
            ymax += ymin;
            this.quadrado.X = xmax;
            this.quadrado.Y = ymax;
        }
    }
}

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
    }
}

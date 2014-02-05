using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectCNPQ
{
    class Player
    {
        public int vida;
        public bool vivo;

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

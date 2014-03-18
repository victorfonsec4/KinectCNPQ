using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using System.Diagnostics;

namespace KinectCNPQ
{
    class Player
    {
        public int vida;
        public bool vivo;
        public Vector2 posMao;
        public Vector2 posRelQuad;
        public Vector2 quadrado;//inicio em(0,0) fim em (quadrado.x, quadrado.y)

        public Player(int vida)
        {
            this.vida = vida;
            vivo = true;
            posMao = new Vector2(0,0);
            quadrado = new Vector2(0, 0);
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
                ymax = Math.Max(ymax, posicao.Y);
            }
            //Debug.WriteLine(xmin + " " + xmax + " " + ymin + " " + ymax);
            xmax -= xmin;
            ymax -= ymin;
            this.quadrado.X = xmax;
            this.quadrado.Y = ymax;
            this.posRelQuad.X = posMao.X - xmin;
            this.posRelQuad.Y = posMao.Y - ymin;
            Debug.WriteLine(quadrado.X + " " + quadrado.Y + " " + posRelQuad.X + " " + posRelQuad.Y);
        }

        public void setPos(Skeleton skeleton)
        {
            if (skeleton != null)
            {
                float x = 0, y = 0;
                foreach (Joint joint in skeleton.Joints)
                {
                    if (joint.JointType == JointType.HandLeft || joint.JointType == JointType.HandRight)
                    {
                        x += joint.Position.X;
                        y += joint.Position.Y;
                    }
                }
                this.posMao.X = x / 2;
                this.posMao.Y = y / 2;
                //Debug.WriteLine(this.pos.X + " " + this.pos.Y);
            }
        }
    }
}

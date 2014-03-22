using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace KinectCNPQ
{
    class Player
    {
        public int vida;
        public bool vivo;
        private Texture2D aimTexture;
        private Vector2 posMao;
        private Vector2 posRelQuad;
        public Vector2 pos;
        private Vector2 quadrado;//inicio em(0,0) fim em (quadrado.x, quadrado.y)
        private Queue<Vector2> ultimasPos900;

        public Player(int vida)
        {
            this.vida = vida;
            vivo = true;
            posMao = new Vector2(0,0);
            posRelQuad = new Vector2(0, 0);
            pos = new Vector2(0, 0);
            quadrado = new Vector2(0, 0);
            ultimasPos900 = new Queue<Vector2>();
            for (int i = 0; i < 900; i++)
                ultimasPos900.Enqueue(new Vector2(0,0));
        }

        public void LoadTexture(ContentManager Content)
        {
            aimTexture = Content.Load<Texture2D>("aim");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(aimTexture, new Vector2(pos.X - aimTexture.Width/2, pos.Y - aimTexture.Height/2), Color.White); 
        }

        public void LevarDano(int dano)
        {
            vida -= dano;
            if (vida <= 0)
                vivo = false;
        }

        public void atualizarQuadrado()
        {
            float xmin = 10, xmax = -10, ymin = 10, ymax = -10;
            this.posMao.X = 0;
            this.posMao.Y = 0;
            List<float> pesos = new List<float>();
            float soma = 0;
            for (int i = 0; i < 30; i++)//seta pesos maior para posicoes mais recentes
            {
                //Debug.WriteLine((float)1 / (float)((30 - i)*(30 - i)));
                pesos.Add((float)1 / (float)((30 - i)*(30 - i)));
                soma += (float)1 / (float)(((30 - i)*(30-i)));
            }
            int k = 0;
            foreach (Vector2 posicao in this.ultimasPos900)
            {
                if (k >= 871)//pega ultimos 30 pesos(30 mais recentes)
                {
                    posMao.X += posicao.X * pesos[k - 871];
                    posMao.Y += posicao.Y * pesos[k - 871];
                    //Debug.WriteLine(posicao.X * pesos[k - 872]);
                }
                xmin = Math.Min(xmin, posicao.X);
                ymin = Math.Min(ymin, posicao.Y);
                xmax = Math.Max(xmax, posicao.X);
                ymax = Math.Max(ymax, posicao.Y);
                k++;
            }
            posMao.X /= soma;
            posMao.Y /= soma;
            Debug.WriteLine(posMao.ToString());
            //Debug.WriteLine(xmin + " " + xmax + " " + ymin + " " + ymax);
            xmax -= xmin;
            ymax -= ymin;
            this.quadrado.X = xmax;
            this.quadrado.Y = ymax;
            posRelQuad.X = posMao.X - xmin;
            posRelQuad.Y = posMao.Y - ymin;
            //Debug.WriteLine(quadrado.X + " " + quadrado.Y + " " + posRelQuad.X + " " + posRelQuad.Y);
        }

        public void setPos(Skeleton skeleton, Viewport view)
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

                ultimasPos900.Enqueue(new Vector2(x/2, y/2));
                this.atualizarQuadrado();//atualiza quadrado baseado nos ultimos 30 segundos de dados
                ultimasPos900.Dequeue();

                pos.X = (posRelQuad.X / quadrado.X) * (view.Width);
                pos.Y = (posRelQuad.Y / quadrado.Y) * (view.Height);
                pos.Y = view.Height - pos.Y;
                //Debug.WriteLine(this.pos.X + " " + this.pos.Y);
            }
        }
    }
}

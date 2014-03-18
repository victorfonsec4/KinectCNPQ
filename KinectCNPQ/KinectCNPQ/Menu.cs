using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace KinectCNPQ
{
    class Menu
    {
        private Texture2D fundo;
        private int botao;//0 - NovoJogo, 1 - Opcoes, 2 - Sair

        public Menu()
        {
            this.botao = 0;
        }

        public void Update(KeyboardState keyb)
        {

            if (keyb.IsKeyDown(Keys.Down))
                botao = (botao + 1) % 3;
            else if (keyb.IsKeyDown(Keys.Up))
                botao = (botao + 2) % 3;
        }

        public void LoadTexture(ContentManager Content)
        {
            fundo = Content.Load<Texture2D>("menuBackground");
        }

        public void Draw(SpriteBatch spriteBatch,GraphicsDevice graphicsDev)
        {
            spriteBatch.Draw(fundo, new Rectangle(0,0,graphicsDev.Viewport.Width,graphicsDev.Viewport.Height), null, Color.White);
        }
    }
}

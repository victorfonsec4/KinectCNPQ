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
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using XnaInp = Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;

namespace KinectCNPQ
{
    static class NativeMethods
    {
        public static Cursor LoadCustomCursor(string path)
        {
            IntPtr hCurs = LoadCursorFromFile(path);
            if (hCurs == IntPtr.Zero) throw new Win32Exception();
            var curs = new Cursor(hCurs);
            // Note: force the cursor to own the handle so it gets released properly
            var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(curs, true);
            return curs;
        }
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //coisas do kinect
        KinectSensor kinect;
        Skeleton[] skeletonData;
        Skeleton skeleton;
        Texture2D jointTexture;
        Queue<Vector2> ultimasPos;

        List<Zombie> inimigos;
        Player player;

        int time;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            //inicializa o kinect
            kinect = KinectSensor.KinectSensors[0];
            kinect.SkeletonStream.Enable();
            kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);
            kinect.Start();

            //inicializa o cursor
            Cursor myCursor = NativeMethods.LoadCustomCursor(@"Content\Cursors\cursor.cur");
            Form winForm = (Form)Form.FromHandle(this.Window.Handle);
            winForm.Cursor = myCursor;
            time = 0;

            //inicializa zumbis
            inimigos = new List<Zombie>();
            Zombie zumbi = new Zombie(100, 1, new Vector3(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 10), (float)0.01);
            inimigos.Add(zumbi);

            player = new Player(100);
            ultimasPos = new Queue<Vector2>();
            for (int i = 0; i < 900; i++)
                ultimasPos.Enqueue(new Vector2(0,0));

            //inicializa lista com 900 zeros(30hz*30seg)

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            jointTexture = Content.Load<Texture2D>("junta");
            foreach (Zombie zumbi in inimigos)
                zumbi.LoadTexture(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            player.setPos(skeleton);//atualiza a posicao do jogador(Cursor)

            ultimasPos.Enqueue(player.posMao);
            player.atualizarQuadrado(ultimasPos);//atualiza quadrado baseado nos ultimos 30 segundos de dados
            ultimasPos.Dequeue();

            //Debug.WriteLine(player.quadrado.X + " " + player.quadrado.Y);
        

            if (keyboard.IsKeyDown(XnaInp::Keys.Escape))
                this.Exit();

            foreach(Zombie zumbi in inimigos)
                if (zumbi.isMarcado(new Point(mouse.X, mouse.Y)))
                    zumbi.marcado = true;

            if (mouse.LeftButton == XnaInp::ButtonState.Pressed)
                inimigos.RemoveAll(zumbi => zumbi.marcado == true);

            foreach (Zombie zumbie in inimigos)
                zumbie.Update();

            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time >= 3000)
            {
                time = 0;
                foreach (Zombie zumbi in inimigos)
                {
                    if (zumbi.DistanciaAteJogador() == 0)
                        player.LevarDano(zumbi.dano);

                }
            }
            //Debug.WriteLine(player.vida);
            if (!player.vivo) ;
                //acabar jogo
            // TODO: Add your update logic here
            //teste

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (Zombie zumbi in inimigos)
                zumbi.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs imageFrames)
        {
            using (SkeletonFrame skeletonFrame = imageFrames.OpenSkeletonFrame())
                if (skeletonFrame != null)
                {
                    if ((skeletonData == null) || (this.skeletonData.Length != skeletonFrame.SkeletonArrayLength))
                        this.skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    //Copy the skeleton data to our array
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);
                }

            if (skeletonData != null)
                foreach (Skeleton skel in skeletonData)
                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        skeleton = skel;
        }
    }
}

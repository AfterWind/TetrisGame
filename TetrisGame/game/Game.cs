using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using TetrisGame.game;

namespace TetrisGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TetrisGame : Game {

        public GraphicsDeviceManager graphics;
        public bool gameStarted;
        
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        private KeyboardState oldState;

        private Texture2D background;

        public TetrisGame() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();
            gameStarted = false;
            InitializeScreen();
        }

        private void InitializeScreen() {
            graphics.PreferredBackBufferHeight = GraphicUtils.screenHeight;
            graphics.PreferredBackBufferWidth = GraphicUtils.screenWidth;
            Type type = typeof(OpenTKGameWindow);
            System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            OpenTK.GameWindow window = (OpenTK.GameWindow)field.GetValue(Window);
            if (window != null) {
                window.X = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - graphics.PreferredBackBufferWidth) / 2;
                window.Y = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - graphics.PreferredBackBufferHeight) / 2 - 50;
            }
        }

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameObjects.Init(Content, GraphicsDevice, this);
            basicEffect = new BasicEffect(GraphicsDevice);
            background = this.Content.Load<Texture2D>("bg");
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            // TODO: Add your update logic here
            Stopwatch sw = new Stopwatch();
            sw.Start();
            GetInput();
            if (gameStarted) {
                GameObjects.Update();
            }
            
            base.Update(gameTime);
            
            sw.Stop();
            Console.WriteLine("Finished tick with " + sw.ElapsedMilliseconds);
            if(sw.ElapsedMilliseconds > 17)
                Console.WriteLine("Breakpoint.");
        }

        public bool WasKeyPressed(Keys key) {
            foreach (Keys k in oldState.GetPressedKeys()) {
                if (k == key)
                    return true;
            }
            return false;
        }

        protected void GetInput() {
            KeyboardState currentState = Keyboard.GetState();
            if (currentState.GetPressedKeys().Length > 0) {
                foreach (Keys k in currentState.GetPressedKeys()) {
                    switch (k) {
                        case Keys.Escape:
                            //this.Exit();
                            if (gameStarted && !GameObjects.GamePaused) {
                                ButtonWindow buttonWindow = new ButtonWindow(150, new TitleReturnButton("Da"), new GameReturnButton("Nu"));
                                buttonWindow.Title = "Iesi   din   joc?";
                                GameObjects.PauseGame(buttonWindow);
                            }
                            break;
                        case Keys.L:
                            GameObjects.GetBoard().PrintMap();
                            break;
                        case Keys.R:
                            GameObjects.GetBoard().ResetMap();
                            break;
                    }
                    if (!WasKeyPressed(k)) {
                        switch (k) {
                            case Keys.P:
                                ButtonWindow buttonWindow = new ButtonWindow(150, new GameReturnButton("Intoarcete"));
                                buttonWindow.Title = "Pauza";
                                GameObjects.PauseGame(buttonWindow);
                                break;
                            case Keys.D:
                                GameObjects.SelectNextBoard();
                                break;
                            case Keys.A:
                                GameObjects.SelectPreviousBoard();
                                break;
                            case Keys.G:
                                Config.isGridEnabled = !Config.isGridEnabled;
                                break;
                        }
                    }
                    
                }
                GameObjects.OnKeyPressed(currentState);
            }
            oldState = currentState;
        }

        protected override void Draw(GameTime gameTime) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // These three lines are required if you use SpriteBatch, to reset the states that it sets
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // Transform your model to place it somewhere in the world
            basicEffect.World = Matrix.CreateRotationZ(MathHelper.PiOver4) * Matrix.CreateTranslation(0.5f, 0, 0); // for sake of example
            //basicEffect.World = Matrix.Identity; // Use this to leave your model at the origin
            // Transform the entire world around (effectively: place the camera)
            basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, -3), Vector3.Zero, Vector3.Up);
            // Specify how 3D points are projected/transformed onto the 2D screen
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                    (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height, 1.0f, 100.0f);

            // Tell BasicEffect to make use of your vertex colors
            basicEffect.VertexColorEnabled = true;
            // I'm setting this so that *both* sides of your triangle are drawn
            // (so it won't be back-face culled if you move it, or the camera around behind it)
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            // Render with a BasicEffect that was created in LoadContent
            // (BasicEffect only has one pass - but effects in general can have many rendering passe

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();
                spriteBatch.Begin();
                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.End();
                GameObjects.Draw(spriteBatch);
                base.Draw(gameTime);
            }
            sw.Stop();
            Console.WriteLine("Finished drawing with " + sw.ElapsedMilliseconds);

        }
    }
}

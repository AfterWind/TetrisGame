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
    public class Game1 : Game {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        private KeyboardState oldState;

        private Texture2D background;

        private bool gameStarted;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1600;
            gameStarted = false;
        }   

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameObjects.Init(Content, GraphicsDevice, this);
            basicEffect = new BasicEffect(GraphicsDevice);
            background = this.Content.Load<Texture2D>("bg");
            //spriteFont = this.Content.Load<SpriteFont>("CourierNew");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
       
        protected override void Update(GameTime gameTime) {
            // TODO: Add your update logic here
            Stopwatch sw = new Stopwatch();
            sw.Start();
            GetInput();
            if (gameStarted) {
                foreach (Board board in GameObjects.Boards) {
                    board.Update();
                    if (board.HasLost) {
                        board.ResetMap();
                    }
                }
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
                            this.Exit();
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
                            case Keys.NumPad2:
                                if (GameObjects.GetBoard().Speed - 2 > 0) {
                                    GameObjects.GetBoard().DebugIncreaseSpeed(-2);
                                }
                                break;
                            case Keys.NumPad8:
                                GameObjects.GetBoard().DebugIncreaseSpeed(2);
                                break;
                            case Keys.Right:
                                GameObjects.GetBoard().MoveShape(Block.Size);
                                break;
                            case Keys.Left:
                                GameObjects.GetBoard().MoveShape(-Block.Size);
                                break;
                            case Keys.Down:
                                GameObjects.GetBoard().IncreaseSpeed(GameObjects.GetBoard().Speed);
                                break;
                            case Keys.Space:
                                GameObjects.GetBoard().RotateShape();
                                break;
                            case Keys.P:
                                GameObjects.GetBoard().Paused = GameObjects.GetBoard().Paused ? false : true;
                                break;
                            case Keys.D:
                                GameObjects.SelectNextBoard();
                                break;
                            case Keys.A:
                                GameObjects.SelectPreviousBoard();
                                break;
                        }
                    }
                    
                }
                GameObjects.OnKeyPressed(currentState);
            }
            oldState = currentState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
                if (gameStarted) {
                    foreach (Board board in GameObjects.Boards)
                        board.Draw(spriteBatch, GraphicsDevice);
                } else {
                    // Draw title screen
                    GameObjects.CurrentButtonScreen.Draw(spriteBatch);
                }
                
                base.Draw(gameTime);
            }
            sw.Stop();
            Console.WriteLine("Finished drawing with " + sw.ElapsedMilliseconds);

        }
    }
}

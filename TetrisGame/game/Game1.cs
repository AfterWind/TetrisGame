using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TetrisGame.game;

namespace TetrisGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameObjects.Init(Content);
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
        bool moved = false;
        protected override void Update(GameTime gameTime) {
            // TODO: Add your update logic here

            GameObjects.board1.Update();
            if (GameObjects.board1.hasLost)
                this.Exit();
            KeyboardState state = Keyboard.GetState();
            if (state.GetPressedKeys().Length > 0) {
                foreach (Keys k in state.GetPressedKeys()) {
                    if (!moved) {
                        if (k == Keys.Right) {
                            GameObjects.board1.MoveShape(Block.size);
                            moved = true;
                        } else if (k == Keys.Left) {
                            GameObjects.board1.MoveShape(-Block.size);
                            moved = true;
                        } else {
                            moved = false;
                        }
                    }
                    if (k == Keys.Escape) {
                        this.Exit();
                    } else if(k == Keys.P){

                        GameObjects.board1.PrintMap();

                    }
                }
            } else {
                moved = false;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //GameObjects.Update(spriteBatch);
            GameObjects.board1.Draw(spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

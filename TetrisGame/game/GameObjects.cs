using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisGame.game;

namespace TetrisGame {
    public class GameObjects {

        
        public static readonly int DISTANCE_BETWEEN_BOARDS = 20;
        public static readonly int ROWS = 20, COLUMNS = 16;
        public static readonly int BOARDS_STARTY = 60;

        public static TetrisGame Game { private set; get; }
        public static Color FromColor { private set; get; }
        public static ContentManager Content { private set; get; }
        public static GraphicsDevice GraphicsDevice { private set; get; }
        public static ButtonScreen TitleScreen { private set; get; }
        public static ButtonScreen CurrentButtonScreen { set; get; }
        public static InfoBar InfoBar { private set; get; }
        
        public static Difficulty Difficulty { private set; get; }
        public static bool GamePaused { private set; get; }
        public static ButtonWindow CurrentPauseScreen { private set; get; }

        private static Board[] boards;
        public static Board[] Boards {
            get {
                if(boards == null)
                    InitBoards();
                return boards;
            }
            private set {
                boards = value;
            }
        }

        private static int selectedBoard = 0;
        
        public static void Init(ContentManager content, GraphicsDevice device, TetrisGame game) {
            Content = content;
            GraphicsDevice = device;
            Game = game;


            FromColor = Color.FromNonPremultiplied(96, 96, 96, 255);
            GraphicUtils.fontCommon = content.Load<SpriteFont>("SmallClassic");
            GraphicUtils.fontTitle = content.Load<SpriteFont>("LargeClassic");
            GraphicUtils.pixel = new Texture2D(device, 1, 1);


            ButtonScreen options = new ButtonScreen(300);
            ButtonScreen difficulty;
            Button[] difficultyButtons = new Button[Enum.GetValues(typeof(Difficulty)).Length];
            int i = 0;
            foreach (Difficulty diff in Enum.GetValues(typeof(Difficulty))) {
                difficultyButtons[i++] = new DifficultyButton(diff);
            }
            difficulty = new ButtonScreen(300, difficultyButtons);
            ButtonScreen bs = new ButtonScreen(300, new AdvanceButton("Incepe joc", difficulty), new AdvanceButton("Optiuni", options), new QuitButton("Exit"));
            bs.Title = "MULTI \n  TETRIS";
            CurrentButtonScreen = bs;
            TitleScreen = bs;

            options.buttons.Add(new BackButton(bs));
            difficulty.buttons.Add(new BackButton(bs));

            Difficulty = Difficulty.Normal;
            GamePaused = false;
            InitBoards();
        }

        public static void InitBoards() {
            int nrBoards = Difficulty.GetNumberOfBoards();
            boards = new Board[nrBoards];
            for (int i = 0; i < nrBoards; i++)
                boards[i] = new Board(GetStartX() + i * (COLUMNS * Block.Size + DISTANCE_BETWEEN_BOARDS), BOARDS_STARTY, COLUMNS, ROWS);
            InfoBar = new InfoBar(GetStartX() + nrBoards * (COLUMNS * Block.Size + DISTANCE_BETWEEN_BOARDS), BOARDS_STARTY);
        }

        public static int GetStartX() {
            int nrBoards = Difficulty.GetNumberOfBoards();
            return (GraphicUtils.screenWidth - InfoBar.INFO_BAR_SIZE - (COLUMNS * Block.Size + DISTANCE_BETWEEN_BOARDS) * nrBoards) / 2;
        }

        public static void Draw(SpriteBatch batch) {
            if (Game.gameStarted) {
                if (GamePaused) {
                    CurrentPauseScreen.Draw(batch);
                } else {

                    // Draw the boards
                    foreach (Board board in Boards)
                        board.Draw(batch, GraphicsDevice);

                    // Draw InfoBar
                    InfoBar.Draw(batch);
                }
            } else {
                CurrentButtonScreen.Draw(batch);
            }
        }

        public static void Update() {

            if (GamePaused) {
                
            } else {
                foreach (Board board in Boards) {
                    board.Update();
                    if (board.HasLost) {
                        // TODO: Set up lose condition
                        board.ResetMap();
                    }
                }
            }
        }

        public static void OnKeyPressed(KeyboardState keys) {
            if (Game.gameStarted) {
                if (GamePaused)
                    CurrentPauseScreen.OnKeyboardPress(keys);
                else 
                    boards[selectedBoard].OnKeyPressed(keys);
            } else {
                CurrentButtonScreen.OnKeyboardPress(keys);
            }
        }


        public static void SelectNextBoard() {
            selectedBoard++;
            if (selectedBoard >= boards.Length)
                selectedBoard = 0;
        }
        public static void SelectPreviousBoard() {
            selectedBoard--;
            if (selectedBoard < 0)
                selectedBoard = boards.Length - 1;
        }

        public static Board GetBoard() {
            /*
            if (board1 == null) {
                board1 = new Board(20, 20, 16, 20);
            }
             * */
            return Boards[selectedBoard];
        }

        public static bool IsBoardSelected(Board board) {
            return board == boards[selectedBoard];
        }


        
        public static void SetDifficulty(Difficulty diff) {
            Difficulty = diff;
        }

        
        public static void StartGame() {
            Game.gameStarted = true;
            InitBoards();
        }

        public static void PauseGame(ButtonWindow window) {
            GamePaused = true;
            CurrentPauseScreen = window;
        }

        public static void UnpauseGame() {
            GamePaused = false;
            CurrentPauseScreen = null;
        }
    }

    

    public enum Difficulty {
        Normal,
        DoubleTime,
        Challenge,
        Quadra,
        INFINITE
    }

    public static class DifficultyMethods {
        public static int GetNumberOfBoards(this Difficulty diff) {
            switch (diff) {
                case Difficulty.Normal:
                    return 1;
                case Difficulty.DoubleTime:
                    return 1;
                case Difficulty.Challenge:
                    return 2;
                case Difficulty.Quadra:
                    return 4;
                case Difficulty.INFINITE:
                    return 4;
                default:
                    return 1;
            }
        }

        public static string GetText(this Difficulty diff) {
            return diff.ToString();
        } 
    }
}

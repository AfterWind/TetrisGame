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

        public static TetrisGame Game { private set; get; }
        public static Color FromColor { private set; get; }
        public static ContentManager Content { private set; get; }
        public static GraphicsDevice GraphicsDevice { private set; get; }
        public static ButtonScreen CurrentButtonScreen { set; get; }
        
        public static Difficulty Difficulty { private set; get; }

        public static int Level { private set; get; }
        public static int RowsCleared { private set; get; }

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

        public static void InitBoards() {
            int nrBoards = Difficulty.GetNumberOfBoards();
            boards = new Board[nrBoards];
            for (int i = 0; i < nrBoards; i++)
                boards[i] = new Board(i * (20 + (16 * Block.Size)) + 10, 20, 16, 20);
        }
        
        public static void Init(ContentManager content, GraphicsDevice device, TetrisGame game) {
            Content = content;
            GraphicsDevice = device;
            Game = game;
            FromColor = Color.FromNonPremultiplied(96, 96, 96, 255);
            GraphicUtils.font = content.Load<SpriteFont>("Test");
            GraphicUtils.pixel = new Texture2D(device, 1, 1);

            ButtonScreen options = new ButtonScreen();

            ButtonScreen difficulty;
            Button[] difficultyButtons = new Button[Enum.GetValues(typeof(Difficulty)).Length];
            int i = 0;
            foreach (Difficulty diff in Enum.GetValues(typeof(Difficulty))) {
                difficultyButtons[i++] = new DifficultyButton(diff);
            }
            difficulty = new ButtonScreen(difficultyButtons);

            ButtonScreen bs = new ButtonScreen("HELOOOOOoooo!", new AdvanceButton("Incepe joc", difficulty), new AdvanceButton("Optiuni", options), new QuitButton("Exit"));

            CurrentButtonScreen = bs;

            options.buttons.Add(new BackButton(bs));
            difficulty.buttons.Add(new BackButton(bs));

            Difficulty = Difficulty.Normal;
            InitBoards();
        }

        public static void IncreaseRowsCleared() {
            RowsCleared++;
            if (RowsCleared % 10 == 0)
                Level++;
        }

        
        public static void SelectNextBoard() {
            selectedBoard++;
            if (selectedBoard == 4)
                selectedBoard = 0;
        }
        public static void SelectPreviousBoard() {
            selectedBoard--;
            if (selectedBoard == -1)
                selectedBoard = 3;
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

        public static void OnKeyPressed(KeyboardState keys)
        {
            if(!Game.gameStarted)
                CurrentButtonScreen.OnKeyboardPress(keys);
        }   
        
        public static void SetDifficulty(Difficulty diff) {
            Difficulty = diff;
        }

        
        public static void StartGame() {
            Game.gameStarted = true;
            InitBoards();
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

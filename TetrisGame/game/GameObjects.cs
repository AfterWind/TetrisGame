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
using TetrisGame.game.utils;

namespace TetrisGame {
    public class GameObjects {

        public static Color FromColor { private set; get; }
        public static ContentManager Content { private set; get; }
        public static GraphicsDevice GraphicsDevice { private set; get; }
        public static TreeList<ButtonScreen> ButtonScreenTree { private set; get; } 

        private static Board[] boards;
        
        public static void Init(ContentManager content, GraphicsDevice device) {
            Content = content;
            GraphicsDevice = device;
            FromColor = Color.FromNonPremultiplied(96, 96, 96, 255);
            GraphicUtils.font = content.Load<SpriteFont>("Test");
            GraphicUtils.pixel = new Texture2D(device, 1, 1);

            ButtonScreen bs = new ButtonScreen(new Button("Hello", test), new Button("Yo", test));

            ButtonScreenTree = new TreeList<ButtonScreen>(bs);

            
        }

        public static void test(Keys k) {

        }

        public static Board[] Boards {
            get {
                if (boards == null) {
                    boards = new Board[4];
                    for (int i = 0; i < 4; i++)
                        boards[i] = new Board(i * (20 + (16 * Block.Size)) + 10, 20, 16, 20);
                }
                
                return boards;
            }
            private set {
                boards = value;   
            }
        }
        private static int selectedBoard = 0;
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
        


        public static void Update(SpriteBatch batch) {
            
        }

        
    }







}

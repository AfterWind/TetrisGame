using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisGame.game;

namespace TetrisGame {
    class GameObjects {

        public static Color FromColor { private set; get; }
        public static ContentManager Content { private set; get; }

        // TODO: Allow multiple boards.
        private static Board board1;
        public static Board GetBoard() {
            if(board1 == null)
                board1 = new Board(20, 20, 16, 20);
            return board1;
        }
        
        public static void Init(ContentManager content) {
            Content = content;
            FromColor = Color.FromNonPremultiplied(96, 96, 96, 255);
            GameObjects.GetBoard().AddShape(Utils.GetRandomShape());
        }

        public static void Update(SpriteBatch batch) {

        }

        
    }







}

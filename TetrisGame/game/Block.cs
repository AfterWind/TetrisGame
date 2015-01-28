using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Block {

        public static int size = 20;

        public Color color { private set; get; }
        public Board board;

        public int X { private set; get; }
        public int Y { private set; get; }

        public int GetRelativeX() {
            return (X - board.PosX) / Block.size;
        }

        public int GetRelativeY() {
            return (Y - board.PosY) / Block.size;
        }

        public Block(Board board, Color color, int x, int y) {
            this.X = x;
            this.Y = y;
            this.color = color;
            this.board = board;
        }

        public void Move(int offX, int offY) {
            this.X += offX;
            this.Y += offY;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(GameObjects.PrepareBlockTexture(color), new Rectangle(X, Y, size, size), Color.White);
        }
    }

}

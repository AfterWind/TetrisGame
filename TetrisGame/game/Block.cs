using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Block {

        public static readonly int Size = 20;

        public Texture2D Texture { private set; get; }
        public Board Board { private set; get; }
        public int X { set; get; }
        public int Y { set; get; }

        private Color Color;

        public int GetRelativeX() {
            return (int)Math.Floor((double)((X - Board.PosX) / Block.Size));
        }

        public int GetRelativeY() {
            return (int)Math.Floor((double)((Y - Board.PosY) / Block.Size));
        }

        public Block(Board board, Color color, int x, int y) {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Texture = Utils.GetTextureForColor(color);
            this.Board = board;
        }

        public void Move(int offX, int offY) {
            this.X += offX;
            this.Y += offY;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(Texture, new Rectangle(X, Y, Size, Size), Color.White);
        }
    }

}

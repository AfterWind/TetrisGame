using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Shape {
        public List<Block> blockList;
        public Block center;
        public Board board;

        public Shape(Board board, Color color, int centerX, int centerY, params int[] adjacent) {
            this.board = board;
            blockList = new List<Block>();
            center = new Block(board, color, centerX, centerY);
            blockList.Add(center);
            for (int i = 0; i < adjacent.Length; i += 2) {
                blockList.Add(new Block(board, color, centerX + adjacent[i] * Block.size, centerY + adjacent[i + 1] * Block.size));
            }
        }

        public IEnumerator<Block> GetBottomBlocks() {
            foreach (Block block in blockList) {
                bool ok = true;
                foreach (Block blockBelow in blockList) {
                    if (blockBelow.X == block.X && blockBelow.Y == block.Y - Block.size)
                        ok = false;
                }
                if (ok)
                    yield return block;
            }
        }

        public bool MoveX(int offX) {
            bool ok = true;

            foreach (Block block in blockList) {
                int newX = block.X + offX;
                if (newX < board.PosX)
                    ok = false;
                if (newX + Block.size > board.PosX + board.SizeX) {
                    //block.Move(board.PosX + board.SizeX - newX - Block.size, 0);
                    ok = false;
                }
                Block blockCheck = null;
                try {
                    blockCheck = board.blockArray[(newX - board.PosX) / Block.size, block.GetRelativeY()];
                } catch (IndexOutOfRangeException ex) {

                }
                if (blockCheck != null) {
                    block.Move(board.blockArray[(newX - board.PosX) / Block.size, block.GetRelativeY()].X - block.X - Block.size, 0);
                    ok = false;
                }
            }
            if (!ok)
                return false;

            foreach (Block block in blockList) {
                block.Move(offX, 0);
            }

            return true;
        }

        public bool MoveY(int offY) {
            bool ok = true;
            foreach (Block block in blockList) {
                int newY = block.Y + offY;
                if (newY < board.PosY)
                    ok = false;
                if (newY + Block.size > board.PosY + board.SizeY) {
                    block.Move(0, board.PosY + board.SizeY - block.Y - Block.size);
                    ok = false;
                }
                Block blockCheck = null;
                try {
                    blockCheck = board.blockArray[block.GetRelativeX(), (newY - board.PosY) / Block.size];
                } catch (IndexOutOfRangeException ex) {

                }
                if (blockCheck != null) {
                    block.Move(0, board.blockArray[block.GetRelativeX(), (newY - board.PosY) / Block.size].Y - block.Y - Block.size);
                    ok = false;
                }
            }
            if (!ok)
                return false;

            foreach (Block block in blockList) {
                block.Move(0, offY);
            }

            return true;
        }

        public void Draw(SpriteBatch batch) {
            foreach (Block block in blockList) {
                block.Draw(batch);
            }
        }
    }

    public class Pattern {
        public int centerX, centerY;
        public int[] rest;

        public Pattern(params int[] rest) {
            this.centerX = GameObjects.board1.PosX + GameObjects.board1.SizeX / 2;
            int minY = 0;
            for (int i = 1; i < rest.Length; i += 2) {
                if (minY > rest[i])
                    minY = rest[i];
            }
            this.centerY = minY * (-1) * Block.size + GameObjects.board1.PosY;
            this.rest = rest;
        }
    }

}

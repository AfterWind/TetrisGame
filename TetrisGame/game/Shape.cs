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

        private void Move(int offX, int offY) {
            foreach (Block block in blockList) {
                block.Move(offX, offY);
            }
        }

        public bool MoveCheckX(int offX) {
            foreach (Block block in blockList) {
                int newX = block.X + offX;
                if (newX < board.PosX) {
                    Move(board.PosX - block.X, 0);
                    return false;
                }
                if (newX + Block.size > board.PosX + board.SizeX) {
                    Move(board.PosX + board.SizeX - block.X - Block.size, 0);
                    return false;
                }
                Block blockCheck = null;
                try {
                    blockCheck = board.blockArray[block.GetRelativeX() + Math.Sign(offX) * 1, block.GetRelativeY()];
                } catch (IndexOutOfRangeException) {}
                if (blockCheck != null) {
                    Move(blockCheck.X - block.X - Math.Sign(offX) * Block.size, 0);
                    return false;
                }
            }

            Move(offX, 0);

            return true;
        }

        public bool MoveCheckY(int offY) {
            foreach (Block block in blockList) {
                int newY = block.Y + offY;
                if (newY < board.PosY)
                    return false;
                if (newY + Block.size > board.PosY + board.SizeY) {
                    Move(0, board.PosY + board.SizeY - block.Y - Block.size);
                    return false;
                }
                Block blockCheck = null;
                for (int i = 1; i <= Math.Ceiling((double)offY / (double)Block.size) && blockCheck == null; i++) {
                    try {
                        blockCheck = board.blockArray[block.GetRelativeX(), block.GetRelativeY() + i];
                    } catch (IndexOutOfRangeException) {}
                }

                if (blockCheck != null) {
                    Move(0, blockCheck.Y - block.Y - Block.size);
                    return false;
                }
            }

            Move(0, offY);

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

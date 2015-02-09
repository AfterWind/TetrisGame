using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Shape {

        public List<Block> blockList { private set; get; }
        public Block center { private set; get; }
        public Board board { private set; get; }
        public bool isSymmetrical { private set; get; }

        public Shape(Shape other) : this(other.board, Utils.GetRandomColor(), new Point(other.center.X, other.center.Y), Utils.GetDifferentialParams(other.blockList, other.center)) { }
        public Shape(Board board, params int[] adjacent) : this(board, Utils.GetRandomColor(), Utils.GetDefaultCenter(board, adjacent), adjacent) { }
        public Shape(Board board, Color color, Point centerPoint, params int[] adjacent) {
            this.board = board;
            this.blockList = new List<Block>();
            this.center = new Block(board, color, centerPoint.X, centerPoint.Y);

            blockList.Add(center);
            for (int i = 0; i < adjacent.Length; i += 2) {
                blockList.Add(new Block(board, color, centerPoint.X + adjacent[i] * Block.Size, centerPoint.Y + adjacent[i + 1] * Block.Size));
            }

            this.isSymmetrical = Utils.IsSymmetrical(blockList);
        }

        public IEnumerator<Block> GetBottomBlocks() {
            foreach (Block block in blockList) {
                bool ok = true;
                foreach (Block blockBelow in blockList) {
                    if (blockBelow.X == block.X && blockBelow.Y == block.Y - Block.Size)
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
                if (newX + Block.Size > board.PosX + board.SizeX) {
                    Move(board.PosX + board.SizeX - block.X - Block.Size, 0);
                    return false;
                }
                Block blockCheck = null;
                try {
                    blockCheck = board.BlockArray[block.GetRelativeX() + Math.Sign(offX) * 1, block.GetRelativeY()];
                } catch (IndexOutOfRangeException) { }
                if (blockCheck != null) {
                    Move(blockCheck.X - block.X - Math.Sign(offX) * Block.Size, 0);
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
                if (newY + Block.Size > board.PosY + board.SizeY) {
                    Move(0, board.PosY + board.SizeY - block.Y - Block.Size);
                    return false;
                }
                Block blockCheck = null;
                for (int i = 1; i <= Math.Ceiling((double)offY / (double)Block.Size) && blockCheck == null; i++) {
                    try {
                        blockCheck = board.BlockArray[block.GetRelativeX(), block.GetRelativeY() + i];
                    } catch (IndexOutOfRangeException) { }
                }

                if (blockCheck != null) {
                    Move(0, blockCheck.Y - block.Y - Block.Size);
                    return false;
                }
            }
            Move(0, offY);
            return true;
        }

        

        public void RotateLeft() {

            Point[] rotatedCoordinates = new Point[blockList.Count];
            int i = 0; // Is used outside of the for loop
            for (; i < blockList.Count; i++ ) {

                Point rotatedPoint;
                Point translationCoordinate = new Point(blockList[i].X - center.X, blockList[i].Y - center.Y);

                translationCoordinate.Y *= -1;

                rotatedPoint = new Point(translationCoordinate.X, translationCoordinate.Y);

                rotatedPoint.X = (int)Math.Round(translationCoordinate.X * Math.Cos(Math.PI / 2) - translationCoordinate.Y * Math.Sin(Math.PI / 2));
                rotatedPoint.Y = (int)Math.Round(translationCoordinate.X * Math.Sin(Math.PI / 2) + translationCoordinate.Y * Math.Cos(Math.PI / 2));

                rotatedPoint.Y *= -1;

                rotatedPoint.X += center.X;
                rotatedPoint.Y += center.Y;
                
                if (Utils.IsPositionValid(board, rotatedPoint.X, rotatedPoint.Y)) {
                    rotatedCoordinates[i] = new Point(rotatedPoint.X, rotatedPoint.Y);
                } else {
                    break;
                }   
            }
            // Check if for loop escaped at the proper time
            if (i >= blockList.Count) {
                for (i = 0; i < blockList.Count; i++) {
                    blockList[i].X = rotatedCoordinates[i].X;
                    blockList[i].Y = rotatedCoordinates[i].Y;
                }
            }
        }

        public void Rotate() {
            if (!isSymmetrical) {
                RotateLeft();
            }
            /*
            foreach (Block block in blockList) {
                if (block == center)
                    continue;
                int diffX = block.GetRelativeX() - center.GetRelativeX();
                int diffY = block.GetRelativeY() - center.GetRelativeY();

                if (diffX <= 0 && diffY <= 0) {
                    block.X = center.X - Block.size * diffY;
                    block.Y = center.Y + Block.size * diffX;
                } else if (diffX >= 0 && diffY <= 0) {
                    block.X = center.X - Block.size * diffY;
                    block.Y = center.Y + Block.size * diffX;
                } else if (diffX >= 0 && diffY >= 0) {
                    block.X = center.X + Block.size * diffY;
                    block.Y = center.Y + Block.size * diffX;
                } else {
                    block.X = center.X - Block.size * diffY;
                    block.Y = center.Y + Block.size * diffX;
                }
            }
             * */
        }

        public void Draw(SpriteBatch batch) {
            foreach (Block block in blockList) {
                block.Draw(batch);
            }
        }
    }

}

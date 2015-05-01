﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Shape {

        public List<Block> BlockList { private set; get; }
        public Block Base { private set; get; }
        public Board Board { private set; get; }
        public Point Middle { private set; get; }
        public bool isSymmetrical { private set; get; }
        public int SizeX { private set; get; }
        public int SizeY { private set; get; }

        public Shape(Board board, params int[] adjacent) : this(board, Utils.GetRandomColor(), Utils.GetDefaultCenter(board, adjacent), adjacent) { }
        public Shape(Board board, Color color, Point centerPoint, params int[] adjacent) {
            this.Board = board;
            this.Base = new Block(board, color, centerPoint.X, centerPoint.Y);

            // All these are used for calculating the size of the shape in pixels
            int xMin = 100, xMax = -100, yMin = 100, yMax = -100;

            this.BlockList = new List<Block>();
            BlockList.Add(Base);
            for (int i = 0; i < adjacent.Length; i += 2) {
                BlockList.Add(new Block(board, color, centerPoint.X + adjacent[i] * Block.Size, centerPoint.Y + adjacent[i + 1] * Block.Size));
                if (adjacent[i] < xMin)
                    xMin = adjacent[i];
                if (adjacent[i] > xMax)
                    xMax = adjacent[i];
                if (adjacent[i + 1] < yMin)
                    yMin = adjacent[i + 1];
                if (adjacent[i + 1] > yMax)
                    yMax = adjacent[i + 1];
            }

            SizeX = (xMax - xMin + 1) * Block.Size;
            SizeY = (yMax - yMin + 1) * Block.Size;

            Middle = new Point(centerPoint.X - (xMin * Block.Size) + SizeX / 2, centerPoint.Y - (yMin * Block.Size) + SizeY / 2);
            
            this.isSymmetrical = Utils.IsSymmetrical(BlockList);
        }

        public IEnumerator<Block> GetBottomBlocks() {
            foreach (Block block in BlockList) {
                bool ok = true;
                foreach (Block blockBelow in BlockList) {
                    if (blockBelow.X == block.X && blockBelow.Y == block.Y - Block.Size)
                        ok = false;
                }
                if (ok)
                    yield return block;
            }
        }

        public void Move(int offX, int offY) {
            Middle = new Point(Middle.X + offX, Middle.Y + offY);
            
            foreach (Block block in BlockList) {
                block.Move(offX, offY);
            }
        }

        public void MoveTo(Point p) {
            MoveTo(p.X, p.Y);
        }

        public void MoveTo(int x, int y) {
            int centerX = Base.X;
            int centerY = Base.Y;
            foreach (Block block in BlockList) {
                block.X = x + (block.X - centerX);
                block.Y = y + (block.Y - centerY);
            }
            Middle = new Point(x, y);
        }

        public bool MoveCheckX(int offX) {
            foreach (Block block in BlockList) {
                int newX = block.X + offX;
                if (newX < Board.PosX) {
                    Move(Board.PosX - block.X, 0);
                    return false;
                }
                if (newX + Block.Size > Board.PosX + Board.SizeX) {
                    Move(Board.PosX + Board.SizeX - block.X - Block.Size, 0);
                    return false;
                }
                Block blockCheck = null;
                try {
                    blockCheck = Board.BlockArray[block.GetRelativeX() + Math.Sign(offX) * 1, block.GetRelativeY()];
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
            foreach (Block block in BlockList) {
                int newY = block.Y + offY;
                if (newY < Board.PosY)
                    return false;
                if (newY + Block.Size > Board.PosY + Board.SizeY) {
                    Move(0, Board.PosY + Board.SizeY - block.Y - Block.Size);
                    return false;
                }
                Block blockCheck = null;
                for (int i = 1; i <= Math.Ceiling((double)offY / (double)Block.Size) && blockCheck == null; i++) {
                    try {
                        blockCheck = Board.BlockArray[block.GetRelativeX(), block.GetRelativeY() + i];
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

            Point[] rotatedCoordinates = new Point[BlockList.Count];
            int i = 0; // Is used outside of the for loop
            for (; i < BlockList.Count; i++ ) {

                Point rotatedPoint;
                Point translationCoordinate = new Point(BlockList[i].X - Base.X, BlockList[i].Y - Base.Y);

                translationCoordinate.Y *= -1;

                rotatedPoint = new Point(translationCoordinate.X, translationCoordinate.Y);

                rotatedPoint.X = (int)Math.Round(translationCoordinate.X * Math.Cos(Math.PI / 2) - translationCoordinate.Y * Math.Sin(Math.PI / 2));
                rotatedPoint.Y = (int)Math.Round(translationCoordinate.X * Math.Sin(Math.PI / 2) + translationCoordinate.Y * Math.Cos(Math.PI / 2));

                rotatedPoint.Y *= -1;

                rotatedPoint.X += Base.X;
                rotatedPoint.Y += Base.Y;
                
                if (Utils.IsPositionValid(Board, rotatedPoint.X, rotatedPoint.Y)) {
                    rotatedCoordinates[i] = new Point(rotatedPoint.X, rotatedPoint.Y);
                } else {
                    break;
                }   
            }
            // Check if for loop escaped at the proper time
            if (i >= BlockList.Count) {
                for (i = 0; i < BlockList.Count; i++) {
                    BlockList[i].X = rotatedCoordinates[i].X;
                    BlockList[i].Y = rotatedCoordinates[i].Y;
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
            foreach (Block block in BlockList) {
                block.Draw(batch);
            }
        }
    }

}

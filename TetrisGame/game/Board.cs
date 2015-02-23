using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Board {

        public int SizeX { private set; get; }
        public int SizeY { private set; get; }
        public int PosX { private set; get; }
        public int PosY { private set; get; }
        public int Speed { private set; get; }
        public Block[,] BlockArray { private set; get; }
        public bool HasLost { private set; get; }

        private static readonly int BORDER_SIZE = 5;
        private static readonly Color BORDER_COLOR = Color.Black;

        private static readonly int DIFFX_NEXT_SHAPE = 150;
        private static readonly int DIFFY_NEXT_SHAPE = 40;
        private static readonly int BOX_SIZE = 150;

        private Shape movingShape;
        private Shape nextShape;

        //TODO: Get rid of most of these variables for a better way to remove rows
        private int[] RemovingRows;
        private int[] RemovingRowProgress;
        private bool CurrentlyRemovingRows = false;

        public Board(int posX, int posY, int columns, int rows) {
            BlockArray = new Block[columns, rows];
            this.SizeX = columns * Block.Size;
            this.SizeY = rows * Block.Size;
            this.PosX = posX;
            this.PosY = posY;
            this.Speed = 3;
            this.RemovingRows = new int[rows];
            this.RemovingRowProgress = new int[rows];
        }

        public void AddShape(Shape shape) {
            shape.MoveTo(PosX + SizeX + DIFFX_NEXT_SHAPE, shape.center.Y);
            if(nextShape != null)
                nextShape.MoveTo(Utils.GetDefaultCenter(this, nextShape.blockList, nextShape.center));
            this.movingShape = nextShape;
            this.nextShape = shape;
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device) {
            // Draw the borders
            /*
             * TODO: Experiment with this more, so that it works more efficiently
            short[] indices = new short[4];
            VertexPositionColor[] points = new VertexPositionColor[4];

            points[0] = new VertexPositionColor(new Vector3(PosX - BORDER_SIZE / 2, PosY - BORDER_SIZE / 2, 0), BORDER_COLOR);
            points[1] = new VertexPositionColor(new Vector3(PosX + SizeX + BORDER_SIZE / 2, PosY - BORDER_SIZE / 2, 0), BORDER_COLOR);
            points[2] = new VertexPositionColor(new Vector3(PosX + SizeX + BORDER_SIZE / 2, PosY + SizeY + BORDER_SIZE / 2, 0), BORDER_COLOR);
            points[3] = new VertexPositionColor(new Vector3(PosX - BORDER_SIZE / 2, PosY + SizeY + BORDER_SIZE / 2, 0), BORDER_COLOR);

            for (short i = 0; i < 4; i++) {
                indices[i] = i;
            }
            
            device.DrawUserIndexedPrimitives<VertexPositionColor> (PrimitiveType.LineList, points, 0, 4, indices, 0, 3);
            */


            // Draw the borders less efficient

            GraphicUtils.DrawRectangle(batch, Color.Black, PosX, PosY, SizeX, SizeY, BORDER_SIZE);
            GraphicUtils.DrawTransparentRectangle(batch, Color.FromNonPremultiplied(155, 180, 225, 75), PosX, PosY, SizeX, SizeY);
            
            
            //GraphicUtils.DrawRectangle(batch, , PosX, PosY, )
            
            GraphicUtils.DrawRectangle(batch, Color.Black, PosX + SizeX + DIFFX_NEXT_SHAPE - BOX_SIZE / 2 + Block.Size / 2, PosY, BOX_SIZE, BOX_SIZE, BORDER_SIZE);
            GraphicUtils.DrawTransparentRectangle(batch, Color.FromNonPremultiplied(155, 180, 225, 75), PosX + SizeX + DIFFX_NEXT_SHAPE - BOX_SIZE / 2 + Block.Size / 2, PosY, BOX_SIZE, BOX_SIZE);

            batch.Begin();
            // Draw all the blocks
            foreach (Block block in BlockArray) {
                if (block != null) {
                    block.Draw(batch);
                }
            }
            // Draw the moving shape
            if (movingShape != null)
                movingShape.Draw(batch);
            
            // Draw the next shape
            if (nextShape != null)
                nextShape.Draw(batch);

            batch.End();    
        }

        public void Update() {

            // Universally used variables
            int i, j, k;

            // Move the shape that is controlled by the player
            if (movingShape != null) {
                if (!CurrentlyRemovingRows) {
                    if (movingShape.MoveCheckY(Speed)) {

                    } else {
                        foreach (Block block in movingShape.blockList) {
                            BlockArray[block.GetRelativeX(), block.GetRelativeY()] = block;
                            block.X = block.GetRelativeX() * Block.Size + PosX;
                            block.Y = block.GetRelativeY() * Block.Size + PosY;
                        }
                        movingShape = null;
                    }
                }
            } else {
                AddShape(Utils.GetRandomShape());
            }

            // Verify if there are blocks in the first row, if so player loses
            for (i = 0; i < BlockArray.GetLength(0); i++) {
                if (BlockArray[i, 0] != null)
                    HasLost = true;
            }

            // Verify if a row has been filled
            for (j = 0; j < BlockArray.Length / BlockArray.GetLength(0); j++) {
                for (i = 0; i < BlockArray.GetLength(0); i++) {
                    if (BlockArray[i, j] == null)
                        break;
                }
                if (i >= BlockArray.GetLength(0)) {
                    RemovingRows[j] = 1;
                    CurrentlyRemovingRows = true;
                }
            }

            for (k = 0; k < RemovingRows.Length; k++ ) {
                if (RemovingRows[k] != 0) {
                    if (RemovingRowProgress[k] >= BlockArray.GetLength(0)) {
                        for (j = k - 1; j >= 0; j--) {
                            for (i = 0; i < BlockArray.GetLength(0); i++) {
                                if (BlockArray[i, j] != null) {
                                    BlockArray[i, j].Move(0, Block.Size);
                                    BlockArray[i, j + 1] = BlockArray[i, j];
                                    BlockArray[i, j] = null;
                                }
                            }
                        }

                        RemovingRows[k] = 0;
                        RemovingRowProgress[k] = 0;
                    } else {
                        BlockArray[RemovingRowProgress[k]++, k] = null;
                    }
                }
            }

            for (k = 0; k < RemovingRows.Length; k++)
                if (RemovingRows[k] != 0)
                    break;
            if (k >= RemovingRows.Length)
                CurrentlyRemovingRows = false;
        }

        public void MoveShape(int offsetX) {
            if (movingShape != null) {
                movingShape.MoveCheckX(offsetX);
            }
        }

        public void RotateShape() {
            if (movingShape != null)
                movingShape.Rotate();
        }

        public void RemoveRow(int row) {
            for (int j = row; j >= 0; j--) {
                for (int i = 0; i < BlockArray.GetLength(0); i++) {
                    if (row == j) {
                        // TODO: Implement a block deleting method
                        BlockArray[i, j] = null;

                    } else if (BlockArray[i, j] != null) {
                        
                        // TODO: Better block movement
                        BlockArray[i, j].Move(0, Block.Size);
                        BlockArray[i, j + 1] = BlockArray[i, j];
                        BlockArray[i, j] = null;
                    }
                }
            }
        }

        
        
        public void ResetMap() {
            BlockArray = new Block[SizeX / Block.Size, SizeY / Block.Size];
            HasLost = false;
        }

        public void SetSpeed(int newSpeed) {
            this.Speed = newSpeed;
        }

        public void PrintMap() {
            Console.Out.WriteLine("Printing Map: ");
            for (int j = 0; j < BlockArray.Length / BlockArray.GetLength(0); j++) {
                for (int i = 0; i < BlockArray.GetLength(0); i++) {
                    if (BlockArray[i, j] != null)
                        Console.Out.Write("1 ");
                    else
                        Console.Out.Write("0 ");
                }
                Console.Out.Write("\n");
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Board {

        public int SizeX { private set; get; }
        public int SizeY { private set; get; }
        public int PosX { private set; get; }
        public int PosY { private set; get; }
        public int BoxX { private set; get; }
        public int BoxY { private set; get; }

        public int RowsCleared { private set; get; }
        
        public int SpeedIncrease { private set; get; }
        public int SpeedIncreaseDebug { private set; get; }

        public Block[,] BlockArray { private set; get; }
        public int Rows { private set; get; }
        public int Columns { private set; get; }
        public bool HasLost { private set; get; }

        public int Speed {
            get {
                return Paused ? 0 : INITIAL_SPEED + (GameObjects.Level / 3) + SpeedIncrease + SpeedIncreaseDebug;
            }
        }
        public bool Paused { set; get; }

        private static readonly int BORDER_SIZE = 5;
        private static readonly Color BORDER_COLOR = Color.Black;

        private static readonly Color GRID_COLOR = Color.DarkBlue;

        private static readonly int DIFFY_NEXT_SHAPE = 30;
        private static readonly int BOX_SIZE = 150;

        private static readonly int INITIAL_SPEED = 3;

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
            this.RemovingRows = new int[rows];
            this.RemovingRowProgress = new int[rows];
            this.Rows = rows;
            this.Columns = columns;
            this.BoxX = posX + SizeX / 2 - BOX_SIZE / 2;
            this.BoxY = posY + SizeY + DIFFY_NEXT_SHAPE + 2 * BORDER_SIZE;
        }

        public void AddShape(Shape shape) {
            this.SpeedIncrease = 0;
            bool isEvenX = shape.SizeX / Block.Size % 2 == 0, isEvenY = shape.SizeY / Block.Size % 2 == 0; 
            shape.MoveTo((BoxX + BOX_SIZE / 2) - (isEvenX ? 0 : Block.Size / 2), (BoxY + BOX_SIZE / 2) - (isEvenY ? 0 : Block.Size / 2));
            if(nextShape != null)
                nextShape.MoveTo(Utils.GetDefaultCenter(this, nextShape.BlockList, nextShape.Base));
            this.movingShape = nextShape;
            this.nextShape = shape;
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device) {

            GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, PosX - BORDER_SIZE, 0, SizeX + 2 * BORDER_SIZE, GraphicUtils.screenHeight);
            
            GraphicUtils.DrawBorder(batch, GameObjects.IsBoardSelected(this) ? Color.White : Color.Black, PosX, PosY, SizeX, SizeY, BORDER_SIZE);
            GraphicUtils.DrawRectangle(batch, Color.FromNonPremultiplied(155, 180, 225, 75), PosX, PosY, SizeX, SizeY);

            GraphicUtils.DrawBorder(batch, GameObjects.IsBoardSelected(this) ? Color.White : Color.Black, PosX + SizeX / 2 - BOX_SIZE / 2, PosY + SizeY + 2 * BORDER_SIZE + DIFFY_NEXT_SHAPE, BOX_SIZE, BOX_SIZE, BORDER_SIZE);
            GraphicUtils.DrawRectangle(batch, Color.FromNonPremultiplied(155, 180, 225, 75), PosX + SizeX/2 - BOX_SIZE / 2, PosY + SizeY + 2*BORDER_SIZE + DIFFY_NEXT_SHAPE, BOX_SIZE, BOX_SIZE);

            // Draw the grid
            if (Config.isGridEnabled) {
                batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                Texture2D pixel = new Texture2D(GameObjects.GraphicsDevice, 1, 1);
                pixel.SetData(new Color[] { GRID_COLOR });
                for (int i = 0; i <= Columns; i++) {
                    if (i == 0) {
                        batch.Draw(pixel, new Rectangle(PosX, PosY, 1, SizeY), Color.White);
                    } else if (i == Columns) {
                        batch.Draw(pixel, new Rectangle(PosX + SizeX - 1, PosY, 1, SizeY), Color.White);
                    } else {
                        batch.Draw(pixel, new Rectangle(PosX + i * Block.Size, PosY, 1, SizeY), Color.White);
                        batch.Draw(pixel, new Rectangle(PosX + i * Block.Size - 1, PosY, 1, SizeY), Color.White);
                    }
                }


                for (int i = 0; i <= Rows; i++) {
                    if (i == 0) {
                        batch.Draw(pixel, new Rectangle(PosX, PosY, SizeX, 1), Color.White);
                    } else if (i == Rows) {
                        batch.Draw(pixel, new Rectangle(PosX, PosY + SizeY - 1, SizeX, 1), Color.White);
                    } else {
                        batch.Draw(pixel, new Rectangle(PosX, PosY + i * Block.Size, SizeX, 1), Color.White);
                        batch.Draw(pixel, new Rectangle(PosX, PosY + i * Block.Size - 1, SizeX, 1), Color.White);
                    }
                }
                batch.End();
            }

            // Draw all the blocks
            batch.Begin();
            foreach (Block block in BlockArray) {
                if (block != null) {
                    block.Draw(batch);
                }
            }
            if (movingShape != null)
                movingShape.Draw(batch);
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
                        foreach (Block block in movingShape.BlockList) {
                            //Console.WriteLine("Finding block at: " + block.GetRelativeX() + ", " + block.GetRelativeY());
                            BlockArray[block.GetRelativeX(), block.GetRelativeY()] = block;
                            block.X = block.GetRelativeX() * Block.Size + PosX;
                            block.Y = block.GetRelativeY() * Block.Size + PosY;
                        }
                        
                        movingShape = null;
                    }
                }
            } else {
                Shape randomShape = Utils.GetRandomShape(this);
                AddShape(randomShape);
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
            GameObjects.IncreaseRowsCleared();
        }
        
        public void ResetMap() {
            BlockArray = new Block[SizeX / Block.Size, SizeY / Block.Size];
            HasLost = false;
            Console.WriteLine("Resetting map.");
        }

        public void IncreaseSpeed(int speed) {
            this.SpeedIncrease += speed;
        }

        public void DebugIncreaseSpeed(int speed) {
            this.SpeedIncreaseDebug += speed;
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

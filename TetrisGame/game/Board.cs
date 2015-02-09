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

        private Shape movingShape;

        public Board(int posX, int posY, int columns, int rows) {
            BlockArray = new Block[columns, rows];
            this.SizeX = columns * Block.Size;
            this.SizeY = rows * Block.Size;
            this.PosX = posX;
            this.PosY = posY;
            this.Speed = 3;
        }

        public void AddShape(Shape shape) {
            this.movingShape = shape;
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
            Texture2D Pixel = new Texture2D(device, 1, 1);
            Pixel.SetData(new Color[] { BORDER_COLOR });
            batch.Begin();
            batch.Draw(Pixel, new Rectangle(PosX - BORDER_SIZE, PosY - BORDER_SIZE, BORDER_SIZE, SizeY + BORDER_SIZE), Color.White);
            batch.Draw(Pixel, new Rectangle(PosX - BORDER_SIZE, PosY + SizeY, SizeX + BORDER_SIZE, BORDER_SIZE), Color.White);
            batch.Draw(Pixel, new Rectangle(PosX, PosY - BORDER_SIZE, SizeX + BORDER_SIZE, BORDER_SIZE), Color.White);
            batch.Draw(Pixel, new Rectangle(PosX + SizeX, PosY, BORDER_SIZE, SizeY + BORDER_SIZE), Color.White);
            batch.End();
            

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
            batch.End();    
        }

        public void Update() {

            // Universally used variables
            int i, j;

            // Move the shape that is controlled by the player
            if (movingShape != null) {
                if (movingShape.MoveCheckY(Speed)) {

                } else {
                    foreach (Block block in movingShape.blockList) {
                        BlockArray[block.GetRelativeX(), block.GetRelativeY()] = block;
                        block.X = block.GetRelativeX() * Block.Size + PosX;
                        block.Y = block.GetRelativeY() * Block.Size + PosY;
                    }
                    movingShape = null;
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
                    RemoveRow(j);
                }
            }
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

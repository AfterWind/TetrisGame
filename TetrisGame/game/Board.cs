using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Board {
        
        private static int BORDER_SIZE = 5;
        private static Color BORDER_COLOR = Color.Black;

        public Block[,] blockArray;
        private Shape movingShape;

        public bool hasLost = false;

        public int SizeX { private set; get; }
        public int SizeY { private set; get; }
        public int PosX { private set; get; }
        public int PosY { private set; get; }
        public int Speed { private set; get; }

        public Board(int posX, int posY, int columns, int rows) {
            blockArray = new Block[columns, rows];
            this.SizeX = columns * Block.size;
            this.SizeY = rows * Block.size;
            this.PosX = posX;
            this.PosY = posY;
            this.Speed = 3;
        }

        public void AddShape(Shape shape) {
            this.movingShape = shape;
        }

        public void PrintMap() {
            Console.Out.WriteLine("Printing Map: ");
            for (int j = 0; j < blockArray.Length / blockArray.GetLength(0); j++) {
                for (int i = 0; i < blockArray.GetLength(0); i++) {
                    if (blockArray[i, j] != null)
                        Console.Out.Write("1 ");
                    else
                        Console.Out.Write("0 ");
                }
                Console.Out.Write("\n");
            }
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
            foreach (Block block in blockArray) {
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
            if (movingShape != null) {
                if (movingShape.MoveCheckY(Speed)) {

                } else {
                    foreach (Block block in movingShape.blockList) {
                        blockArray[block.GetRelativeX(), block.GetRelativeY()] = block;
                        block.X = block.GetRelativeX() * Block.size + PosX;
                        block.Y = block.GetRelativeY() * Block.size + PosY;
                    }
                    movingShape = null;
                }
            } else {
                AddShape(Utils.GetRandomShape());
            }
            for (int i = 0; i < blockArray.GetLength(0); i++) {
                if (blockArray[i, 0] != null)
                    hasLost = true;
            }
        }

        public void MoveShape(int offsetX) {
            if (movingShape != null) {
                movingShape.MoveCheckX(offsetX);
            }
        }

        public void ResetMap() {
            blockArray = new Block[SizeX / Block.size, SizeY / Block.size];
        }

        public void SetSpeed(int newSpeed) {
            this.Speed = newSpeed;
        }

        public void RotateShape() {
            if (movingShape != null)
                movingShape.Rotate();
        }
    }
}

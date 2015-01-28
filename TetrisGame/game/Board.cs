using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Board {

        public Block[,] blockArray;
        private Shape movingShape;

        public bool hasLost = false;

        public int SizeX { private set; get; }
        public int SizeY { private set; get; }
        public int PosX { private set; get; }
        public int PosY { private set; get; }

        public Board(int posX, int posY, int columns, int rows) {
            blockArray = new Block[columns, rows];
            this.SizeX = columns * Block.size;
            this.SizeY = rows * Block.size;
            this.PosX = posX;
            this.PosY = posY;
        }

        public void AddShape(Shape shape) {
            this.movingShape = shape;
        }

        public void Update() {
            if (movingShape != null) {
                if (movingShape.MoveY(3)) {

                } else {
                    foreach (Block block in movingShape.blockList) {
                        blockArray[block.GetRelativeX(), block.GetRelativeY()] = block;
                    }
                    movingShape = null;
                }
            } else {
                AddShape(GameObjects.GetRandomShape());
            }
            for (int i = 0; i < blockArray.GetLength(0); i++) {
                if (blockArray[i, 0] != null)
                    hasLost = true;
            }
        }

        public void PrintMap() {
            Console.Out.WriteLine("Printing Map: ");
            for (int j = 0; j < blockArray.Length / blockArray.GetLength(0); j++) {
                for (int i = 0; i < blockArray.GetLength(0); i++) {
                    try {
                        if (blockArray[i, j] != null)
                            Console.Out.Write("1 ");
                        else
                            Console.Out.Write("0 ");
                    } catch {
                        Console.Out.WriteLine("Error occured on reading: " + i + ", " + j);
                    }
                }
                Console.Out.Write("\n");
            }
        }

        public void Draw(SpriteBatch batch) {
            foreach (Block block in blockArray) {
                if (block != null) {
                    block.Draw(batch);
                }
            }
            if (movingShape != null)
                movingShape.Draw(batch);
        }

        public void MoveShape(int offsetX) {
            if (movingShape != null) {
                movingShape.MoveX(offsetX);
            }
        }
    }
}

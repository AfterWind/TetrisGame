using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame {
    class GameObjects {

        public static Color fromColor = Color.FromNonPremultiplied(96, 96, 96, 255);
        public static Texture2D baseTexture;
        public static Board board1=new Board(0, 0, 16, 20);
        public static ContentManager content;
        public static void Init(ContentManager content) {
            GameObjects.content = content;
            GameObjects.board1.AddShape(GetRandomShape());
        }

        public static Texture2D PrepareBlockTexture(Color color) {
            Texture2D texture = content.Load<Texture2D>("Block");
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++) {
                if (pixels[i] == fromColor) {
                    pixels[i] = color;
                }
            }
            Texture2D tex = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            tex.SetData(pixels);
            return tex;
        }

        public static void Update(SpriteBatch batch) {

        }

        public static Color[] colors = new Color[] {
            Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.Black
        };

        public static Pattern[] patterns = new Pattern[] {
            new Pattern(1, 0, -1, 0, 0, 1, -1, -1, 1, -1),
            new Pattern(0, 1, 0, 2, 0, 3),
            new Pattern(1, 0, -1, 0, 0, 1, 0, -1),
            new Pattern(1, 0, -1, 0, 0, -1),
            new Pattern(1, 0, 0, -1, 1, -1),
        };

        public static Random random = new Random();
        public static Color GetRandomColor() {
            return colors[random.Next(0, colors.Length)];
        }

        public static Shape GetRandomShape() {
            Random random = new Random();
            Pattern pattern = patterns[random.Next(0, patterns.Length)];
            Color color = GetRandomColor();
            return new Shape(GameObjects.board1, GetRandomColor(), pattern.centerX, pattern.centerY, pattern.rest);
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

    public class Block {

        public static int size = 20;

        public Color color { private set; get; }
        public Board board;

        public int X { private set; get; }
        public int Y { private set; get; }

        public int GetRelativeX() {
            return (X - board.PosX) / Block.size;
        }

        public int GetRelativeY() {
            return (Y - board.PosY) / Block.size;
        }

        public Block(Board board, Color color, int x, int y) {
            this.X = x;
            this.Y = y;
            this.color = color;
            this.board = board;
        }

        public void Move(int offX, int offY) {
            this.X += offX;
            this.Y += offY;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(GameObjects.PrepareBlockTexture(color), new Rectangle(X, Y, size, size), Color.White);
        }
    }

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
                } catch(IndexOutOfRangeException ex) {

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

    public class Board {

        public Block[,] blockArray;
        private Shape movingShape;

        public bool hasLost = false;

        public int SizeX {private set; get;}
        public int SizeY {private set; get;}
        public int PosX {private set; get; }
        public int PosY {private set; get;}

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
            for (int i = 0; i < blockArray.GetLength(0); i++ ) {
                if (blockArray[i, 0] != null)
                    hasLost = true;
            }
        }

        public void PrintMap() {
            Console.Out.WriteLine("Printing Map: ");
            for (int j = 0; j < blockArray.Length / blockArray.GetLength(0); j++) {
                for (int i = 0; i < blockArray.GetLength(0); i++ ) {
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

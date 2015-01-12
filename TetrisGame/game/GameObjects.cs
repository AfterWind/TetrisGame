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
        public static Board board1=new Board(0, 0, 8, 20);
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
            new Pattern(board1.PosX + board1.SizeX / 2, board1.PosY + 10, 1, 0, -1, 0, 0, 1, -1, -1, 1, -1)
        };

        public static Random random = new Random();
        public static Color GetRandomColor() {
            return colors[random.Next(0, colors.Length)];
        }

        public static Shape GetRandomShape() {
            Random random = new Random();
            Pattern pattern = patterns[random.Next(0, patterns.Length)];
            Color color = GetRandomColor();
            Console.Out.WriteLine("Got color: " + color.ToString());
            return new Shape(GetRandomColor(), pattern.centerX, pattern.centerY, pattern.rest);
        }
    }

    public class Pattern {
        public int centerX, centerY;
        public int[] rest;

        public Pattern(int centerX, int centerY, params int[] rest) {
            this.centerX = centerX;
            this.centerY = centerY;
            this.rest = rest;
        }
    }

    public class Block {

        public static int size = 20;

        public Color color { private set; get; }
        public int X { private set; get; }
        public int Y { private set; get; }

        public Block(Color color, int x, int y) {
            this.X = x;
            this.Y = y;
            this.color = color;
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

        public Shape(Color color, int centerX, int centerY, params int[] adjacent) {

            blockList = new List<Block>();
            center = new Block(color, centerX, centerY);
            blockList.Add(center);
            for (int i = 0; i < adjacent.Length; i += 2) {
                blockList.Add(new Block(color, centerX + adjacent[i] * Block.size, centerY + adjacent[i + 1] * Block.size));
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

        public void Move(int offX, int offY) {
            foreach (Block block in blockList) {
                block.Move(offX, offY);
            }
        }

        public void Draw(SpriteBatch batch) {
            foreach (Block block in blockList) {
                block.Draw(batch);
            }
        }
    }

    public class Board {

        private List<Block> blocks;
        private Shape movingShape;
        
        public int SizeX {private set; get;}
        public int SizeY {private set; get;}
        public int PosX {private set; get; }
        public int PosY {private set; get;}

        public Board(int posX, int posY, int columns, int rows) {
            this.SizeX = columns * Block.size;
            this.SizeY = rows * Block.size;
            this.PosX = posX;
            this.PosY = posY;
            blocks = new List<Block>();
        }

        public void AddShape(Shape shape) {
            this.movingShape = shape;
        }

        public void Update() {
            if (movingShape != null) {
                if (!IsShapeObstructed(movingShape)) {
                    movingShape.Move(0, 5);
                } else {
                    foreach (Block block in movingShape.blockList)
                        blocks.Add(block);
                    movingShape = null;
                }
            } else {
                AddShape(GameObjects.GetRandomShape());
            }
        }

        public void Draw(SpriteBatch batch) {
            foreach (Block block in blocks) {
                block.Draw(batch);
            }
            if (movingShape != null)
                movingShape.Draw(batch);
        }

        public bool IsBlockObstructed(Block b) {
            if (b.Y + Block.size + 1 > PosY + SizeY)
                return true;
            foreach (Block block in blocks) {
                if (b.X >= block.X && b.X <= block.X + Block.size && b.Y + Block.size + 1 >= block.Y && b.Y + Block.size + 1 <= block.Y + Block.size) {
                    return true;
                }
            }
            return false;
        }

        public bool IsShapeObstructed(Shape shape) {
            IEnumerator<Block> bottom = shape.GetBottomBlocks();
            while (bottom.MoveNext()) {
                if (IsBlockObstructed(bottom.Current))
                    return true;
            }
            return false;
        }


    }
}

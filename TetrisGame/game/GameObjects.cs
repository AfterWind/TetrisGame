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
        public static void Init(ContentManager content) {
            baseTexture = content.Load<Texture2D>("Block");
        }

        public static Texture2D PrepareBlockTexture(Texture2D texture, Color color) {
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++) {
                if (pixels[i] == fromColor) {
                    pixels[i] = color;
                }
            }
            texture.SetData(pixels);
            return texture;
        }

        public static void Update(SpriteBatch batch) {

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
            batch.Draw(GameObjects.PrepareBlockTexture(GameObjects.baseTexture, color), new Rectangle(X, Y, size, size), Color.White);
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
        private int posX, posY;
        private int sizeX, sizeY;

        public Board(int posX, int posY, int columns, int rows) {
            this.sizeX = columns * Block.size;
            this.sizeY = rows * Block.size;
            this.posX = posX;
            this.posY = posY;
            blocks = new List<Block>();
        }

        public void AddShape(Shape shape) {
            this.movingShape = shape;
        }

        public void Update() {
            if (movingShape != null) {
                if (!IsShapeObstructed(movingShape)) {
                    movingShape.Move(0, 1);
                } else {
                    foreach (Block block in movingShape.blockList)
                        blocks.Add(block);
                    movingShape = null;
                }
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
            if (b.Y + Block.size + 1 > posY + sizeY)
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

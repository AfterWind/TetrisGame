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
            for (int i = 0; i < pixels.Length; i++ ) {
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

        public int X { private set; get; }
        public int Y { private set; get; }

        public Block() {
        }

        public Block(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public void Move(int offX, int offY) {
            this.X += offX;
            this.Y += offY;
        }
    }

    public class Shape {
        public List<Block> blockList;
        public Color color;

        public Shape(Color color, params Block[] blocks) {
            
            blockList = new List<Block>();

            foreach(Block block in blocks) {
                blockList.Add(block);
            }
            this.color = color;
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
            foreach(Block block in blockList) {
                block.Move(offX, offY);
            }
        }

        public void Draw(SpriteBatch batch) {
            Texture2D tex;
            foreach (Block block in blockList) {
                batch.Draw(GameObjects.PrepareBlockTexture(GameObjects.baseTexture, color), new Rectangle(block.X, block.Y, Block.size, Block.size), Color.White);
            }
        }
    }

    public class Board {

        private List<Shape> shapes;
        private Shape movingShape;
        private int posX, posY;
        private int sizeX, sizeY;

        public Board(int posX, int posY, int rows, int columns) {
            this.sizeX = columns * Block.size;
            this.sizeY = rows* Block.size;
            this.posX = posX;
            this.posY = posY;
            shapes = new List<Shape>();
        }

        public void AddShape(Shape shape) {
            this.movingShape = shape;
        }

        public void Update() {
            if (movingShape != null) {
                if (!IsShapeObstructed(movingShape)) {
                    movingShape.Move(0, 1);
                } else {
                    shapes.Add(movingShape);
                    movingShape = null;
                }
            }
        }

        public void Draw(SpriteBatch batch) {
            foreach (Shape shape in shapes) {
                shape.Draw(batch);
            }
            if(movingShape != null)
                movingShape.Draw(batch);
        }

        public bool IsBlockObstructed(Block b) {
            if (b.Y + Block.size + 1 > posY + sizeY)
                return true;
            foreach (Shape shape in shapes) {
                foreach (Block block in shape.blockList) {
                    if(b.X >= block.X && b.X <= block.X + Block.size && b.Y + Block.size + 1 >= block.Y && b.Y + Block.size + 1 <= block.Y + Block.size) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsShapeObstructed(Shape shape) {
            IEnumerator<Block> bottom = shape.GetBottomBlocks();
            while(bottom.MoveNext()) {
                if(IsBlockObstructed(bottom.Current))
                    return true;
            }
            return false;
        }


    }
}

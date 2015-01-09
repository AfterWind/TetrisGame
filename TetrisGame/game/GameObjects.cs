using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    class GameObjects {

        public static Dictionary<Color, Texture2D> textures;
        public static Color fromColor = Color.FromArgb(255, 96, 96, 96);

        public static void init(ContentManager content) {
            textures = new Dictionary<Color, Texture2D>();

            Texture2D tex = content.Load<Texture2D>("Block");

            textures.Add(Color.Aqua, prepareBlockTexture(tex, Color.Aqua));
            

        }

        public static Texture2D prepareBlockTexture(Texture2D texture, Color color) {
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++ ) {
                if(pixels[i] == something)
            }
        }
    }

    class Block {

        public static int size = 20;

        private Color color;

        public int X { set; get; }
        public int Y { set; get; }

        public Block() {
        }

        public Block(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public void move(int offX, int offY) {
            this.X += offX;
            this.Y += offY;
        }
    }

    public class Board {

        private List<Block> blocks;
        private Block movingBlock;
        int sizeX, sizeY;

        public Board(int sizeX, int sizeY) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            blocks = new List<Block>();
        }

        public void addBlock(Block block) {
            this.movingBlock = block;
        }

        public void update() {
            if (movingBlock != null) {
                if (getBlock(movingBlock.X, movingBlock.Y + Block.size + 1) == null) {
                    movingBlock.move(0, 1);
                } else {
                    blocks.Add(movingBlock);
                    movingBlock = null;
                }
            }
        }

        public void draw(SpriteBatch batch) {
            
        }

        public Block getBlock(int x, int y) {
            foreach (Block block in blocks) {
                if (x > block.X && x < block.X + Block.size && y > block.Y && y < block.Y + Block.size)
                    return block;
            }
            return null;
        }


    }
}

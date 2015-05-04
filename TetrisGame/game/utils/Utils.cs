using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Utils {
        private static Random random = new Random();
        private static List<int[]> patterns;
        private static Color[] colors;
        private static Dictionary<Color, Texture2D> textures;

        public static int[] GetDifferentialParams(List<Block> blocks, Block center) {
            int[] array = new int[(blocks.Count - 1) * 2];
            int i = 0;
            foreach (Block block in blocks) {
                if (block != center) {
                    array[i++] = (block.X - center.X) / Block.Size;
                    array[i++] = (block.Y - center.Y) / Block.Size;
                }
            }
            return array;
        }

        public static Point GetDefaultCenter(Board board, params int[] adjacent) {
            int minY = 0;
            for (int i = 1; i < adjacent.Length; i += 2) {
                if (minY > adjacent[i])
                    minY = adjacent[i];
            }
            return new Point(board.PosX + board.SizeX / 2, minY * (-1) * Block.Size + board.PosY);
        }

        public static Point GetDefaultCenter(Board board, List<Block> blockList, Block center) {
            int minY = 0;
            foreach (Block b in blockList) {
                if (minY > (b.Y - center.Y))
                    minY = (b.Y - center.Y);
            }
            return new Point(board.PosX + board.SizeX / 2, minY * (-1) + board.PosY);
        }

        public static Texture2D PrepareBlockTexture(Color color) {
            Texture2D texture = GameObjects.Content.Load<Texture2D>("Block");
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++) {
                if (pixels[i] == GameObjects.FromColor) {
                    pixels[i] = color;
                }
            }
            Texture2D tex = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            tex.SetData(pixels);
            return tex;
        }

        public static bool IsPositionValid(Board board, int x, int y) {
            if (x < board.PosX)
                return false;
            if (x + Block.Size > board.PosX + board.SizeX)
                return false;
            if (y < board.PosY)
                return false;
            if (y + Block.Size > board.PosY + board.SizeY)
                return false;
            Block blockCheck = null;
            try {
                blockCheck = board.BlockArray[(x - board.PosX) / Block.Size, (y - board.PosY) / Block.Size];
            } catch (IndexOutOfRangeException) { }
            if (blockCheck != null)
                return false;
            return true;
        }

        public static bool IsSymmetrical(List<Block> blocks) {
            int centerX = 0, centerY = 0;
            foreach(Block block in blocks) {
                centerX += block.X + Block.Size / 2;
                centerY += block.Y + Block.Size / 2;
            }

            centerX /= blocks.Count;
            centerY /= blocks.Count;

            bool[] checkArray = new bool[blocks.Count];
            int diffX, diffY;
            int i, j, k;

            for (i = 0; i < blocks.Count; i++) {
                // If it wasn't verified yet
                if (!checkArray[i]) {
                            
                    if (blocks[i].X + Block.Size / 2 == centerX && blocks[i].Y + Block.Size / 2 == centerY)
                        continue;

                    diffX = blocks[i].X + Block.Size / 2 - centerX;
                    diffY = blocks[i].Y + Block.Size / 2 - centerY;

                    // Three symmetrical blocks should be found, if one is not then shape is not symmetrical.
                    int[] dx = {-diffX, diffY, -diffY };
                    int[] dy = {-diffY, diffX, -diffX};
                    
                    for (k = 0; k < 3; k++) {
                        for (j = 0; j < blocks.Count; j++)
                            if (blocks[j].X + Block.Size / 2 - centerX == dx[k] && blocks[j].Y + Block.Size / 2 - centerY == dy[k]) {
                                checkArray[j] = true;
                                break;
                            }
                        if (j >= blocks.Count)
                            return false;
                    }
                }   
            }
            return true;
        }

        

        public static Color GetRandomColor() {
            if (colors == null) {
                colors = new Color[] {
                    Color.Red, Color.Yellow, Color.Blue, Color.Black
                };
            }
            return colors[random.Next(colors.Length)];
        }

        public static Shape GetRandomShape(Board board) {
            if (patterns == null) {
                patterns = new List<int[]> {
                    //new int[] {1, 0, -1, 0, 0, 1, -1, -1, 1, -1},
                    //new int[] {0, -1, 0, -2, 0, 1},
                    //new int[] {1, 0, -1, 0, 0, 1, 0, -1},
                    //new int[] {1, 0, -1, 0, 0, -1},
                    //new int[] {0, -1, -1, -1, -1, 0},
                    //new int[] {-2, 0, -3, 0, -2, -1, -3, -1, -1, -3, -1, -2, 0, -3, 0, -2, 1, 0, 2, 0, 1, -1, 2, -1, -1, 1, -1, 2, 0, 1, 0, 2, -1, -1, -1, 0, 0, -1}, // lol
                    new int[] {-8, 0, -7, 0, -6, 0, -5, 0, -4, 0, -3, 0, -2, 0, -1, 0, 1, 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7, 0}
                };
            }
            return new Shape(board, patterns[random.Next(0, patterns.Count)]);
        }

        public static Texture2D GetTextureForColor(Color color) {
            if (textures == null) {
                textures = new Dictionary<Color, Texture2D>();
                foreach (Color c in colors) {
                    textures.Add(c, PrepareBlockTexture(c));
                }
            }
            return textures[color];
        }
    }
}

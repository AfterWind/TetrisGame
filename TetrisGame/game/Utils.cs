using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    class Utils {

        public static Random random = new Random();
        public static int[] GetDifferentialParams(List<Block> blocks, Block center) {
            int[] array = new int[(blocks.Count - 1) * 2];
            int i = 0;
            foreach (Block block in blocks) {
                if (block != center) {
                    array[i++] = (block.X - center.X) / Block.size;
                    array[i++] = (block.Y - center.Y) / Block.size;
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
            return new Point(board.PosX + board.SizeX / 2, minY * (-1) * Block.size + board.PosY);
        }

        public static bool IsPositionValid(Board board, int x, int y) {
            if (x < board.PosX)
                return false;
            if (x + Block.size > board.PosX + board.SizeX)
                return false;
            if (y < board.PosY)
                return false;
            if (y + Block.size > board.PosY + board.SizeY)
                return false;
            Block blockCheck = null;
            try {
                blockCheck = board.blockArray[(x - board.PosX) / Block.size, (y - board.PosY) / Block.size];
            } catch (IndexOutOfRangeException) { }
            if (blockCheck != null)
                return false;
            return true;
        }

        public static bool IsSymmetrical(List<Block> blocks) {

            int centerX = 0, centerY = 0;
            foreach(Block block in blocks) {
                centerX += block.X + Block.size / 2;
                centerY += block.Y + Block.size / 2;
            }

            centerX /= blocks.Count;
            centerY /= blocks.Count;

            bool[] checkArray = new bool[blocks.Count];
            int diffX, diffY;
            int i, j, k;

            for (i = 0; i < blocks.Count; i++) {
                // If it wasn't verified yet
                if (!checkArray[i]) {
                            
                    if (blocks[i].X + Block.size / 2 == centerX && blocks[i].Y + Block.size / 2 == centerY)
                        continue;

                    diffX = blocks[i].X + Block.size / 2 - centerX;
                    diffY = blocks[i].Y + Block.size / 2 - centerY;

                    // Three symmetrical blocks should be found, if one is not then shape is not symmetrical.
                    int[] dx = {-diffX, diffY, -diffY };
                    int[] dy = {-diffY, diffX, -diffX};
                    
                    for (k = 0; k < 3; k++) {
                        for (j = 0; j < blocks.Count; j++)
                            if (blocks[j].X + Block.size / 2 - centerX == dx[k] && blocks[j].Y + Block.size / 2 - centerY == dy[k]) {
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

        private static Color[] colors = new Color[] {
            Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.Black
        };

        public static Color GetRandomColor() {
            return colors[random.Next(colors.Length)];
        }

        public static Shape[] patterns;



        public static Shape GetRandomShape() {
            return new Shape(patterns[random.Next(patterns.Length)]);
        }
    }
}

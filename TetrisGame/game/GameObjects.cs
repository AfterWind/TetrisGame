using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisGame.game;

namespace TetrisGame {
    class GameObjects {

        public static Color fromColor = Color.FromNonPremultiplied(96, 96, 96, 255);
        public static Board board1=new Board(20, 20, 16, 20);
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







}

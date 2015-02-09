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
        private static Board board1;
        public static Board GetBoard() {
            if(board1 == null)
                board1 = new Board(20, 20, 16, 20);
            return board1;
        }

        public static ContentManager content;
        public static void Init(ContentManager content) {
            GameObjects.content = content;
            Utils.patterns = new Shape[] {
            //new Shape(GetBoard(), 1, 0, -1, 0, 0, 1, -1, -1, 1, -1),
            new Shape(GetBoard(), 0, 1, 0, 2, 0, 3),
            //new Shape(GetBoard(), 1, 0, -1, 0, 0, 1, 0, -1),
            //new Shape(GetBoard(), 1, 0, -1, 0, 0, -1),
            //new Shape(GetBoard(), 1, 0, 0, -1, 1, -1),
            //new Shape(GetBoard(), 0, -1, 0, -2, 0, -3, 1, 0, 1, -1, 1, -2, 1, -3, -1, 0, -2, 0, -1, -1, -2, -1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 0, 3, 0, 2, -1, 3, -1) // lol
            };
            
            GameObjects.GetBoard().AddShape(Utils.GetRandomShape());
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

        
    }







}

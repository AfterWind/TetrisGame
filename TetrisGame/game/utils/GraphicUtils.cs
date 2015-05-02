using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    class GraphicUtils {

        public static Texture2D pixel;
        public static SpriteFont font;
        public static int screenWidth = 1600;
        public static int screenHeight = 720;

        public static void DrawBorder(SpriteBatch batch, Color color, int posX, int posY, int sizeX, int sizeY, int borderSize) {
            pixel.SetData(new Color[] { color });
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(pixel, new Rectangle(posX - borderSize, posY - borderSize, borderSize, sizeY + borderSize), Color.White);
            batch.Draw(pixel, new Rectangle(posX - borderSize, posY + sizeY, sizeX + borderSize, borderSize), Color.White);
            batch.Draw(pixel, new Rectangle(posX, posY - borderSize, sizeX + borderSize, borderSize), Color.White);
            batch.Draw(pixel, new Rectangle(posX + sizeX, posY, borderSize, sizeY + borderSize), Color.White);
            batch.End();
        }

        public static void DrawRectangle(SpriteBatch batch, Color color, int posX, int posY, int sizeX, int sizeY) {
            pixel.SetData(new Color[] { color });
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(pixel, new Rectangle(posX, posY, sizeX, sizeY), Color.White);
            batch.End();
        }

        public static void DrawString(SpriteBatch batch, Color color, Vector2 pos, string text) {
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.DrawString(font, text, pos, color);
            batch.End();
        }
    }
}

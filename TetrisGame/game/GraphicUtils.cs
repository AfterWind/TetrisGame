using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    class GraphicUtils {

        public static Texture2D Pixel = new Texture2D(GameObjects.GraphicsDevice, 1, 1);

        public static void DrawRectangle(SpriteBatch batch, Color color, int posX, int posY, int sizeX, int sizeY, int borderSize) {
            
            Pixel.SetData(new Color[] { color });
            batch.Begin();
            batch.Draw(Pixel, new Rectangle(posX - borderSize, posY - borderSize, borderSize, sizeY + borderSize), Color.White);
            batch.Draw(Pixel, new Rectangle(posX - borderSize, posY + sizeY, sizeX + borderSize, borderSize), Color.White);
            batch.Draw(Pixel, new Rectangle(posX, posY - borderSize, sizeX + borderSize, borderSize), Color.White);
            batch.Draw(Pixel, new Rectangle(posX + sizeX, posY, borderSize, sizeY + borderSize), Color.White);
            batch.End();
        }

        public static void DrawTransparentRectangle(SpriteBatch batch, Color color, int posX, int posY, int sizeX, int sizeY) {

            Pixel.SetData(new Color[] { color });
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(Pixel, new Rectangle(posX, posY, sizeX, sizeY), Color.White);
            batch.End();
        }

    }
}

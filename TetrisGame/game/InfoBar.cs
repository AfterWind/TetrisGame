using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class InfoBar {

        public static readonly int INFO_BAR_SIZE = 200;

        public int Score { set; get; }
        public int Level { private set; get; }
        public int RowsCleared { private set; get; }

        public int PosX { private set; get; }
        public int PosY { private set; get; }

        public InfoBar(int posX, int posY) {
            PosX = posX;
            PosY = posY;
            Score = 0;
            Level = 1;
            RowsCleared = 0;
        }

        public void IncreaseRowsCleared() {
            RowsCleared++;
            Score += Level * 50;
            if (RowsCleared % 5 == 0)
                Level++;
            
        }

        public void Draw(SpriteBatch batch) {
            GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, PosX, 0, INFO_BAR_SIZE, GraphicUtils.screenHeight);
            Vector2 textPos = GraphicUtils.fontCommon.MeasureString("SCORE");
            textPos.X = (INFO_BAR_SIZE - textPos.X) / 2 + PosX;
            textPos.Y =  50 + PosY;
            GraphicUtils.DrawString(batch, Color.White, textPos, "SCORE");
            textPos = GraphicUtils.fontCommon.MeasureString(Score.ToString());
            textPos.X = (INFO_BAR_SIZE - textPos.X) / 2 + PosX;
            textPos.Y = 70 + PosY;
            GraphicUtils.DrawString(batch, Color.White, textPos, Score.ToString());
        }

    }
}

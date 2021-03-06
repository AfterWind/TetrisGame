﻿using Microsoft.Xna.Framework;
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
            Reset();
        }

        public void IncreaseRowsCleared() {
            RowsCleared++;
            Score += Level * 50;
            if (RowsCleared % 5 == 0)
                Level++;
        }

        public void Reset() {
            Score = 0;
            Level = 1;
            RowsCleared = 0;
        }

        public void Draw(SpriteBatch batch) {
            GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, PosX, 0, INFO_BAR_SIZE, GraphicUtils.screenHeight);

            Vector2 scorePos = GraphicUtils.fontCommon.MeasureString("SCOR");
            scorePos.X = (INFO_BAR_SIZE - scorePos.X) / 2 + PosX;
            scorePos.Y = 50 + PosY;

            Vector2 scorePointsPos = GraphicUtils.fontCommon.MeasureString(Score.ToString());
            scorePointsPos.X = (INFO_BAR_SIZE - scorePointsPos.X) / 2 + PosX;
            scorePointsPos.Y = 70 + PosY;

            Vector2 levelPos = GraphicUtils.fontCommon.MeasureString("NIVELUL");
            levelPos.X = (INFO_BAR_SIZE - levelPos.X) / 2 + PosX;
            levelPos.Y = 170 + PosY;

            Vector2 levelPointsPos = GraphicUtils.fontCommon.MeasureString(Level.ToString());
            levelPointsPos.X = (INFO_BAR_SIZE - levelPointsPos.X) / 2 + PosX;
            levelPointsPos.Y = 198 + PosY;

            GraphicUtils.DrawRectangle(batch, Color.DarkRed, PosX, 200 + PosY, (RowsCleared % 5) * (INFO_BAR_SIZE / 5), 20);
            GraphicUtils.DrawBorder(batch, Color.White, PosX + 3, 200 + PosY, INFO_BAR_SIZE - 6, 20, 3);
            
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.DrawString(GraphicUtils.fontCommon, "SCOR", scorePos, Color.White);
            batch.DrawString(GraphicUtils.fontCommon, Score.ToString(), scorePointsPos, Color.White);
            batch.DrawString(GraphicUtils.fontCommon, "NIVELUL", levelPos, Color.White);
            batch.DrawString(GraphicUtils.fontCommon, Level.ToString(), levelPointsPos, Color.White);
            batch.End();

            
        }
    }
}

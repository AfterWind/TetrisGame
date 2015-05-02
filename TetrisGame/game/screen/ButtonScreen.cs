using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class ButtonScreen {
        public static readonly Color TITLE_TEXT_COLOR = Color.White;
        public static readonly int BACKGROUND_STRIP_OFFX = 130;
        public static readonly int MAX_TEXT_SIZE = Button.SizeX + BACKGROUND_STRIP_OFFX * 2 - 50;
        
        public static readonly int Distance = 25;
        public static int ButtonsPosStartX = (GraphicUtils.screenWidth - Button.SizeX) / 2, ButtonPosStartY = 280;

        public List<Button> buttons = new List<Button>();
        public string Title { set; get; }
        
        private int selected = 0;

        public ButtonScreen(params Button[] buttons) {
            this.buttons.AddRange(buttons);
        }

        public void Draw(SpriteBatch batch) {
            GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, ButtonsPosStartX - BACKGROUND_STRIP_OFFX, 0, Button.SizeX + BACKGROUND_STRIP_OFFX * 2, GraphicUtils.screenHeight);
            if (Title != null) {
                Vector2 dim = GraphicUtils.fontTitle.MeasureString(Title);
                batch.Begin();
                batch.DrawString(GraphicUtils.fontTitle, Title, new Vector2((GraphicUtils.screenWidth - dim.X) / 2, 150), TITLE_TEXT_COLOR);
                batch.End();
            }
            for (int i = 0; i < buttons.Count; i++) {
                int x = ButtonsPosStartX, y = ButtonPosStartY + i * (Button.SizeY + Distance);
                Vector2 textPos = GraphicUtils.fontCommon.MeasureString(buttons[i].Text);
                //Console.WriteLine("Text size: " + textPos);

                if (i == selected)
                    buttons[i].BorderColor = Color.White;
                else
                    buttons[i].BorderColor = Color.Black;

                textPos.X = Button.SizeX / 2 - textPos.X / 2 + x;
                textPos.Y = Button.SizeY / 2 - textPos.Y / 2 + y + 2;
                //Console.WriteLine("Text position: " + textPos);
                //Console.WriteLine("Button size: " + Button.SizeX + ", " + Button.SizeY);
                
                GraphicUtils.DrawRectangle(batch, Button.buttonColor, x, y, Button.SizeX, Button.SizeY);
                GraphicUtils.DrawBorder(batch, buttons[i].BorderColor, x, y, Button.SizeX, Button.SizeY, Button.BorderSize);
                GraphicUtils.DrawString(batch, Button.textColor, textPos, buttons[i].Text);
            }
        }

        public void DrawTitle(SpriteFont font) {
            Vector2 textSize = font.MeasureString(Title);
            //if(textSize > )
        }

        public void OnKeyboardPress(KeyboardState keys) {
            foreach (Keys k in keys.GetPressedKeys())
            {
                if (!GameObjects.Game.WasKeyPressed(k))
                {
                    switch (k)
                    {
                        case Keys.Down:
                            buttons[selected].BorderColor = Color.Black;
                            if (selected < buttons.Count - 1)
                                selected++;
                            buttons[selected].BorderColor = Color.White;
                            break;
                        case Keys.Up:
                            buttons[selected].BorderColor = Color.Black;
                            if (selected > 0)
                                selected--;
                            buttons[selected].BorderColor = Color.White;
                            break;
                        case Keys.Enter:
                            buttons[selected].OnClicked();
                            break;
                        case Keys.Space:
                            buttons[selected].OnClicked();
                            break;
                    }
                }
            }
        }
    }
}

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
        public readonly int MAX_TEXT_SIZE;
        
        public int Distance = 25;
        public int ButtonSizeX;
        public int ButtonsPosStartX, ButtonsPosStartY;

        public List<Button> buttons = new List<Button>();
        public string Title { set; get; }
        
        protected int selected = 0;

        public ButtonScreen(int buttonSizeX, params Button[] buttons) {
            this.buttons.AddRange(buttons);
            ButtonSizeX = buttonSizeX;
            ButtonsPosStartX = (GraphicUtils.screenWidth - ButtonSizeX) / 2;
            ButtonsPosStartY = 280;
            MAX_TEXT_SIZE = ButtonSizeX + BACKGROUND_STRIP_OFFX * 2 - 50;
        }

        public virtual void Draw(SpriteBatch batch) {
            GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, ButtonsPosStartX - BACKGROUND_STRIP_OFFX, 0, ButtonSizeX + BACKGROUND_STRIP_OFFX * 2, GraphicUtils.screenHeight);
            if (Title != null) {
                Vector2 dim = GraphicUtils.fontTitle.MeasureString(Title);
                batch.Begin();
                batch.DrawString(GraphicUtils.fontTitle, Title, new Vector2((GraphicUtils.screenWidth - dim.X) / 2, 150), TITLE_TEXT_COLOR);
                batch.End();
            }
            for (int i = 0; i < buttons.Count; i++) {
                int x = ButtonsPosStartX, y = ButtonsPosStartY + i * (Button.SizeY + Distance);
                Vector2 textPos = GraphicUtils.fontCommon.MeasureString(buttons[i].Text);
                //Console.WriteLine("Text size: " + textPos);

                if (i == selected)
                    buttons[i].BorderColor = Color.White;
                else
                    buttons[i].BorderColor = Color.Black;

                textPos.X = ButtonSizeX / 2 - textPos.X / 2 + x;
                textPos.Y = Button.SizeY / 2 - textPos.Y / 2 + y + 2;
                //Console.WriteLine("Text position: " + textPos);
                //Console.WriteLine("Button size: " + Button.SizeX + ", " + Button.SizeY);
                
                GraphicUtils.DrawRectangle(batch, Button.buttonColor, x, y, ButtonSizeX, Button.SizeY);
                GraphicUtils.DrawBorder(batch, buttons[i].BorderColor, x, y, ButtonSizeX, Button.SizeY, Button.BorderSize);
                batch.Begin();
                batch.DrawString(GraphicUtils.fontCommon, buttons[i].Text, textPos, Button.textColor);
                batch.End();
            }
        }

        public virtual void OnKeyboardPress(KeyboardState keys) {
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

    public class ButtonWindow : ButtonScreen {
        public ButtonWindow(int buttonSizeX, params Button[] buttons) : base(buttonSizeX, buttons) {
            Distance = 80;
            ButtonsPosStartX = (GraphicUtils.screenWidth - buttons.Count() * ButtonSizeX - (buttons.Count() - 1) * Distance) / 2;
            ButtonsPosStartY = 360;
            //MAX_TEXT_SIZE = 400;
        }

        public override void Draw(SpriteBatch batch) {
            GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, ButtonsPosStartX - BACKGROUND_STRIP_OFFX, 0, buttons.Count * ButtonSizeX + (buttons.Count - 1) * Distance + BACKGROUND_STRIP_OFFX * 2, GraphicUtils.screenHeight);
            if (Title != null) {
                Vector2 dim = GraphicUtils.fontTitle.MeasureString(Title);
                batch.Begin();
                batch.DrawString(GraphicUtils.fontTitle, Title, new Vector2((GraphicUtils.screenWidth - dim.X) / 2, 150), TITLE_TEXT_COLOR);
                batch.End();
            }
            for (int i = 0; i < buttons.Count; i++) {
                int x = ButtonsPosStartX + i * (ButtonSizeX + Distance), y = ButtonsPosStartY;
                Vector2 textPos = GraphicUtils.fontCommon.MeasureString(buttons[i].Text);

                if (i == selected)
                    buttons[i].BorderColor = Color.White;
                else
                    buttons[i].BorderColor = Color.Black;

                textPos.X = ButtonSizeX / 2 - textPos.X / 2 + x;
                textPos.Y = Button.SizeY / 2 - textPos.Y / 2 + y + 2;
                //Console.WriteLine("Text position: " + textPos);
                //Console.WriteLine("Button size: " + Button.SizeX + ", " + Button.SizeY);

                GraphicUtils.DrawRectangle(batch, Button.buttonColor, x, y, ButtonSizeX, Button.SizeY);
                GraphicUtils.DrawBorder(batch, buttons[i].BorderColor, x, y, ButtonSizeX, Button.SizeY, Button.BorderSize);
                batch.Begin();
                batch.DrawString(GraphicUtils.fontCommon, buttons[i].Text, textPos, Button.textColor);
                batch.End();
            }
        }
        
        public override void OnKeyboardPress(KeyboardState keys) {
            foreach (Keys k in keys.GetPressedKeys()) {
                if (!GameObjects.Game.WasKeyPressed(k)) {
                    switch (k) {
                        case Keys.Right:
                            buttons[selected].BorderColor = Color.Black;
                            if (selected < buttons.Count - 1)
                                selected++;
                            buttons[selected].BorderColor = Color.White;
                            break;
                        case Keys.Left:
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

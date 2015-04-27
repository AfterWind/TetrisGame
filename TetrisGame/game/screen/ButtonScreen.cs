﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class ButtonScreen {
        public static int Distance = 25;
        public static int ButtonsPosStartX = 30, ButtonPosStartY = 40;
        
        public List<Button> buttons = new List<Button>();

        private int selected = 0;

        public ButtonScreen(params Button[] buttons) {
            this.buttons.AddRange(buttons);
        }

        public void Draw(SpriteBatch batch) {
            for(int i = 0; i < buttons.Count; i++) {
                int x = ButtonsPosStartX, y = ButtonPosStartY + i * (Button.SizeY + Distance);
                Vector2 textPos = GraphicUtils.font.MeasureString(buttons[i].Text) / 2;
                //Console.WriteLine("Text size: " + textPos);

                if (i == selected)
                    buttons[i].BorderColor = Color.White;
                else
                    buttons[i].BorderColor = Color.Black;

                textPos.X = Button.SizeX / 2 - textPos.X / 2 + x;
                textPos.Y = Button.SizeY / 2 - textPos.Y / 2 + y;
                //Console.WriteLine("Text position: " + textPos);
                //Console.WriteLine("Button size: " + Button.SizeX + ", " + Button.SizeY);

                GraphicUtils.DrawRectangle(batch, Button.buttonColor, x, y, Button.SizeX, Button.SizeY);
                GraphicUtils.DrawBorder(batch, buttons[i].BorderColor, x - Button.BorderSize, y - Button.BorderSize, Button.SizeX + 2 * Button.BorderSize, Button.SizeY + 2 * Button.BorderSize, Button.BorderSize);
                GraphicUtils.DrawString(batch, Button.textColor, textPos, buttons[i].Text);
            }
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

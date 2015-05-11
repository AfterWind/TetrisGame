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

        protected List<Button> buttons = new List<Button>();
        public string Title { set; get; }
        public ButtonScreen Parent { private set; get; }
        protected int selected = 0;

        public ButtonScreen(int buttonSizeX) {
            ButtonSizeX = buttonSizeX;
            ButtonsPosStartX = (GraphicUtils.screenWidth - ButtonSizeX) / 2;
            ButtonsPosStartY = 280;
            MAX_TEXT_SIZE = ButtonSizeX + BACKGROUND_STRIP_OFFX * 2 - 50;
        }

        public virtual void AddButtons(params Button[] buttons) {
            this.buttons.AddRange(buttons);
            foreach (Button b in buttons) {
                if (b is BackButton)
                    Parent = ((BackButton)b).PrevScreen;
                b.SetOwner(this);
            }
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
                buttons[i].Draw(batch, ButtonsPosStartX, ButtonsPosStartY + i * (Button.SizeY + Distance), i == selected);
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
                        case Keys.Escape:
                            if(Parent != null)
                                GameObjects.CurrentButtonScreen = Parent;
                            break;
                        default:
                            buttons[selected].OnClicked(k);
                            break;
                    }
                }
            }
        }
    }

    public class ButtonWindow : ButtonScreen {
        public ButtonWindow(int buttonSizeX) : base(buttonSizeX) {
            Distance = 80;
            ButtonsPosStartY = 360;
            //MAX_TEXT_SIZE = 400;
        }

        public override void AddButtons(params Button[] buttons) {
            base.AddButtons(buttons);
            ButtonsPosStartX = (GraphicUtils.screenWidth - buttons.Count() * ButtonSizeX - (buttons.Count() - 1) * Distance) / 2;
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
                buttons[i].Draw(batch, ButtonsPosStartX + i * (ButtonSizeX + Distance), ButtonsPosStartY, i == selected);
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
                        default:
                            buttons[selected].OnClicked(k);
                            break;
                    }
                }
            }
        }
    }

    public class ImageScreen : ButtonScreen {
        public Texture2D tex;

        public ImageScreen(int buttonSizeX, Texture2D tex) : base(buttonSizeX) {
            ButtonSizeX = buttonSizeX;
            ButtonsPosStartX = (GraphicUtils.screenWidth - ButtonSizeX) / 2;
            ButtonsPosStartY = GraphicUtils.screenHeight - 100;
            this.tex = tex;
        }

        public override void Draw(SpriteBatch batch) {
            //GraphicUtils.DrawRectangle(batch, GraphicUtils.BACKGROUND_STRIP, ButtonsPosStartX - BACKGROUND_STRIP_OFFX, 0, ButtonSizeX + BACKGROUND_STRIP_OFFX * 2, GraphicUtils.screenHeight);
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(tex, new Rectangle(0, 0, GraphicUtils.screenWidth, GraphicUtils.screenHeight), Color.White);
            batch.End();
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].Draw(batch, ButtonsPosStartX, ButtonsPosStartY + i * (Button.SizeY + Distance), i == selected);
           }
        }
    }
}

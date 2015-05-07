using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public abstract class Button {
        public const int BorderSize = 5, SizeY = 40;
        public static Color buttonColor = Color.Orange, textColor = Color.Blue;

        public ButtonScreen ButtonScreen { protected set; get; }
        public string Text { protected set; get; }
        public Color BorderColor { set; get; }

        public Button() { 
        }

        public Button(string text) {
            this.Text = text;
        }

        public virtual void SetOwner(ButtonScreen buttonScreen) {
            this.ButtonScreen = buttonScreen;
        }

        public virtual void Draw(SpriteBatch batch, int x, int y, bool selected) {
            Vector2 textPos = GraphicUtils.fontCommon.MeasureString(Text);

            if (selected)
                this.BorderColor = Color.White;
            else
                this.BorderColor = Color.Black;

            textPos.X = ButtonScreen.ButtonSizeX / 2 - textPos.X / 2 + x;
            textPos.Y = Button.SizeY / 2 - textPos.Y / 2 + y + 2;

            GraphicUtils.DrawRectangle(batch, Button.buttonColor, x, y, ButtonScreen.ButtonSizeX, Button.SizeY);
            GraphicUtils.DrawBorder(batch, this.BorderColor, x, y, ButtonScreen.ButtonSizeX, Button.SizeY, Button.BorderSize);
            batch.Begin();
            batch.DrawString(GraphicUtils.fontCommon, this.Text, textPos, Button.textColor);
            batch.End();
        }

        public abstract void OnClicked(Keys key);

        public bool IsKeyEnter(Keys key) {
            return key == Keys.Enter || key == Keys.Space;
        }
    }

    public class QuitButton : Button {
        public QuitButton(String text) : base(text) { }

        public override void OnClicked(Keys key) {
            if(IsKeyEnter(key))
                GameObjects.Game.Exit();
        }
    }

    public class AdvanceButton : Button {

        public ButtonScreen NextScreen { private set; get; }

        public AdvanceButton(string text, ButtonScreen nextScreen) : base(text) {
            this.NextScreen = nextScreen;
        }

        public override void SetOwner(ButtonScreen buttonScreen) {
            base.SetOwner(buttonScreen);
            this.NextScreen.AddButtons(new BackButton(ButtonScreen));
        }

        public override void OnClicked(Keys key) {
            if (IsKeyEnter(key))
                GameObjects.CurrentButtonScreen = NextScreen;
        }
    }

    public class BackButton : Button {
        public ButtonScreen PrevScreen { private set; get; }

        public BackButton(ButtonScreen prevScreen) : base("Inapoi") {
            this.PrevScreen = prevScreen;
        }

        public override void OnClicked(Keys key) {
            if (IsKeyEnter(key))
                GameObjects.CurrentButtonScreen = PrevScreen;
        }
    }

    public class StartButton : Button {
        public StartButton(string text) : base(text) { }

        public override void OnClicked(Keys key) {
            if (IsKeyEnter(key))
                GameObjects.StartGame();
        }
    }

    public class DifficultyButton : StartButton {
        private Difficulty diff;
        
        public DifficultyButton(Difficulty diff) : base(diff.GetText()) {
            this.diff = diff;
        }

        public override void OnClicked(Keys key) {
            if (IsKeyEnter(key)) {
                GameObjects.SetDifficulty(diff);
            }
            base.OnClicked(key);
        }
    }

    public class GameReturnButton : Button {
        public GameReturnButton(string text) : base(text) { }

        public override void OnClicked(Keys key) {
            if (IsKeyEnter(key))
                if (GameObjects.GamePaused) {
                    GameObjects.UnpauseGame();
                }
        }
    }

    public class TitleReturnButton : Button {
        public TitleReturnButton(string text) : base(text) { }

        public override void OnClicked(Keys key) {
            if (IsKeyEnter(key)) {
                GameObjects.Game.gameStarted = false;
                GameObjects.CurrentButtonScreen = GameObjects.TitleScreen;
                GameObjects.UnpauseGame();
            }
        }
    }

    public abstract class SelectButton : Button {
        public object[] objects;
        public int selected = 0;

        public SelectButton(params object[] objects) {
            this.objects = objects;
        }

        public override void OnClicked(Keys key) {
            switch(key) {
                case Keys.Left:
                    if (selected != 0)
                        selected--;
                    break;
                case Keys.Right:
                    if (selected != objects.Count() - 1)
                        selected++;
                    break;
            }
        }

        public override void Draw(SpriteBatch batch, int x, int y, bool isSelected) {
            base.Draw(batch, x, y, isSelected);

            if (selected != objects.Count() - 1) {
                GraphicUtils.DrawBorder(batch, BorderColor, x + ButtonScreen.ButtonSizeX + 10, y, 20, Button.SizeY, 2);
                GraphicUtils.DrawRectangle(batch, buttonColor, x + ButtonScreen.ButtonSizeX + 10, y, 20, Button.SizeY);
            }

            if (selected != 0) {
                GraphicUtils.DrawBorder(batch, BorderColor, x - 30, y, 20, Button.SizeY, 2);
                GraphicUtils.DrawRectangle(batch, buttonColor, x - 30, y, 20, Button.SizeY);
            }

            Vector2 leftPos = GraphicUtils.fontCommon.MeasureString("<");
            leftPos.X = (20 - leftPos.X) / 2 + x - 30;
            leftPos.Y = Button.SizeY / 2 - leftPos.Y / 2 + y + 2;

            Vector2 rightPos = GraphicUtils.fontCommon.MeasureString(">");
            rightPos.X = (20 - rightPos.X) / 2 + x + ButtonScreen.ButtonSizeX + 10;
            rightPos.Y = Button.SizeY / 2 - rightPos.Y / 2 + y + 2;

            batch.Begin();
            if (selected != 0)
                batch.DrawString(GraphicUtils.fontCommon, "<", leftPos, textColor);
            if (selected != objects.Count() - 1)
                batch.DrawString(GraphicUtils.fontCommon, ">", rightPos, textColor);
            batch.End();

        }
    }

    public class PatternSelectButton : SelectButton {
        public PatternSelectButton(params PatternSet[] patterns) : base(patterns) {
            this.Text = patterns[selected].Name;
        }

        public override void OnClicked(Keys key) {
            base.OnClicked(key);
            if (key == Keys.Left || key == Keys.Right) {
                Utils.SelectedPattern = selected;
                this.Text = ((PatternSet)objects[selected]).Name;
            }
        }
    }

    public class BoardSizeSelectButton : SelectButton {
        public BoardSizeSelectButton(params BoardSize[] boardSizes) : base(boardSizes) {
            this.Text = boardSizes[selected].rows + " x " + boardSizes[selected].columns;
        }

        public override void OnClicked(Keys key) {
            base.OnClicked(key);
            if (key == Keys.Left || key == Keys.Right) {
                GameObjects.BoardSize = ((BoardSize)objects[selected]);
                this.Text = ((BoardSize)objects[selected]).rows + " x " + ((BoardSize)objects[selected]).columns;
            }
        }
    }
}

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
        public const int SizeX = 300, SizeY = 40, BorderSize = 5;
        public static Color buttonColor = Color.Orange, textColor = Color.Blue;

        public string Text { protected set; get; }
        public Color BorderColor { set; get; }

        public Button() { 
        }

        public Button(string text) {
            this.Text = text;
        }

        public abstract void OnClicked();
    }

    public class QuitButton : Button {
        public QuitButton(String text) : base(text) { }

        public override void OnClicked() {
            GameObjects.Game.Exit();
        }
    }

    public class AdvanceButton : Button {

        public ButtonScreen NextScreen { private set; get; }

        public AdvanceButton(string text, ButtonScreen nextScreen) : base(text) {
            this.NextScreen = nextScreen;
        }

        public override void OnClicked() {
            GameObjects.CurrentButtonScreen = NextScreen;
        }
    }

    public class BackButton : AdvanceButton {
        public BackButton(ButtonScreen prevScreen) : base("Back", prevScreen) { }
    }

    public class StartButton : Button {
        public StartButton(string text) : base(text) { }

        public override void OnClicked() {
            GameObjects.StartGame();
        }
    }

    public class DifficultyButton : StartButton {
        private Difficulty diff;
        
        public DifficultyButton(Difficulty diff) : base(diff.GetText()) {
            this.diff = diff;
        }

        public override void OnClicked() {
            GameObjects.SetDifficulty(diff);
            base.OnClicked();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.game {
    public class Button {
        public const int SizeX = 300, SizeY = 50, BorderSize = 5;
        public static Color buttonColor = Color.Gray, textColor = Color.Blue;

        public string Text { protected set; get; }
        public Color BorderColor { protected set; get; }

        public delegate void OnClicked(Keys key);
        public OnClicked handler;

        public Button() { 
        }

        public Button(string text, OnClicked handler) {
            this.Text = text;
            this.handler = handler;
        }

        public void OnSelected() {
            BorderColor = Color.White;
        }

        public void OnUnselected() {
            BorderColor = Color.Black;
        }
    }
}

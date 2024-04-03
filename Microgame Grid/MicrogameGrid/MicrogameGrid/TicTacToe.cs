using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgame_Grid
{
    internal class TicTacToe : Microgame
    {
        private static bool[] ticTacs;
        private Rectangle? clickedRect;

        private Texture2D circle;
        private Texture2D cross;

        private bool win;
       

        public override Rectangle? ClickedRect
        {
            get { return clickedRect; }
        }
        public override bool Win
        {
            get { return win; }
        }
        public TicTacToe(Texture2D circle, Texture2D cross)
        {
            ticTacs = Setup();
            this.circle = circle;
            this.cross = cross;
        }

        private bool[] Setup()
        {
            bool[] result = new bool[4];
            int number;
            for (int i = 0; i < result.Length; i++)
            {
                number = Game1.CoinFlip();
                if (number == 1)
                {
                    result[i] = true;
                }
                else
                {
                    result[i] = false;
                }
            }
            if (result.Count(b => b == true) < 2)
            {
                result = Setup();
            }
            else if ((result.Count(b => b == true) == 2) && (result[1] && result[2]))
            {
                result = Setup();
            }
            return result;
        }

        public override void Update(MouseState[] state, Rectangle[] gridSlots)
        {
            if (LeftClick(state, gridSlots[2]))
            {
                clickedRect = gridSlots[2];
            }
            else if (LeftClick(state, gridSlots[5]))
            {
                clickedRect = gridSlots[5];
            }
            else if (LeftClick(state, gridSlots[6]))
            {
                clickedRect = gridSlots[6];
            }
            else if (LeftClick(state, gridSlots[7]))
            {
                clickedRect = gridSlots[7];
            }
            else if (LeftClick(state, gridSlots[8]))
            {
                clickedRect = gridSlots[8];
            }

            win = WinCheck(gridSlots);

            if (win)
            {
                Game1.WinCount++;
            }
        }

        public override void Draw(SpriteBatch sb, Rectangle[] gridSlots)
        {
            int arraySlot = 0;
            for (int i = 0; i < Math.Sqrt(ticTacs.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(ticTacs.Length); j++)
                {
                    if (ticTacs[arraySlot])
                    {
                        sb.Draw(cross, gridSlots[arraySlot + i], Color.White);
                    }
                    else
                    {
                        sb.Draw(circle, gridSlots[arraySlot + i], Color.White);
                    }
                    arraySlot++;
                }
            }

            if (clickedRect != null)
            {
                sb.Draw(cross, (Rectangle)clickedRect, Color.White);
            }
        }

        private bool WinCheck(Rectangle[] gridSlots)
        {
            if (ticTacs[0])
            {
                return (ticTacs[1] && clickedRect == gridSlots[2]) ||
                    (ticTacs[2] && clickedRect == gridSlots[6]) ||
                    (ticTacs[3] && clickedRect == gridSlots[8]) ||
                    (ticTacs[1] && ticTacs[3] && clickedRect == gridSlots[7]) ||
                    (ticTacs[2] && ticTacs[3] && clickedRect == gridSlots[5]);
            }
            else if (ticTacs[1] || ticTacs[2])
            {
                return (ticTacs[1] && ticTacs[3] && clickedRect == gridSlots[7]) ||
                    (ticTacs[2] && ticTacs[3] && clickedRect == gridSlots[5]);
            }
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            ticTacs = Setup();
            clickedRect = null;
            win = false;
        }
    }
}

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
    internal class BlueSquare : Microgame
    {
        private int[] tiles;
        private Texture2D square;

        private Rectangle? clickedRect;
        private bool win;
        private int goalSlot;

        public override Rectangle? ClickedRect
        {
            get { return clickedRect; }
        }

        public override bool Win
        {
            get { return win; }
        }

        public BlueSquare(Texture2D square)
        {
            this.square = square;
            goalSlot = RandomNine();
            tiles = Setup(goalSlot);
        }
        private int[] Setup(int goalSlot)
        {
            int[] result = new int[9];
            for (int i = 0; i < result.Length; i++)
            {
                if (i == goalSlot)
                {
                    result[i] = 2;
                }
                else
                {
                    result[i] = 1;
                }
            }
            return result;
        }

        public override void Update(MouseState[] state, Rectangle[] gridSlots)
        {
            clickedRect = gridSlots[goalSlot];
            if (LeftClick(state, (Rectangle)clickedRect))
            {
                win = true;
            }

            if (win)
            {
                Game1.WinCount++;
            }
        }

        public override void Draw(SpriteBatch sb, Rectangle[] gridSlots)
        {
            int arraySlot = 0;
            for (int i = 0; i < Math.Sqrt(tiles.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(tiles.Length); j++)
                {
                    if (tiles[arraySlot] == 1)
                    {
                        sb.Draw(square, gridSlots[arraySlot], Color.Red);
                    }
                    else if (tiles[arraySlot] == 2)
                    {
                        sb.Draw(square, gridSlots[arraySlot], Color.Blue);
                    }
                    arraySlot++;
                }
            }
        }

        public override void Reset()
        {
            goalSlot = RandomNine();
            tiles = Setup(goalSlot);
            win = false;
        }

        private int RandomNine()
        {
            Random random = new Random();
            return random.Next(9);
        }
    }
}

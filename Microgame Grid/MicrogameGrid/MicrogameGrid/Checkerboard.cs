using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgame_Grid
{
    internal class Checkerboard : Microgame
    {
        private int[] tiles;
        private Texture2D square;

        private Rectangle? clickedRect;
        private bool win;

        private int alternating;

        public override Rectangle? ClickedRect
        {
            get { return clickedRect; }
        }

        public override bool Win
        {
            get { return win; }
        }

        public Checkerboard(Texture2D square)
        {
            this.square = square;
            alternating = Game1.CoinFlip();
            tiles = Setup();
        }

        private int[] Setup()
        {
            Random random = new();
            int skip = random.Next(9);
            int[] result = new int[9];
            for (int i = 0; i < result.Length; i++)
            {
                if (alternating == 0)
                {
                    if (i != skip)
                    {
                        result[i] = 0;
                    }
                    alternating++;
                }
                else
                {
                    if (i != skip)
                    {
                        result[i] = 1;
                    }
                    alternating--;
                }
            }
            return result;
        }

        public void Draw(SpriteBatch sb, Rectangle[] gridSlots)
        {
            int arraySlot = 0;
            for (int i = 0; i < Math.Sqrt(tiles.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(tiles.Length); j++)
                {
                    if (tiles[arraySlot] == 1)
                    {
                        sb.Draw(square, gridSlots[arraySlot], Color.White);
                    }
                    else if (tiles[arraySlot] == 0)
                    {
                        sb.Draw(square, gridSlots[arraySlot], Color.Black);
                    }
                }
            }
        }
    }
}

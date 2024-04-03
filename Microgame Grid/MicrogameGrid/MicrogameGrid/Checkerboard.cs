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
    internal class Checkerboard : Microgame
    {
        private int[] tiles;
        private Texture2D square;

        private Rectangle? clickedRect;
        private bool win;

        private int alternating;
        private int playerTile;
        private int skip;

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
            skip = RandomNine();
            tiles = Setup(skip);
        }
        private int[] Setup(int skip)
        {
            Random random = new();
            int[] result = new int[9];
            for (int i = 0; i < result.Length; i++)
            {
                if (alternating == 0)
                {
                    if (i != skip)
                    {
                        result[i] = 1;
                    }
                    alternating++;
                }
                else
                {
                    if (i != skip)
                    {
                        result[i] = 2;
                    }
                    alternating--;
                }
            }
            return result;
        }

        public override void Update(MouseState[] state, Rectangle[] gridSlots)
        {
            int skip = Array.IndexOf(tiles, 0);
            clickedRect = gridSlots[skip];
            if (RightClick(state, (Rectangle)clickedRect))
            {
                playerTile = 2;
            }
            else if (LeftClick(state, (Rectangle)clickedRect))
            {
                playerTile = 1;
            }

            win = CheckWin();

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
                        sb.Draw(square, gridSlots[arraySlot], Color.White);
                    }
                    else if (tiles[arraySlot] == 2)
                    {
                        sb.Draw(square, gridSlots[arraySlot], Color.Black);
                    }
                    arraySlot++;
                }
            }

            if (clickedRect != null)
            {
                if (playerTile == 1)
                {
                    sb.Draw(square, (Rectangle)clickedRect, Color.White);
                }
                else if (playerTile == 2) 
                {
                    sb.Draw(square, (Rectangle)clickedRect, Color.Black);
                }
            }
        }

        private bool CheckWin()
        {
            if (tiles[0] == 1)
            {
                return (skip % 2 == 0 && playerTile == 1) || 
                    (skip % 2 == 1 && playerTile == 2);
            }
            else if (tiles[0] == 2)
            {
                return (skip % 2 == 0 && playerTile == 2) ||
                    (skip % 2 == 1 && playerTile == 1);
            }
            else if (tiles[0] == 0)
            {
                if (tiles[1] == 2)
                {
                    return (skip % 2 == 0 && playerTile == 1) ||
                        (skip % 2 == 1 && playerTile == 2);
                }
                else if (tiles[1] == 1)
                {
                    return (skip % 2 == 0 && playerTile == 2) ||
                        (skip % 2 == 1 && playerTile == 1);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            alternating = Game1.CoinFlip();
            skip = RandomNine();
            tiles = Setup(skip);
            playerTile = 0;
            win = false;
        }

        private int RandomNine()
        {
            Random random = new Random();
            return random.Next(9);
        }
    }
}

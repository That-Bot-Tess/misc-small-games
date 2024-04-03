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
    enum Direction
    {
        Left = 3,
        Right = 5,
        Up = 1,
        Down = 7
    }

    internal class Arrow : Microgame
    {
        private int[] tiles;
        private Texture2D arrow;

        private Rectangle? clickedRect;
        private bool win;
        private int goalSlot;
        private Direction[] incorrects;
        private Vector2 center;
        private float angle;

        public override Rectangle? ClickedRect
        {
            get { return clickedRect; }
        }

        public override bool Win
        {
            get { return win; }
        }

        public Arrow(Texture2D arrow)
        {
            this.arrow = arrow;
            goalSlot = RandomOrtho();
            tiles = Setup(goalSlot);
            incorrects = new Direction[3];
            int incorrectSlot = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == 0)
                {
                    incorrects[incorrectSlot] = RandomDir((Direction)i);
                    incorrectSlot++;
                }
            }
            center = new(arrow.Width / 2, arrow.Height / 2);
        }
        private int[] Setup(int goalSlot)
        {
            int[] result = new int[9];
            for (int i = 0; i < result.Length; i++)
            {
                if (i % 2 == 0)
                {
                    result[i] = -1;
                }
                else if (i != goalSlot)
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = 1;
                }
            }
            angle = 0;
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
            int incorrectSlot = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                    if (tiles[i] >= 0)
                    {
                        if (tiles[i] == 1)
                        {
                            sb.Draw(arrow, new(gridSlots[i].X + arrow.Width / 2, gridSlots[i].Y + arrow.Height / 2, arrow.Width, arrow.Height), null, Color.Pink, DirectionParse((Direction)i), center, SpriteEffects.None, 0);
                        }
                        else
                        {
                            sb.Draw(arrow, new(gridSlots[i].X + arrow.Width / 2, gridSlots[i].Y + arrow.Height / 2, arrow.Width, arrow.Height), null, Color.Pink, DirectionParse(incorrects[incorrectSlot]), center, SpriteEffects.None, 0); 
                            incorrectSlot++;
                        }
                    }
            }
        }

        public override void Reset()
        {
            goalSlot = RandomOrtho();
            tiles = Setup(goalSlot);
            int incorrectSlot = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == 0)
                {
                    incorrects[incorrectSlot] = RandomDir((Direction)i);
                    incorrectSlot++;
                }
            }
            win = false;
        }

        private int RandomOrtho()
        {
            Random random = new Random();
            return random.Next(3) * 2 + 1;
        }

        private Direction RandomDir(Direction exception)
        {
            Random random = new Random();
            switch (random.Next(4))
            {
                case 0:
                    if (exception != Direction.Up)
                        return Direction.Up;
                    else
                        return RandomDir(Direction.Up);
                case 1:
                    if (exception != Direction.Left)
                        return Direction.Left;
                    else
                        return RandomDir(Direction.Left);
                case 2:
                    if (exception != Direction.Right)
                        return Direction.Right;
                    else
                        return RandomDir(Direction.Right);
                case 3:
                    if (exception != Direction.Down)
                        return Direction.Down;
                    else
                        return RandomDir(Direction.Down);
            }
            return Direction.Up;
        }

        private float DirectionParse(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return 0;
                case Direction.Right:
                    return (float)Math.PI / 2;
                case Direction.Down:
                    return (float)Math.PI;
                case Direction.Left:
                    return (float)Math.PI * 3 / 2;
            }
            return 0;
        }
    }
}

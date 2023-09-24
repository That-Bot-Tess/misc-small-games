using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgame_Grid
{
    internal abstract class Microgame
    {
        public abstract Rectangle? ClickedRect { get; }
        public abstract bool Win { get; }

        protected virtual bool LeftClick(MouseState[] state, Rectangle position)
        {
            Vector2 mousePos = state[1].Position.ToVector2();
            return position.Contains(mousePos) && state[1].LeftButton == ButtonState.Pressed && state[0].LeftButton == ButtonState.Released;
        }

        protected virtual bool RightClick(MouseState[] state, Rectangle position)
        {
            Vector2 mousePos = state[1].Position.ToVector2();
            return position.Contains(mousePos) && state[1].RightButton == ButtonState.Pressed && state[0].RightButton == ButtonState.Released;
        }
    }
}

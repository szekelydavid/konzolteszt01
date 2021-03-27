using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SadConsole;
using Console = SadConsole.Console;

namespace SadC01
{
    public class GridConsole : ControlsConsole
    {
        
        public GridConsole() : base(100, 40)
        {
            DefaultBackground = Color.Transparent;
        }
    }
}
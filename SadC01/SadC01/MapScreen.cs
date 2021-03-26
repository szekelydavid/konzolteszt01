using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using Console = SadConsole.Console;
namespace RogueTutorial
{
        
       public class MapScreen : Console
    {
        public SadConsole.ScrollingConsole mapConsole;
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public MapScreen() : base(100, 40)
        {
            var mapConsole = new ControlsConsole(20,20);
            mapConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0);
            mapConsole.Position = new Point(2, 2);

            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            
            Parent = SadConsole.Global.CurrentScreen;
            mapConsole.SetGlyph(5, 5, 4);
            mapConsole.SetGlyph(7, 7, 9);

        }
   
    }
}
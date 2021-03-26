using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using Console = SadConsole.Console;
namespace RogueTutorial
{
        
       public class MapScreen : Console
    {
        private readonly SadConsole.Timer progressTimer;
        public SadConsole.ScrollingConsole mapConsole;
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public  int timeSum { get; set; }
        public MapScreen() : base(100, 40)
        {
            timeSum = 0;
            var mapConsole = new ControlsConsole(80,38);
            //mapConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0);
            mapConsole.Position = new Point(0, 3);

            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            
            Parent = SadConsole.Global.CurrentScreen;
            progressTimer = new Timer(TimeSpan.FromSeconds(1));
            
            progressTimer.TimerElapsed += (timer, e) => { timeSum++; };
            Components.Add(progressTimer);
            //mapConsole.SetGlyph(5, 5, 4);
            //mapConsole.SetGlyph(7, 7, 9);

        }
   
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SadC01;
using SadConsole;
using Console = SadConsole.Console;


namespace RogueTutorial
{
        
       public class MapScreen : Console
    {
      

        private readonly SadConsole.Timer progressTimer;
        public SadConsole.ScrollingConsole mapConsole;
        public SadConsole.ScrollingConsole gridConsole;
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public  int timeSum { get; set; }
        public  int animtimer { get; set; }
        public char[,] gamegrid { get; set; }
        public MapScreen() : base(100, 40)
        {
            
            timeSum = 0;
            mapConsole = new ControlsConsole(80,38);
            //mapConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0);
            mapConsole.Position = new Point(0, 3);
            
            
            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            Parent = SadConsole.Global.CurrentScreen;
           
            
            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);
            gridConsole = new ScrollingConsole(10, 10,normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                //DefaultBackground = Color.Purple,
                DefaultForeground = Color.White,
            };
            
            
            mapConsole.Children.Add(gridConsole);
            
            //this.gridConsole.SetGlyph(2, 2, 'x',Color.Yellow);
            
            
            
            progressTimer = new Timer(TimeSpan.FromSeconds(0.5));
            
            progressTimer.TimerElapsed += (timer, e) =>
            { 
                timeSum++;
                animtimer = (animtimer % 80);
                animtimer ++;

            };
            Components.Add(progressTimer);
            //mapConsole.SetGlyph(5, 5, 4);
            //mapConsole.SetGlyph(7, 7, 9);
            
            gamegrid = new char[10,9];
            initTheGrid();

        }

        public void animateStars(int animtimer)
        {
            for (int y = 0; y < 39; y++)
            {
                for (int x = 0; x < 80; x++)
                {
                    int color = (int)
                    (
                        animtimer%128.0 + ((animtimer+(128.0)) * Math.Sin(x / 8.0))
                                        + animtimer+128.0 + (animtimer +(128.0 * Math.Sin(y / 8.0)))
                    ) / 4;
                    
                    Color myRgbColor = new Color(color, color+((animtimer*4)%200), color+((animtimer*4)%250));
                    this.mapConsole.SetGlyph(x, y, 'x');
                    this.mapConsole.SetForeground(x,y,myRgbColor);
                    this.mapConsole.SetBackground(x,y,Color.Black);
                    
                }
            }
        }

        public void initTheGrid()
        {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 9; j++)
                {
                    this.gamegrid[i, j] = '0';
                }
            }
        }

    }
}
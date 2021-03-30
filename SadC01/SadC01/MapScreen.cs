using System;
using System.Collections.Generic;
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
      

        public readonly SadConsole.Timer progressTimer;
        public SadConsole.ScrollingConsole mapConsole;
        public SadConsole.ScrollingConsole gridConsole;
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public  int timeSum { get; set; }
        public  int animtimer { get; set; }
        public char[,] gamegrid { get; set; }

        public List<Monster> monsterList { get; set; }
        
        public MapScreen() : base(100, 40)
        {
            
            timeSum = 0;
            mapConsole = new ControlsConsole(80,38);
            
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
                DefaultForeground = Color.White,
            };
            
            
            mapConsole.Children.Add(gridConsole);
            
            progressTimer = new Timer(TimeSpan.FromSeconds(0.5));
            
            progressTimer.TimerElapsed += (timer, e) =>
            { 
                timeSum++;
                if (timeSum % 2 == 0)
                {
                    updateTheGrid();
                    foreach (Monster m in monsterList)
                    {
                        byte beB = (byte)(timeSum % 4);
                        m.phaseChange(beB);
                    }
                }

                animtimer = (animtimer % 80);
                animtimer ++;

            };
            Components.Add(progressTimer);
            //gridConsole.SetGlyph(5, 5, 'F');
     
            monsterList = new List<Monster>();
            
            gamegrid = new char[10,10];
            
            initTheGrid();
            
        }

        public void spawnMTest()
        {
            var m = new Mosquito(9,3)
            {
            };
            gamegrid[9,3] = 'F';
            monsterList.Add(m);
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
                    this.mapConsole.SetBackground(x,y,Color.Transparent);
                    
                }
            }
        }

        public void initTheGrid()
        {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++)
                {
                    this.gamegrid[i, j] = '0';
                }
                
            }
        }

        public void updateTheGrid()
        {
            
                foreach (Monster m in monsterList)
                {
                    gamegrid = m.moveOneStep(gamegrid);
                }
            
        }

        public void renderTheGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (gamegrid[i, j] == '0')
                    {
                        this.gridConsole.SetGlyph(i, j, '0');
                        this.gridConsole.SetForeground(i,j,Color.Transparent);
                        this.gridConsole.SetBackground(i,j,Color.Transparent);
                    }
                    else
                    {
                        char actualGlyph = gamegrid[i, j];
                        this.gridConsole.SetGlyph(i, j, actualGlyph);
                        this.gridConsole.SetForeground(i,j,Color.White);
                        this.gridConsole.SetBackground(i,j,Color.Transparent);
                    }
                }
            }
/*
            this.gridConsole.SetGlyph(2, 2, 'K');
            this.gridConsole.SetForeground(2,2,Color.White);
            this.gridConsole.SetBackground(2,2,Color.Transparent);
*/
            
        }

    }
}
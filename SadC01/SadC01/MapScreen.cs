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
        public SadConsole.ScrollingConsole statsConsole;
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public  int timeSum { get; set; }
        public  int animtimer { get; set; }
        public char[,] gamegrid { get; set; }

        public List <Monster> monsterList { get; set; }
        public List <Bonus> bonusList { get; set; }

        public int playerLifeCount;
        public int playerEnergyCount;
        
        public MapScreen() : base(100, 40)
        {
            
            timeSum = 0;
            mapConsole = new ControlsConsole(80,38)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            
            mapConsole.Position = new Point(0, 3);
            
            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            Parent = SadConsole.Global.CurrentScreen;
           
            
            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);
            
            gridConsole = new ScrollingConsole(10, 10,normalSizedFontPL)
            {
                DefaultBackground = Color.DarkBlue,
                DefaultForeground = Color.White,
            };
            statsConsole = new ScrollingConsole(3, 10,normalSizedFontPL)
            {
                DefaultBackground = Color.DodgerBlue,
                DefaultForeground = Color.White,
            };
            statsConsole.Position=new Point(10,3);
            mapConsole.Children.Add(gridConsole);
            mapConsole.Children.Add(statsConsole);
            
            progressTimer = new Timer(TimeSpan.FromSeconds(0.5));
            
            progressTimer.TimerElapsed += (timer, e) =>
            { 
                timeSum++;
                
                foreach (Bonus b in bonusList)
                {
                    byte beB = (byte)(timeSum % 4);
                    b.phaseChange(beB,gamegrid);
                }
                
                if (timeSum % 2 == 0)
                {
                    updateTheGrid();
                    foreach (Monster m in monsterList)
                    {
                        byte beB = (byte)(timeSum % 4);
                        m.phaseChange(beB);
                    }
                }

                updateTheStats();
                animtimer = (animtimer % 80);
                animtimer ++;

            };
            Components.Add(progressTimer);
            //gridConsole.SetGlyph(5, 5, 'F');
     
            monsterList = new List<Monster>();
            bonusList = new List<Bonus>();
            
            gamegrid = new char[10,10];
            initTheGrid();
            playerLifeCount = 3;
            playerEnergyCount = 9;

        }
        
        public void spawnMTest()
        {
            //var m = new SmallSaucer(7, 3);
            //var m = new BigSaucer(2, 2);
            //gamegrid[2,2] = 'M';
            //var m = new Blobtopus(5, 5);
            
            //gamegrid[7,3] = 'L';
            /*
            var m = new Mosquito(9,3)
            {
            };
            gamegrid[9,3] = 'F';
            gamegrid[7,2] = '3';
            */
            //monsterList.Add(m);
            
            
            
            var b = new BonusLife(2,2);
            bonusList.Add(b);
            
            gamegrid[2,2] = 'X';
            
            var bketto = new BonusEnergy(3,3);
            bonusList.Add(bketto);
            
            gamegrid[3,3] = 'Z';
            
            //
        }

        /*
        public void animateStars(int animtimer)
        {
            for (int y = 0; y < 39; y++)
            {
                for (int x = 0; x < 80; x++)
                {
                    int color = (int)
                    (
                        animtimer%128 + ((animtimer+(128.0)) * Math.Sin(x / 8.0))
                                        + animtimer+128.0 + (animtimer +(128.0 * Math.Sin(y / 8.0)))
                    ) / 4;
                    
                    Color myRgbColor = new Color(color*2%120, color+((animtimer*4)%120), color+((animtimer*4)%1200));
                    this.mapConsole.SetGlyph(x, y, 'x');
                    this.mapConsole.SetForeground(x,y,myRgbColor);
                    this.mapConsole.SetBackground(x,y,myRgbColor);
                    
                }
            }
        }
        */

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
        public void updateTheStats();

    }
}
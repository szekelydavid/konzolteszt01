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
        public SadConsole.ScrollingConsole playerLaserConsole;
        //public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public  int timeSum { get; set; }
        public  int animtimer { get; set; }
        //rács, amin a szörnyek vannak
        public char[,] gamegrid { get; set; }
        public int[,] playerLaserGrid { get; set; }
        // szörnyek
        public List <Monster> monsterList { get; set; }
        // bónuszok
        public List <Bonus> bonusList { get; set; }
        //játékos lövései
        public List <PlayerLaser> playerLaserList { get; set; }
        
        //HP
        public int playerLifeCount;
        //ENERGIA
        public int playerEnergyCount;
        
        public MapScreen() : base(100, 40)
        {
            //Számláló nullázása
            timeSum = 0;
            
            mapConsole = new ControlsConsole(80,3)
            {
                DefaultBackground = Color.Pink,
                DefaultForeground = Color.White,
            };
            
            mapConsole.Position = new Point(0, 3);
            
            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            Parent = SadConsole.Global.CurrentScreen;
           
            
            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);
            
            //a szörnyeket megjelenítő és kezelelő konzol inicializálása
            gridConsole = new ScrollingConsole(10, 9,normalSizedFontPL)
            {
                DefaultBackground = Color.DarkBlue,
                DefaultForeground = Color.White,
            };
            mapConsole.Children.Add(gridConsole);
            
            //játékos lézerlövéseit megjelenítő konzol
            playerLaserConsole = new ScrollingConsole(10, 9,normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            mapConsole.Children.Add(playerLaserConsole);

            //játékos statisztikákat megjelenítő konzol incializálása
            var normalSizedFontST = fontMasterPL.GetFont(Font.FontSizes.Two);
            statsConsole = new ScrollingConsole(6, 20,normalSizedFontST)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            statsConsole.Position=new Point(20,3);
            
            mapConsole.Children.Add(statsConsole);
            
            //idó-számláló kezelése
            progressTimer = new Timer(TimeSpan.FromSeconds(0.5));
            progressTimer.TimerElapsed += (timer, e) =>
            { 
                timeSum++;
                updateTheGridLasers(); 
                //BÓnuszok animációja
                foreach (Bonus b in bonusList)
                {
                    byte beB = (byte)(timeSum % 4);
                    b.phaseChange(beB,gamegrid);
                }
                
                if (timeSum % 2 == 0)
                {
                    //szörnyek mozgása
                    updateTheGridMonsters();
                    foreach (Monster m in monsterList)
                    {
                        byte beB = (byte)(timeSum % 4);
                        m.phaseChange(beB);
                    }
                }
                //statisztikák aktualizálása
                updateTheStats();
                //effekt animáció időzítője
                animtimer = (animtimer % 80);
                animtimer ++;

            };
            Components.Add(progressTimer);
            //gridConsole.SetGlyph(5, 5, 'F');
     
            monsterList = new List<Monster>();
            bonusList = new List<Bonus>();
            playerLaserList = new List<PlayerLaser>();

            gamegrid = new char[10,9];
            playerLaserGrid = new int[10,9];

            initTheGrids();

            //PlayerLaser laseregy = new PlayerLaser(5,5,0);
            //playerLaserList.Add(laseregy);

            playerLifeCount = 3;
            playerEnergyCount = 9;

            playerLaserGrid[2, 3] = 1;

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
        
        public void initTheGrids()
        {
            //monsterGrid
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 9; j++)
                {
                    this.gamegrid[i, j] = '0';
                }
            }
            //playerLaserGrid
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 9; j++)
                {
                    this.playerLaserGrid[i, j] = 9;
                }
            }
        }

        public void updateTheGridMonsters()
        {
            
                foreach (Monster m in monsterList)
                {
                    gamegrid = m.moveOneStep(gamegrid);
                }
              
            
        }
        public void updateTheGridLasers()
        {

                foreach (PlayerLaser pll in playerLaserList)
                {
                    playerLaserGrid= pll.moveOneStep(playerLaserGrid);
                }
            //System.Console.WriteLine("UPDATE");

        }
        

        public void renderTheGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 9; j++)
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
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 9; j++)
                {
                    if (this.playerLaserGrid[i, j] == 9)
                    {
                        this.playerLaserConsole.SetGlyph(i, j, '3');
                        this.playerLaserConsole.SetForeground(i, j, Color.White);
                        this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                    }
                    if (this.playerLaserGrid[i, j] != 9)
                    {
                        this.playerLaserConsole.SetGlyph(i, j, '6');
                        this.playerLaserConsole.SetForeground(i,j,Color.White);
                        this.playerLaserConsole.SetBackground(i,j,Color.Transparent); 
                    }
                    
                }
            }
            
        }

         public void updateTheStats()
        {
            this.statsConsole.SetGlyph(0, 0, 'd');
            this.statsConsole.SetGlyph(1, 0, '4');
            this.statsConsole.SetGlyph(2, 0, '4');
            this.statsConsole.SetGlyph(3, 0, '4');
            this.statsConsole.SetGlyph(4, 0, '4');
            this.statsConsole.SetForeground(0,0,Color.White);
            this.statsConsole.SetBackground(0,0,Color.Transparent);
            
            this.statsConsole.SetGlyph(0, 2, '2');
            this.statsConsole.SetGlyph(1, 2, '>');
            this.statsConsole.SetGlyph(2, 2, '>');
            this.statsConsole.SetGlyph(3, 2, '>');
            this.statsConsole.SetGlyph(4, 2, '>');
            
        }

    }
}
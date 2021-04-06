using System;
using System.Collections.Generic;
using System.Linq;
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
        public int timeSum { get; set; }
        public int animtimer { get; set; }
        //rács, amin a szörnyek vannak
        public char[,] gamegrid { get; set; }
        public int[,] playerLaserGrid { get; set; }
        // szörnyek
        public List<Monster> monsterList { get; set; }
        // bónuszok
        public List<Bonus> bonusList { get; set; }
        //játékos lövései
        //public List<PlayerLaser> playerLaserList { get; set; }

        //HP
        public int playerLifeCount { get; set; }
        //ENERGIA
        public int playerEnergyCount { get; set; }

        public MapScreen() : base(100, 40)
        {
            //! Számláló nullázása
            timeSum = 0;

            mapConsole = new ScrollingConsole(100, 40)
            {
                DefaultBackground = Color.Black,
                DefaultForeground = Color.White,
            };

            mapConsole.Position = new Point(0, 0);

            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            Parent = SadConsole.Global.CurrentScreen;


            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);

            //! a szörnyeket megjelenítő és kezelelő konzol inicializálása
            gridConsole = new ScrollingConsole(10, 9, normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            mapConsole.Children.Add(gridConsole);

            //! játékos lézerlövéseit megjelenítő konzol
            playerLaserConsole = new ScrollingConsole(10, 9, normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            mapConsole.Children.Add(playerLaserConsole);

            //! játékos statisztikákat megjelenítő konzol incializálása
            var normalSizedFontST = fontMasterPL.GetFont(Font.FontSizes.Two);
            statsConsole = new ScrollingConsole(6, 20, normalSizedFontST)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            statsConsole.Position = new Point(19, 3);

            mapConsole.Children.Add(statsConsole);


            //! _____ISMÉTLŐDŐ CIKLUS___

            //! idő-számláló kezelése
            //! _____UPDATEK_______

            progressTimer = new Timer(TimeSpan.FromSeconds(0.25));
            progressTimer.TimerElapsed += (timer, e) =>
            {
                timeSum++;
                updateTheGridLasers();
                //BÓnuszok animációja
                foreach (Bonus b in bonusList)
                {
                    byte beB = (byte)(timeSum % 4);
                    b.phaseChange(beB, gamegrid);
                }
                if (timeSum % 4 == 0)
                {
                    updateTheGridExplosions();
                }
                if (timeSum % 4 == 0)
                {
                    //! szörnyek mozgása
                    updateTheGridMonsters();
                    foreach (Monster m in monsterList)
                    {
                        byte beB = (byte)(timeSum % 8);
                        m.phaseChange(beB);
                    }
                }
                
                if (timeSum % 4 == 0)
                {
                    /*
                    for (int j = 0; j < 9; j++)
                    {
                        for (int i = 0; i < 10; i++)
                        { System.Console.Write(gamegrid[i, j] + " "); }
                        System.Console.Write("\n");
                    }
                    System.Console.Write("\n");
                    */
                }

                //! statisztikák aktualizálása
                updateTheStats();
                //! effekt animáció időzítője
                animtimer = (animtimer % 80);
                animtimer++;

            };
            Components.Add(progressTimer);
            //gridConsole.SetGlyph(5, 5, 'F');


            //! ___LISTÁK INICIALIZÁLÁSA___
            monsterList = new List<Monster>();
            bonusList = new List<Bonus>();
            //playerLaserList = new List<PlayerLaser>();

            gamegrid = new char[10, 9];
            playerLaserGrid = new int[10, 10];

            initTheGrids();

            //! ________________________DATA
            //! Player adatok inicaializálása
            playerLifeCount = 4;
            playerEnergyCount = 9;

            // ! TESZT
            playerLaserGrid[4, 4] = 3;


        }

        //! ______MONSTER SPAWNOK___
        public void spawnBlobptosaurus()
        {
            Random rnd = new Random();
            int blX = rnd.Next(1, 9);
            int blY = rnd.Next(1, 8);
            var m = new Blobtopus(blX, blY);
            gamegrid[blX, blY] = 'J';
            monsterList.Add(m);
        }

        public void spawnMosquito()
        {
            Random rnd = new Random();
            int mosqX = rnd.Next(0, 10);
            int mosqY = rnd.Next(0, 3);
            var m = new Mosquito(mosqX, mosqY);
            gamegrid[mosqX, mosqY] = 'F';
            monsterList.Add(m);
        }

        public void spawnSmallSaucer()
        {
            Random rnd = new Random();
            int smallSX;
            int smallSY;
            int fennORlenn = rnd.Next(0, 2);
            smallSX = rnd.Next(0, 10);
            if (fennORlenn == 0)
            {
                smallSY = rnd.Next(0, 3);
            }
            else
            {
                smallSY = rnd.Next(7, 9);
            }
            var m = new SmallSaucer(smallSX, smallSY);
            gamegrid[smallSX, smallSY] = 'L';
            monsterList.Add(m);
        }

        public void spawnBigSaucer()
        {
            Random rnd = new Random();
            int BigSX;
            int BigSY;
            int fennORlenn = rnd.Next(0, 2);
            BigSX = rnd.Next(0, 10);
            if (fennORlenn == 0)
            {
                BigSY = rnd.Next(0, 3);
            }
            else
            {
                BigSY = rnd.Next(7, 9);
            }
            var m = new BigSaucer(BigSX, BigSY);
            gamegrid[BigSX, BigSY] = 'M';
            monsterList.Add(m);
        }
        //! _________________________________



        public void spawnMTest()
        {

            spawnBlobptosaurus();
            spawnMosquito();
            spawnSmallSaucer();
            spawnBigSaucer();
            //var m = new SmallSaucer(7, 3);
            //var m = new BigSaucer(2, 2);
            //gamegrid[2,2] = 'M';


            //var m = new BigSaucer(8, 7);


            //gamegrid[8,7] = 'M';
            //gamegrid[7,2] = '3';
            //*/

            //monsterList.Remove(m);

            /*
            var b = new BonusLife(2, 2);
            bonusList.Add(b);

            gamegrid[2, 2] = 'X';

            var bketto = new BonusEnergy(3, 3);
            bonusList.Add(bketto);

            gamegrid[3, 3] = 'Z';
            */
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
            //! monsterGrid
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.gamegrid[i, j] = '0';
                }
            }
            //! playerLaserGrid
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.playerLaserGrid[i, j] = 9;
                }
            }
        }
        public void updateTheGridExplosions()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    if (gamegrid[i, j] == 'G')
                    {
                        this.gridConsole.SetGlyph(i, j, 'W');
                        this.gridConsole.SetForeground(i, j, Color.White);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                        gamegrid[i, j] = 'W';
                    }
                    else if (gamegrid[i, j] == 'W')
                    {
                        this.gridConsole.SetGlyph(i, j, '0');
                        this.gridConsole.SetForeground(i, j, Color.Transparent);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                        gamegrid[i, j] = '0';
                    }


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

        public bool isItHit(int x, int y)
        {
            string enemiesString = "GFVWJKL\\M]N";

            char targetChar = gamegrid[x, y];
            if (enemiesString.Contains(targetChar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void removeMonster(int bex, int bey)
        {
            for (int mnstcount = monsterList.Count - 1; mnstcount >= 0; mnstcount--)
            {
                /*
                System.Console.WriteLine(monsterList[mnstcount]);
                System.Console.WriteLine("MX " + monsterList[mnstcount].monsterX);
                System.Console.WriteLine("MY" + monsterList[mnstcount].monsterY);
                */
                if ((monsterList[mnstcount].monsterX == bex) && ((monsterList[mnstcount].monsterY == bey)))
                {
                    System.Console.WriteLine("remove");
                    monsterList.RemoveAt(mnstcount);
                    gamegrid[bex, bey] = 'G';
                }
            }
        }

        //! lézerek mozgatása, ellenőrzés, hogy találnak-e
        public void updateTheGridLasers()
        {
            //playerLaserGrid[6, 4] = 1;
            //System.Console.WriteLine( "cycle");

            int[,] returngrid = new int[10, 9];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    returngrid[i, j] = 9;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {


                    if (playerLaserGrid[i, j] == 0)
                    {

                        if ((j - 1) >= 0)
                        {

                            if (isItHit(i, j - 1))
                            {
                                System.Console.WriteLine("TALALT");

                                removeMonster(i, j - 1);
                                returngrid[i, j - 1] = 9;
                            }
                            else
                            {
                                returngrid[i, j - 1] = 0;
                            }
                        }

                    }

                    if (playerLaserGrid[i, j] == 1)
                    {
                        //System.Console.WriteLine("f1");
                        if ((i + 1) < 10)
                        {
                            if (isItHit(i + 1, j))
                            {
                                System.Console.WriteLine("TALALT");
                                removeMonster(i + 1, j);
                                returngrid[i + 1, j] = 9;
                            }
                            else
                            {
                                returngrid[i + 1, j] = 1;
                            }
                        }

                    }

                    if (playerLaserGrid[i, j] == 2)
                    {
                        System.Console.WriteLine(j);
                        //System.Console.WriteLine("f2");
                        if ((j + 1) < 9)
                        {
                            if (isItHit(i, j + 1))
                            {
                                System.Console.WriteLine("TALALT");

                                removeMonster(i, j + 1);
                                returngrid[i, j + 1] = 9;
                            }
                            else
                            {
                                returngrid[i, j + 1] = 2;
                            }
                        }

                    }

                    if (playerLaserGrid[i, j] == 3)
                    {
                        //System.Console.WriteLine("f3");
                        if ((i - 1) >= 0)
                        {
                            if (isItHit(i - 1, j))
                            {
                                System.Console.WriteLine("TALALT");

                                removeMonster(i - 1, j);
                                returngrid[i - 1, j] = 0;
                            }
                            else
                            {
                                returngrid[i - 1, j] = 3;
                            }
                        }

                    }

                    //System.Console.WriteLine("UPDATE");

                }
            }
            //! átírja a rácsot
            playerLaserGrid = returngrid;
        }


        public void renderTheGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamegrid[i, j] == '?')
                    {

                        removeMonster(i, j);
                        gamegrid[i, j] = '0';
                    }
                    if (gamegrid[i, j] == '0')
                    {

                        this.gridConsole.SetGlyph(i, j, '0');
                        this.gridConsole.SetForeground(i, j, Color.Transparent);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                    }

                    else
                    {
                        char actualGlyph = gamegrid[i, j];
                        this.gridConsole.SetGlyph(i, j, actualGlyph);
                        this.gridConsole.SetForeground(i, j, Color.White);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                    if (this.playerLaserGrid[i, j] == 9)
                    {
                        this.playerLaserConsole.SetGlyph(i, j, ':');
                        this.playerLaserConsole.SetForeground(i, j, Color.White);
                        this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                    }
                    if (this.playerLaserGrid[i, j] != 9)
                    {
                        this.playerLaserConsole.SetGlyph(i, j, '6');
                        this.playerLaserConsole.SetForeground(i, j, Color.White);
                        this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                    }
                }
            }
        }



        public void updateTheStats()
        {
            //statsConsole.Fill(new Rectangle(0, 0, 6, 20), Color.White, Color.Navy, 0, 0);
            if (playerLifeCount == 4)
            {
                this.statsConsole.SetGlyph(1, 0, 'd');
                this.statsConsole.SetGlyph(2, 0, '4');
                this.statsConsole.SetGlyph(3, 0, '4');
                this.statsConsole.SetGlyph(4, 0, '4');
                this.statsConsole.SetGlyph(5, 0, '4');
            }
            this.statsConsole.SetForeground(0, 0, Color.White);


            this.statsConsole.SetGlyph(1, 2, '2');
            this.statsConsole.SetGlyph(2, 2, '>');
            this.statsConsole.SetGlyph(3, 2, '>');
            this.statsConsole.SetGlyph(4, 2, '>');
            this.statsConsole.SetGlyph(5, 2, '>');

        }

    }
}

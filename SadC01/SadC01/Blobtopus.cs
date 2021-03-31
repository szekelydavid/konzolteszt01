using System;
using Microsoft.Xna.Framework.Media;
using RogueTutorial;
using System.Linq;


namespace RogueTutorial
{
    public class Blobtopus : Monster
    {
        public char actualGlyph = 'J';

        private char actualField;

        public int blobtopusX { get; set; }
        public int blobtopusY { get; set; }
        public int monsterX { get; set; }
        public int monsterY { get; set; }


        public Blobtopus(int bex, int bey)
        {
            this.monsterX = bex;
            this.monsterY = bey;
            blobtopusX = bex;
            blobtopusY = bey;
        }

        public override void phaseChange(byte inb)
        {
            if (inb == 2)
            {
                this.actualGlyph = 'K';
            }

            if (inb == 0)
            {
                this.actualGlyph = 'J';
            }
        }

        public override char[,] moveOneStep(char[,] grid)
        {
            Random rand = new Random();
            int irany = rand.Next(0, 4);

            //GameLoop._mapscreen.Print(1, 1, "F");

            int coorXmoveTo = 0;
            int coorYmoveTo = 0;

            if (irany == 0)
            {
                coorXmoveTo = blobtopusX;
                coorYmoveTo = blobtopusY - 1;
            }

            if (irany == 1)
            {
                coorXmoveTo = blobtopusX + 1;
                coorYmoveTo = blobtopusY;
            }

            if (irany == 2)
            {
                coorXmoveTo = blobtopusX;
                coorYmoveTo = blobtopusY + 1;
            }

            if (irany == 3)
            {
                coorXmoveTo = blobtopusX - 1;
                coorYmoveTo = blobtopusY;
            }

            //grid[8, 8] = actualField;
            string enableToMove = "zjZ0hX";
            if (enableToMove.Contains(grid[coorXmoveTo, coorYmoveTo]))
            {
                grid[blobtopusX, blobtopusY] = '0';

                grid[coorXmoveTo, coorYmoveTo] = actualGlyph;

                this.blobtopusX = coorXmoveTo;
                this.blobtopusY = coorYmoveTo;

                monsterX = blobtopusX;
                monsterY = blobtopusY;
            }

            return grid;
        }
    }
}
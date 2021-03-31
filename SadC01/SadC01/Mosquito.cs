using System;
using Microsoft.Xna.Framework.Media;
using RogueTutorial;

namespace RogueTutorial
{
    public class Mosquito : Monster
    {
        public char actualGlyph = 'V';
        private char actualField;
        public int mosquitoX { get; set; }
        public int mosquitoY { get; set; }
        public int monsterX { get; set; }
        public int monsterY { get; set; }
        
        private char[,] mozgasMinta = new char[9, 10]
        {
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'D'},
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'D'},
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'D'},
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'}
            
        };   
        
        public Mosquito (int bex,int bey) {
            this.monsterX = bex;
            this.monsterY = bey;
            mosquitoX = bex;
            mosquitoY = bey;
            
        }

        public override void phaseChange(byte inb)
        {
            if (inb == 2)
            {
                this.actualGlyph = 'F';
            }
            
            if (inb == 0)
            {
                this.actualGlyph = 'V';
            }
            
        }
        
        public override char[,]  moveOneStep  (char[,] grid)
        {
            
            actualField = mozgasMinta[this.mosquitoY,this.mosquitoX];
            //GameLoop._mapscreen.Print(1, 1, "F");
            
            int coorXmoveTo =0;
            int coorYmoveTo =0;
            
            if (actualField == 'E')
            {
                coorXmoveTo = mosquitoX;
                coorYmoveTo = mosquitoY - 1;
            }
            if (actualField == 'K')
            {
                coorXmoveTo = mosquitoX+1;
                coorYmoveTo = mosquitoY;
            }
            if (actualField == 'D')
            {
                coorXmoveTo = mosquitoX;
                coorYmoveTo = mosquitoY + 1;
            }
            if (actualField == 'N')
            {
                coorXmoveTo = mosquitoX-1;
                coorYmoveTo = mosquitoY;
            }
            //grid[8, 8] = actualField;
            //Console.WriteLine(coorXmoveTo.ToString()+" "+coorYmoveTo.ToString());
            grid[mosquitoX, mosquitoY] = '0';
            
            grid[coorXmoveTo, coorYmoveTo] = actualGlyph;
            
            this.mosquitoX = coorXmoveTo;
            this.mosquitoY = coorYmoveTo;
            
            monsterX = mosquitoX;
            monsterY = mosquitoY;
            
            return grid;
        }
        
    }
}
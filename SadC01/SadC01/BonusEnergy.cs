using SadC01;

namespace RogueTutorial
{
public class BonusEnergy : Bonus
{
    char actualGlyph='X';
    public int bonusenergyX;
    public int bonusenergyY;
    
    public BonusEnergy (int bex,int bey) {
        this.bonusenergyX = bex;
        this.bonusenergyY = bey;
                
    }
    public override void phaseChange(byte inb,char[,] grid)
    {
        if (inb == 0)
        {
            this.actualGlyph = 'Z';
        }
                    
        if (inb == 1)
        {
            this.actualGlyph = 'j';
        }
        if (inb == 2)
        {
            this.actualGlyph = 'z';
        }
        grid[bonusenergyX, bonusenergyY] = actualGlyph;            
    }
}
}
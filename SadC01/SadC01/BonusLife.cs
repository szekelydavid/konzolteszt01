namespace RogueTutorial
{
    public class BonusLife : Bonus
    {
    public int bonuslifeX;
    public int bonuslifeY;
    public char actualGlyph='X';
    public BonusLife (int bex,int bey) {
                this.bonuslifeX = bex;
                this.bonuslifeY = bey;
                
            }
        public override void phaseChange(byte inb,char[,] grid)
                {
                    if (inb ==1 || inb==3)
                    {
                        this.actualGlyph = 'h';
                    }
                    
                    if (inb==0 || inb==2 )
                    {
                        this.actualGlyph = 'X';
                    } 
                    grid[bonuslifeX, bonuslifeY] = actualGlyph;
                }
                
    }
}
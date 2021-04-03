namespace RogueTutorial
{
    public class PlayerLaser
    {
        public int Direction;
        public int playerlaserX;
        public int playerlaserY;
        
        
        public PlayerLaser (int bex,int bey, int direction) {

            //byte[,]playerlasergrid
            this.playerlaserX = bex;
            this.playerlaserY = bey;
            this.Direction = direction;
            if (Direction == 0)
            {
                playerlaserX = bex;
                playerlaserY = bey - 1;
            }
            if (Direction == 1)
            {
                playerlaserX = bex+1;
                playerlaserY = bey;
            }
            if (Direction == 2)
            {
                playerlaserX = bex;
                playerlaserY = bey + 1;
            }
            if (Direction == 3)
            {
                playerlaserX = bex-1;
                playerlaserY = bey;
            }
            
            
        }
        public int[,] placementToTheGrid(int[,] grid) {
            grid[playerlaserX, playerlaserY] = this.Direction;
            return grid;
        }
        
        public  int[,]  moveOneStep  (int[,] grid)
        {            
            
            
            //grid[playerlaserX, playerlaserY] = 0;
           

            if (Direction == '0')
            {
                playerlaserY--;
                grid[playerlaserX, playerlaserY] = 0;
            }

            if (Direction == '1')
            {
                playerlaserX++;
                grid[playerlaserX, playerlaserY] = 1;

            }

            if (Direction == '2')
            {
                playerlaserY ++;
                grid[playerlaserX, playerlaserY] = 2;
            }

            if (Direction == '3')
            {
                playerlaserX --;
                grid[playerlaserX, playerlaserY] = 3;
            }
            /*
            playerlaserX = coorXmoveTo;
            playerlaserY = coorYmoveTo;
            */
            //grid[5, 6] = 1;
            /*
            for (int j = 0; j < 10; j++)
            {
                for (int k=0; k < 10; k++)
                {
                    System.Console.Write(grid[k,j].ToString());
                }
                System.Console.WriteLine("");
            }
            System.Console.WriteLine(" ");
            */
            return grid;

           
        }

        /*
        if (Direction == '0')
        {
            coorXmoveTo = playerlaserX;
            coorYmoveTo = playerlaserY - 1;
        }
        if (Direction == '1')
        {
            coorXmoveTo = playerlaserX+1;
            coorYmoveTo = playerlaserY;
        }
        if (Direction == '2')
        {
            coorXmoveTo = playerlaserX;
            coorYmoveTo = playerlaserY + 1;
        }
        if (Direction == '3')
        {
            coorXmoveTo = playerlaserX-1;
            coorYmoveTo = playerlaserY;
        }
        //grid[8, 8] = actualField;
        //Console.WriteLine(coorXmoveTo.ToString()+" "+coorYmoveTo.ToString());
        grid[playerlaserX, playerlaserY] = 0;
        
  
        }*/
       
            
    }
}
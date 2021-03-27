using System;
using System.Linq;
using System.Collections.Generic;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Themes;

namespace RogueTutorial
{
    public class Monster 
    {
        public void moveOneStep()
        {
        }

        private char actualGlyph;
        public int X { get; set; }
        public int Y { get; set; }

        public int iranyMon { get; set; }
        //! észak: 0 , kelet: 1, dél:2 , nyugat 3
        
    }
}
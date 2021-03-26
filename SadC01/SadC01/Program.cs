using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueTutorial
{
    class GameLoop
    {

        public const int Width = 100;
        public const int Height = 40;

        public int fee;
        public static MapScreen _mapscreen;
        public static Console startingConsole;
        
  
        private static Player _player { get; set; }

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time)
        {
            CheckPlayerButton();
            PlayerAnim();
            
        }

        private static void CheckPlayerButton()
        {
            // Called each logic update.
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            // Decrement player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                _player.iranyPL = 0;
                _player.Position += new Point(0, -1);
            }

            // Keyboard movement for Player character: DOWN arrow
            // Increment player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                _player.iranyPL = 2;
                _player.Position += new Point(0, 1);
            }

            // Keyboard movement for Player character: Left arrow
            // Decrement player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                _player.iranyPL = 3;
                _player.Position += new Point(-1, 0);
            }

            // Keyboard movement for Player character: RIGHT arrow
            // Increment player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                _player.iranyPL = 1;
                _player.Position += new Point(1, 0);
                
                
            }
            _mapscreen.Print(1, 1, _player.iranyPL.ToString());
            _mapscreen.Print(5, 1, _mapscreen.timeSum.ToString());
        }

        public static void PlayerAnim()
        {
            if (_player.iranyPL == 1)
            {
                _player.pGlyph = 'D';
            }
            if (_player.iranyPL == 2)
            {
                _player.pGlyph = 'C';
            }
            if (_player.iranyPL == 3)
            {
                _player.pGlyph = 'E';
            }
            if (_player.iranyPL == 0)
            {
                _player.pGlyph = 'B';
            }
            
        }



        private static void Init()
        {
            
            var fontMaster = SadConsole.Global.LoadFont("Fonts/chess.font");
     
            var normalSizedFont = fontMaster.GetFont(Font.FontSizes.Four);

            startingConsole = new Console(Width, Height, normalSizedFont);
            startingConsole.SetGlyph(3, 3, 4);
            
            _mapscreen = new MapScreen();
            startingConsole.Children.Add(_mapscreen);

            
            // Set our new console as the thing to render and process
            SadConsole.Global.CurrentScreen = startingConsole;
            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);
            
            //váltoZÁS
            // create an instance of player
            _player = new Player();
            _player.Font = normalSizedFontPL;
            _player.Position = new Point(5, 8);
            _player.pGlyph = 'B';
            _player.Animation.CurrentFrame[0].Background = Color.TransparentBlack;
            _player.Animation.CurrentFrame[0].Foreground = Color.White;
            // add the player Entity to our only console
            // so it will display on screen
            startingConsole.Children.Add(_player);
        }
    
       
        
    }
}

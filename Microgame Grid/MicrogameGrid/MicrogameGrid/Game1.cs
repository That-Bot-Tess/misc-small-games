using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Microgame_Grid
{
    public enum MicroGame
    {
        TicTacToe,
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static Texture2D grid;
        private static Rectangle[] gridSlots;

        private MicroGame currentGame;

        private TicTacToe ticTacToe;
        private Checkerboard checkerboard;

        private MouseState[] mouseState;

        private SpriteFont UIfont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 810;
            _graphics.PreferredBackBufferWidth = 810;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            grid = Content.Load<Texture2D>("grid");
            
            gridSlots = SetGridSlots();
            ticTacToe = new(Content.Load<Texture2D>("gridCircle"), Content.Load<Texture2D>("gridCross"));
            checkerboard = new(Content.Load<Texture2D>("squareWhite"));

            mouseState = new MouseState[2];
            UIfont = Content.Load<SpriteFont>("UIfont");

            currentGame = MicroGame.TicTacToe;
            mouseState[0] = Mouse.GetState();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState[1] = Mouse.GetState();
            // TODO: Add your update logic here
            switch (currentGame)
            {
                case MicroGame.TicTacToe:
                    ticTacToe.Update(mouseState, gridSlots);
                    break;
            }
            mouseState[0] = mouseState[1];
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightCyan);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(grid, new Rectangle(100, 100, 610, 610), Color.White);
            switch (currentGame)
            {
                case MicroGame.TicTacToe:
                    ticTacToe.Draw(_spriteBatch, gridSlots, ticTacToe.ClickedRect);
                    _spriteBatch.DrawString(UIfont, ticTacToe.WinCount.ToString(), new(400, 25), Color.Black);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private static Rectangle[] SetGridSlots()
        {
            Rectangle[] result = new Rectangle[9];
            int arraySlot = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[arraySlot] = new Rectangle(100 + 205 * j, 100 + 205 * i, 205, 205);
                    arraySlot++;
                }
            }
            return result;
        }

        public static int CoinFlip()
        {
            Random random = new();
            return random.Next(2);
        }
    }
}
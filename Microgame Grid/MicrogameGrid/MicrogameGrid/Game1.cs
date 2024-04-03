using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Microgame_Grid
{
    public enum MicroGame
    {
        Title,
        TicTacToe,
        Checkerboard,
        BlueSquare,
        Arrow,
        GameEnd,
        Rules
    }

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int windowWidth;
        private int windowHeight;

        private static Texture2D grid;
        private SpriteFont UIfont;
        private SpriteFont titleFont;
        
        private static Rectangle[] gridSlots;

        private MicroGame currentGame;

        private TicTacToe ticTacToe;
        private Checkerboard checkerboard;
        private BlueSquare blueSquare;
        private Arrow arrow;

        private MouseState[] mouseState;
        private float timer;

        private static int winCount;
        private static int rulePage;

        private Texture2D square;

        public static int WinCount
        {
            get { return winCount; }
            set { winCount = value; }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            windowHeight = _graphics.PreferredBackBufferHeight = 810;
            windowWidth = _graphics.PreferredBackBufferWidth = 810;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            grid = Content.Load<Texture2D>("grid");

            square = Content.Load<Texture2D>("squareWhite");

            gridSlots = SetGridSlots();
            ticTacToe = new(Content.Load<Texture2D>("gridCircle"), Content.Load<Texture2D>("gridCross"));
            checkerboard = new(square);
            blueSquare = new(square);
            arrow = new(Content.Load<Texture2D>("arrow"));

            mouseState = new MouseState[2];
            UIfont = Content.Load<SpriteFont>("UIfont");
            titleFont = Content.Load<SpriteFont>("TitleFont");

            timer = 60.0f;

            currentGame = MicroGame.Title;
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
                case MicroGame.Title:
                    timer = 30;
                    if (ticTacToe.LeftClick(mouseState, gridSlots[6]))
                    {
                        rulePage = 0;
                        currentGame = MicroGame.Rules;
                    }
                    if (ticTacToe.LeftClick(mouseState, gridSlots[7]))
                        currentGame = RandomMicrogame();
                    if (ticTacToe.LeftClick(mouseState, gridSlots[8]))
                        Exit();
                    break;
                case MicroGame.Rules:
                    if (ticTacToe.LeftClick(mouseState, gridSlots[8]))
                        rulePage++;
                    if (rulePage == 5)
                    {
                        arrow.Reset();
                        currentGame = MicroGame.Title;
                    }
                    break;
                case MicroGame.TicTacToe:
                    ticTacToe.Update(mouseState, gridSlots);
                    if (ticTacToe.Win)
                    {
                        ticTacToe.Reset();
                        currentGame = RandomMicrogame();
                    }
                    goto default;
                case MicroGame.Checkerboard:
                    checkerboard.Update(mouseState, gridSlots);
                    if (checkerboard.Win)
                    {
                        checkerboard.Reset();
                        currentGame = RandomMicrogame();
                    }
                    goto default;
                case MicroGame.BlueSquare:
                    blueSquare.Update(mouseState, gridSlots);
                    if (blueSquare.Win)
                    {
                        blueSquare.Reset();
                        currentGame = RandomMicrogame();
                    }
                    goto default;
                case MicroGame.Arrow:
                    arrow.Update(mouseState, gridSlots);
                    if (arrow.Win)
                    {
                        arrow.Reset();
                        currentGame = RandomMicrogame();
                    }
                    goto default;
                case MicroGame.GameEnd:
                    if (ticTacToe.LeftClick(mouseState, gridSlots[8]))
                        currentGame = MicroGame.Title;
                    break;
                default:
                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer <= 0)
                        currentGame = MicroGame.GameEnd;
                    break;

            }
            mouseState[0] = mouseState[1];
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Cyan);
            Color currentColor;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(grid, new Rectangle(100, 100, 610, 610), Color.White);
            switch (currentGame)
            {
                case MicroGame.Title:
                    _spriteBatch.DrawString(titleFont, "MICRO", new(gridSlots[0].X + 30, gridSlots[0].Y + 70), Color.Black);
                    _spriteBatch.DrawString(titleFont, "GAME", new(gridSlots[1].X + 40, gridSlots[1].Y + 70), Color.Black);
                    _spriteBatch.DrawString(titleFont, "GRID", new(gridSlots[2].X + 40, gridSlots[2].Y + 70), Color.Black);
                    currentColor = gridSlots[6].Contains(mouseState[1].Position) ? Color.RoyalBlue : Color.Navy;
                    _spriteBatch.DrawString(titleFont, "RULES", new(gridSlots[6].X + 30, gridSlots[6].Y + 70), currentColor);

                    currentColor = gridSlots[7].Contains(mouseState[1].Position) ? Color.RoyalBlue : Color.Navy;
                    _spriteBatch.DrawString(titleFont, "START", new(gridSlots[7].X + 30, gridSlots[7].Y + 70), currentColor);

                    currentColor = gridSlots[8].Contains(mouseState[1].Position) ? Color.RoyalBlue : Color.Navy;
                    _spriteBatch.DrawString(titleFont, "EXIT", new(gridSlots[8].X + 30, gridSlots[8].Y + 70), currentColor);
                    break;
                case MicroGame.Rules:
                    string text = "NEXT";
                    currentColor = gridSlots[8].Contains(mouseState[1].Position) ? Color.RoyalBlue : Color.Navy;
                    switch (rulePage)
                    {
                        case 0:
                            _spriteBatch.DrawString(titleFont, "SOLVE\nPUZZLES", new(gridSlots[0].X + 10, gridSlots[0].Y + 70), Color.Black);
                            _spriteBatch.DrawString(titleFont, "ACT\nFAST", new(gridSlots[1].X + 50, gridSlots[1].Y + 70), Color.Black);
                            _spriteBatch.DrawString(titleFont, "ONE\nMINUTE", new(gridSlots[2].X + 20, gridSlots[2].Y + 70), Color.Black);
                            _spriteBatch.DrawString(UIfont, "Complete the short puzzles as fast as you can. \nObtain as many points as possible in one minute.", new(10, 720), Color.Black);
                            break;
                        case 1:
                            ticTacToe.Draw(_spriteBatch, gridSlots);
                            _spriteBatch.DrawString(titleFont, "Tic Tac Toe", new(windowWidth / 2 - titleFont.MeasureString("Tic Tac Toe").X / 2, 10), Color.Black);
                            _spriteBatch.DrawString(UIfont, "Create three X's in a row! \nClick on the right square to complete the row.", new(10, 720), Color.Black);
                            break;
                        case 2:
                            ticTacToe.Reset();
                            checkerboard.Draw(_spriteBatch, gridSlots);
                            _spriteBatch.DrawString(titleFont, "Checkerboard", new(windowWidth / 2 - titleFont.MeasureString("Checkerboard").X / 2, 10), Color.Black);
                            currentColor = gridSlots[8].Contains(mouseState[1].Position) ? Color.Red : Color.Gray;
                            _spriteBatch.DrawString(UIfont, "Complete the checkerboard! \nLeft click for a white square, and right click for a black square.", new(10, 720), Color.Black);
                            break;
                        case 3:
                            checkerboard.Reset();
                            blueSquare.Draw(_spriteBatch, gridSlots);
                            _spriteBatch.DrawString(titleFont, "Blue Square", new(windowWidth / 2 - titleFont.MeasureString("Blue Square").X / 2, 10), Color.Black);
                            _spriteBatch.DrawString(UIfont, "Easy break! \nSimply click the blue square to advance.", new(10, 720), Color.Black);
                            currentColor = gridSlots[8].Contains(mouseState[1].Position) ? Color.White : Color.Yellow;
                            break;
                        case 4:
                            blueSquare.Reset();
                            arrow.Draw(_spriteBatch, gridSlots);
                            _spriteBatch.DrawString(titleFont, "Arrow", new(windowWidth / 2 - titleFont.MeasureString("Arrow").X / 2, 10), Color.Black);
                            _spriteBatch.DrawString(UIfont, "Find the right arrow! Click the arrow that points in the same \ndirection as its position relative to the center of the grid.", new(10, 720), Color.Black);
                            text = "TO MENU";
                            break;
                    }
                    _spriteBatch.DrawString(titleFont, text, new(gridSlots[8].X + 30, gridSlots[8].Y + 70), currentColor);
                    break;
                case MicroGame.TicTacToe:
                    ticTacToe.Draw(_spriteBatch, gridSlots);
                    goto default;
                case MicroGame.Checkerboard:
                    checkerboard.Draw(_spriteBatch, gridSlots);
                    goto default;
                case MicroGame.BlueSquare:
                    blueSquare.Draw(_spriteBatch, gridSlots);
                    goto default;
                case MicroGame.Arrow:
                    arrow.Draw(_spriteBatch, gridSlots);
                    goto default;
                case MicroGame.GameEnd:
                    _spriteBatch.DrawString(titleFont, "TIME\nUP", new(gridSlots[0].X + 30, gridSlots[0].Y + 70), Color.Black);
                    _spriteBatch.DrawString(titleFont, "WELL\nDONE!", new(gridSlots[1].X + 40, gridSlots[1].Y + 70), Color.Black);
                    _spriteBatch.DrawString(titleFont, $"SCORE:\n{WinCount}!!", new(gridSlots[2].X + 40, gridSlots[2].Y + 70), Color.Black);

                    currentColor = gridSlots[8].Contains(mouseState[1].Position) ? Color.RoyalBlue : Color.Navy;
                    _spriteBatch.DrawString(titleFont, "TO MENU", new(gridSlots[8].X + 30, gridSlots[8].Y + 70), currentColor);
                    break;
                default:
                    _spriteBatch.DrawString(UIfont, $"Points: {winCount}", new(175, 25), Color.Black);
                    _spriteBatch.DrawString(UIfont, $"Time Left: {Math.Floor(timer)}", new(400, 25), Color.Black);
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

        private static MicroGame RandomMicrogame()
        {
            Random random = new();
            int choice = random.Next(1, 5);
            return (MicroGame)choice;
        }
    }
}
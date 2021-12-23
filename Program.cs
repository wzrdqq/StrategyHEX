using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace NoStepBack
{
    class Program
    {
        static RenderWindow window;

        public static RenderWindow Window { get { return window; } }
        public static GameConroller Game { get; private set; }

        public static View gameView;

        public static bool isRightClick = false;

        public static Vector2f vecStart;
        public static Vector2f vecClick;

        public static int zoomNimber = 1;
        public static float[] zoomUpper = new float[6] { 0.5f, 0.5f, 0.666f, 1f, 1.6f, 2.666f};

        static void Main(string[] args)
        {
            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 8;

            window = new RenderWindow(new SFML.Window.VideoMode(1920, 1080), "NoStepBack", Styles.Default, settings);
            window.SetVerticalSyncEnabled(true);

            window.Closed += WinClosed;
            window.Resized += WindowRealixed;
            window.KeyPressed += UpdateClick;
            window.MouseWheelScrolled += UpdateMouseScroll;
            window.MouseButtonPressed += UpdateMouseButtonPressed;
            window.MouseMoved += UpdateMouseMoved;
            window.MouseButtonReleased += UpdateMouseButtonRelised;

            Game = new GameConroller();

            gameView = window.GetView();
            gameView.Center = new Vector2f(Tile.offsetX * 2 * Chunk.chunkSize * TileMap.mapSize / 2, Tile.offsetY * Chunk.chunkSize * TileMap.mapSize / 2);
            window.SetView(gameView);

            while (window.IsOpen)
            {
                window.DispatchEvents();
                Game.Update();
                IfNewChunk();

                window.Clear(Color.Black);

                Game.Draw();

                window.Display();
            }
        }

        private static void WindowRealixed(object sender, SizeEventArgs e)
        {
            Vector2f center = gameView.Center;
            gameView = new View(new FloatRect(0, 0, e.Width, e.Height));
            gameView.Center = center;
            gameView.Zoom((float) Math.Pow(2, zoomNimber - 1));
            window.SetView(gameView);
        }

        private static void IfNewChunk()
        {
            Vector2f center = gameView.Center;
            Game.IfNewChunk(center);
        }

        private static void UpdateMouseScroll(object sender, MouseWheelScrollEventArgs e)
        {
            gameView = window.GetView();
            if ((e.Delta > -0.1f) && (zoomNimber > 0))
            {
                zoomNimber--;
                gameView.Zoom(0.5f);
            }
            if ((e.Delta < -0.1f) && (zoomNimber < 5))
            {
                zoomNimber++;
                gameView.Zoom(2f);
            }
            window.SetView(gameView);
        }

        private static void UpdateMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Right)
            {
                vecStart = new Vector2f(e.X - window.Size.X / 2, e.Y - window.Size.Y / 2);
                isRightClick = true;
            }
            if (e.Button == Mouse.Button.Left)
            {
                vecClick = new Vector2f((float)((e.X - window.Size.X / 2) * Math.Pow(2, zoomNimber - 1)), (float)((e.Y - window.Size.Y / 2) * Math.Pow(2, zoomNimber - 1)));
                Vector2f posInMap = new Vector2f(vecClick.X + gameView.Center.X, vecClick.Y + gameView.Center.Y);
                Game.TileClick(posInMap);
            }
        }

        private static void UpdateMouseButtonRelised(object sender, MouseButtonEventArgs e)
        {
            isRightClick = false;
        }

        private static void UpdateMouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (isRightClick)
            {
                gameView = window.GetView();
                Vector2f vecNow = new Vector2f(e.X - window.Size.X / 2, e.Y - window.Size.Y / 2);
                Vector2f vec = new Vector2f((vecStart.X - vecNow.X) * zoomUpper[zoomNimber] * (zoomNimber + 1), (vecStart.Y - vecNow.Y) * zoomUpper[zoomNimber] * (zoomNimber + 1));
                vecStart = vecNow;
                gameView.Move(vec);
                window.SetView(gameView);
            }
        }

        private static void UpdateClick(object sender, KeyEventArgs e)
        {
            gameView = window.GetView();
            if ((e.Code == Keyboard.Key.W))
            {
                gameView.Move(new Vector2f(0, -50 * (zoomNimber + 1)));
            }
            if (e.Code == Keyboard.Key.S)
            {
                gameView.Move(new Vector2f(0, 50 * (zoomNimber + 1)));
            }
            if (e.Code == Keyboard.Key.A)
            {
                gameView.Move(new Vector2f(-50 * (zoomNimber + 1), 0));
            }
            if (e.Code == Keyboard.Key.D)
            {
                gameView.Move(new Vector2f(50 * (zoomNimber + 1), 0));
            }
            if (e.Code == Keyboard.Key.X)
            {
                Game.TilePlusH();
            }
            if (e.Code == Keyboard.Key.Z)
            {
                Game.TileMinusH();
            }
            window.SetView(gameView);
        }

        private static void WinClosed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}

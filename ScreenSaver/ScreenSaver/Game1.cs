using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScreenSaver.Classes;
using System;
using System.Collections.Generic;

namespace ScreenSaver
{
    /// <summary>
    /// Основная программа
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Параметры движения 
        private const float BiasForX = 0.5f;           // Горизонтальная скорость 
        private const float BiasForY = 3.3f;           // Базовая вертикальная скорость

        // Размеры снежинок
        private const int MinFlakeSize = 30;
        private const int MaxFlakeSize = 60;

        // Количество и границы
        private const int MaxSnowFlakes = 150;

        // Границы появления и сброса
        private const int SpawnOffsetX = 150;          // Допуск слева/справа при создании
        private const int SpawnOffsetY = 500;          // Допуск сверху при создании
        private const int ResetOffsetY = 50;           // Минимальное смещение при сбросе

        private const int ResetThresholdLeft = 150;    // Порог сброса слева
        private const int ResetThresholdRight = 100;   // Порог сброса справа
        private const int ResetThresholdBottom = 100;  // Порог сброса снизу

        // Ресурсы
        private Texture2D backgroundTexture;
        private Texture2D snowFlakeTexture;

        private List<SnowFlake> snowFlakes = new List<SnowFlake>();
        private Random rnd = new Random();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        /// <summary>
        /// Инициализация скрин сейвера
        /// </summary>
        protected override void Initialize()
        {
            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            graphics.PreferredBackBufferWidth = displayMode.Width;
            graphics.PreferredBackBufferHeight = displayMode.Height;
            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// Загрузка ресурсов
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("gory_sneg_zima_132544_1920x1080");
            snowFlakeTexture = Content.Load<Texture2D>("snowflake");

            CreateSnowFlakes();
        }

        /// <summary>
        /// Создание снежинок
        /// </summary>
        private void CreateSnowFlakes()
        {
            int screenWidth = graphics.PreferredBackBufferWidth;

            for (int i = 0; i < MaxSnowFlakes; i++)
            {
                int size = rnd.Next(MinFlakeSize, MaxFlakeSize + 1);
                float speed = BiasForY * (MinFlakeSize / (float)size);

                snowFlakes.Add(new SnowFlake
                {
                    X = rnd.Next(-SpawnOffsetX, screenWidth + SpawnOffsetX),
                    Y = rnd.Next(-SpawnOffsetY, 0),
                    Size = size,
                    Speed = speed
                });
            }
        }

        /// <summary>
        /// Проверка изменений
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (keyboard.GetPressedKeys().Length > 0 ||
                mouse.LeftButton == ButtonState.Pressed ||
                mouse.RightButton == ButtonState.Pressed ||
                mouse.MiddleButton == ButtonState.Pressed)
            {
                Exit();
            }

            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;

            foreach (var flake in snowFlakes)
            {
                flake.X += BiasForX;
                flake.Y += flake.Speed;

                // Сброс, если снежинка ушла за пределы видимой зоны
                if (flake.Y > screenHeight + ResetThresholdBottom ||
                    flake.X < -ResetThresholdLeft ||
                    flake.X > screenWidth + ResetThresholdRight)
                {
                    flake.X = rnd.Next(-SpawnOffsetX, screenWidth + SpawnOffsetX);
                    flake.Y = rnd.Next(-SpawnOffsetY, -ResetOffsetY);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Отрисовка на экране
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Фон
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            // Снежинки
            foreach (var flake in snowFlakes)
            {
                var destRect = new Rectangle(
                    x: (int)(flake.X - flake.Size / 2f),
                    y: (int)(flake.Y - flake.Size / 2f),
                    width: flake.Size,
                    height: flake.Size
                );
                spriteBatch.Draw(snowFlakeTexture, destRect, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

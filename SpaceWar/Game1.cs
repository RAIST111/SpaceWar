using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceWar.Classes;
using System;
using System.Collections.Generic;

namespace SpaceWar
{
    public class Game1 : Game
    {
        // константы
        private const int COUNT_ASTEROIDS = 10;

        //инструменты
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //поля
        private Player _player;
        private Space _space;
        private Asteroid _asteroid;
        

        private List<Asteroid> _asteroids;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _player = new Player();
            _space = new Space();
            // _asteroid = new Asteroid();
            
            _asteroids = new List<Asteroid>();
            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _space.LoadContent(Content);
            _player.LoadContent(Content);
            //_asteroid.LoadContent(Content);
            
            for (int i = 0; i < COUNT_ASTEROIDS; i++)
            {
                LoadAsteroid();
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _player.Update(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Content);
            _space.Update();
           
            UpdateAsteroids();
            // _asteroid.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            {
                _space.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                // _asteroid.Draw(_spriteBatch);
                
                foreach (Asteroid asteroid in _asteroids)
                {
                    asteroid.Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private void UpdateAsteroids()
        {
            for (int i = 0; i < _asteroids.Count; i++)

            {
                Asteroid asteroid = _asteroids[i];

                asteroid.Update();
                //teleport
                if (asteroid.Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    Random random = new Random();
                    int x = random.Next(0, _graphics.PreferredBackBufferWidth - asteroid.Width);
                    int y = random.Next(0, _graphics.PreferredBackBufferHeight);
                    asteroid.Position = new Vector2(x, -y);
                }
                if (asteroid.Collision.Intersects(_player.Collision))
                {
                    _asteroids.Remove(asteroid);
                    i--;
                }

            }
            // загружаем доп элементы в игру
            if (_asteroids.Count < COUNT_ASTEROIDS)
            {
                LoadAsteroid();
            }
        }
        private void LoadAsteroid()
        {
            Asteroid asteroid = new Asteroid();
            asteroid.LoadContent(Content);
            Random random = new Random();
            int x = random.Next(0, _graphics.PreferredBackBufferWidth - asteroid.Width);
            int y = random.Next(0, _graphics.PreferredBackBufferHeight);
            asteroid.Position = new Vector2(x, -y);
            _asteroids.Add(asteroid);
        }
    }
}

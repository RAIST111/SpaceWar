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
        
        private Label _label;
        private GameMode _GameMode = GameMode.menu;
        private GameOver _gameOver;
        private MainMenu _mainMenu;
        private List<Asteroid> _asteroids;
        private List<Explosion> _explosions;
        
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
            _explosions = new List<Explosion>();
            _gameOver = new GameOver(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _mainMenu = new MainMenu();
            _label = new Label(Vector2.Zero, "Hello world", Color.White);
            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _space.LoadContent(Content);
            _player.LoadContent(Content);
            _gameOver.LoadContent(Content);
            _mainMenu.LoadContent(Content);
            //_asteroid.LoadContent(Content);

            for (int i = 0; i < COUNT_ASTEROIDS; i++)
            {
                LoadAsteroid();
            }
            _label.LoadContent(Content);
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (_GameMode)
            {
                case GameMode.menu:
                    _mainMenu.Update();
                    break;
                case GameMode.playing:
            _player.Update(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Content);
            _space.Update();
           
            UpdateAsteroids();
            CheckCollision();
            UpdateExplosions(gameTime);    

                    break;
                case GameMode.GameOver:
                    _gameOver.Update();
                    _space.Update();
                    break;
                
            }
           
            // _asteroid.Update();
            //_explosion.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            {
                switch (_GameMode)
                {
                    case GameMode.menu:
                        _mainMenu.Draw(_spriteBatch);
                        break;
                    case GameMode.playing:
                     _space.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                
                // _asteroid.Draw(_spriteBatch);
                
                foreach (Asteroid asteroid in _asteroids)
                {
                    asteroid.Draw(_spriteBatch);
                }

                foreach(Explosion explosion in _explosions)
                {
                    explosion.Draw(_spriteBatch);
                }

                _label.Draw(_spriteBatch);      
                        break;
                    case GameMode.GameOver:
                        _space.Draw(_spriteBatch);
                        _gameOver.Draw(_spriteBatch);

                        
                        break;
                    
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
                if (!asteroid.IsAlive)
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
        private void CheckCollision()
        {
            foreach(Asteroid asteroid in _asteroids)
            {
                //кажджый астероид и игрока
                if (asteroid.Collision.Intersects(_player.Collision))
                {
                    asteroid.IsAlive = false;
                    Explosion explosion = new Explosion(asteroid.Position);
                    Vector2 position = asteroid.Position;
                    position = new Vector2(position.X - explosion.Width / 2, position.Y - explosion.Height / 2);
                    position = new Vector2(position.X + asteroid.Width / 2, position.Y + asteroid.Height / 2);
                    explosion.Position = position;
                    explosion.LoadContent(Content);
                    _explosions.Add(explosion);
                }
                // каждый астероид и каждую пулю
                foreach ( Bullet bullet in _player.Bullets)
                {
                    if (asteroid.Collision.Intersects(bullet.Collision))
                    {
                        asteroid.IsAlive = false;
                        bullet.IsAlive = false;
                        Explosion explosion = new Explosion(asteroid.Position);
                        Vector2 position = asteroid.Position;
                        position = new Vector2(position.X - explosion.Width / 2, position.Y - explosion.Height / 2);
                        position = new Vector2(position.X + asteroid.Width / 2, position.Y + asteroid.Height / 2);

                        explosion.Position = position;
                        explosion.LoadContent(Content);
                        _explosions.Add(explosion);
                    }
                }
            }
        }
        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = 0; i <_explosions.Count; i++)
            {
                _explosions[i].Update(gameTime);
                if (!_explosions[i].IsAlive)
                {
                    _explosions.RemoveAt(i);
                    i--;    
                }
            }
        }
    }
}

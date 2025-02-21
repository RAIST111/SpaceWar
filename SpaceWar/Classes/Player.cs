﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceWar.Classes
{
    public class Player
    {
        private Vector2 _position;
        private Texture2D _texture;
        private float _speed;
        private Rectangle _collision;
        //weapon
        private List<Bullet> _bulletList = new List<Bullet>(); // магазин патронов
        //time
        private int _timer = 0;
        private int _maxTime = 15d;
        public Rectangle Collision
        { 
            get { return _collision; } 
        }
        public Player() 
        {
            _position = new Vector2(30, 30);
            _texture = null;
            _speed = 7;
            _collision = new Rectangle((int)_position.X,(int)_position.Y, 0, 0);
        }
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("player");
        }
        public void Update(int widthScreen, int heightScreen, ContentManager content)
        {
            KeyboardState keyboard = Keyboard.GetState();

            #region Movment
            if (keyboard.IsKeyDown(Keys.S))
            {
                _position.Y += _speed;
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                _position.Y -= _speed;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                _position.X -= _speed;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                _position.X += _speed;
            }
            #endregion

            #region Bounds
            if(_position.X <= 0)
            {
                _position.X = 0;
            }
            if (_position.Y <= 0)
            {
                _position.Y = 0;
            }
           if( _position.Y >= heightScreen - _texture.Height)
            {
                _position.Y = heightScreen - _texture.Height;
            }
           if (_position.X >= widthScreen- _texture.Width)
            {
                _position.X = widthScreen- _texture.Width;
            }

            #endregion

            _collision = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            if(_timer <= _maxTime)
            {
                _timer++;
            }
            if (keyboard.IsKeyDown(Keys.Space) && _timer >= _maxTime)
            {
                Bullet bullet = new Bullet();
                bullet.Position = new Vector2(_position.X + _texture.Width / 2 - bullet.Width / 2, _position.Y - bullet.Height /2);
                bullet.LoadContent(content);
                _bulletList.Add(bullet);
                _timer = 0;
            }
            //работа со всемти пульками в игре
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace SpaceWar.Classes
{
    internal class Explosion
    {
        private Texture2D _texture;
        private Vector2 _position;
        private double _time = 0.0f;
        private double _duration = 30.0f;
        private int _frameNumber = 0;
        private int _widthFrame = 117;
        private int _heightFrame = 117;
        private Rectangle _sourceRectangle;
        public bool IsAlive { get; set; } = true;
        public int Width
        {
            get { return _widthFrame; }
        }
        public int Height
        {
            get { return _heightFrame; }
        }
        public Vector2 Position
        {
            set { _position = value; }
        }

        public Explosion(Vector2 position)
        {
            _texture = null;
            _position = position;
            _sourceRectangle = new Rectangle(_frameNumber * _widthFrame, 0, _widthFrame, _heightFrame);
        }
        public void LoadContent(ContentManager Content)
        {
            _texture = Content.Load<Texture2D>("explosion3");
        }
       public void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_time >= _duration)
            {
               _frameNumber++;
                _time = 0;
            }
            if (_frameNumber == 17)
            {
                IsAlive = false;
            }
            _sourceRectangle = new Rectangle(_frameNumber * _widthFrame, 0, _widthFrame, _heightFrame);
            Debug.WriteLine("Time:" + gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White);
        }
    }
}

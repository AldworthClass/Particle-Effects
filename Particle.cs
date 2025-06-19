using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Effects
{
    public class Particle
    {
        public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
        public Vector2 Position { get; set; }        // The current position of the particle        
        public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
        public float Angle { get; set; }            // The current angle of rotation of the particle
        public float AngularVelocity { get; set; }  // The speed that the shape rotates at in radians
        public Color Color { get; set; }            // The color of the particle
        public float Size { get; set; }             // The size of the particle
        public int TTL { get; set; }                // The 'time to live' of the particle

        private float _fadeOutSpeed;

        public bool _fadeOut;
        public float Opacity { get; set; }

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, bool fadeOut)
        {
            Opacity = 1f;
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            _fadeOut = fadeOut;
            if (!fadeOut)
            {
                _fadeOutSpeed = 0f;
            }
            else
            {
                _fadeOutSpeed = 1.0f / TTL; // Calculate fade out speed based on TTL
            }
        }
        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;          
            Opacity -= _fadeOutSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color * Opacity,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}

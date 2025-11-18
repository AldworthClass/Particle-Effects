using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Effects
{
    /// <summary>
    /// Represents a single particle in a particle system, with properties for position, velocity, angular velocity, color, size, and lifetime.
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// Gets or sets the texture used to draw the particle.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets the current position of the particle.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the particle.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the current angle of rotation of the particle.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the angular velocity (rotation speed) of the particle in radians.
        /// </summary>
        public float AngularVelocity { get; set; }

        /// <summary>
        /// Gets or sets the color of the particle.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the size of the particle.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Gets or sets the time to live (TTL) of the particle, in frames.
        /// </summary>
        public int TTL { get; set; }

        private float _fadeOutSpeed;

        /// <summary>
        /// Indicates whether the particle should fade out over its lifetime.
        /// </summary>
        public bool _fadeOut;

        /// <summary>
        /// Gets or sets the opacity of the particle (1.0 = fully opaque, 0.0 = fully transparent).
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets whether gravity will be applied to the particles.
        /// </summary>
        public bool ApplyGravity {  get; set; }
        /// <summary>
        /// Gets or sets what gravity should be. 
        /// </summary>
        public float Gravity {  get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Particle"/> class with the specified properties.
        /// </summary>
        /// <param name="texture">The texture to use for the particle.</param>
        /// <param name="position">The initial position of the particle.</param>
        /// <param name="velocity">The initial velocity of the particle.</param>
        /// <param name="angle">The initial rotation angle of the particle.</param>
        /// <param name="angularVelocity">The angular velocity (rotation speed) of the particle.</param>
        /// <param name="color">The color of the particle.</param>
        /// <param name="size">The size of the particle.</param>
        /// <param name="ttl">The time to live (TTL) of the particle, in frames.</param>
        /// <param name="fadeOut">Whether the particle should fade out over its lifetime.</param>
        /// <param name="applyGravity">gravity should be applied to the particle.</param>
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, bool fadeOut, bool applyGravity, float gravity)
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
            ApplyGravity = applyGravity;
            Gravity = gravity;
            if (!fadeOut)
            {
                _fadeOutSpeed = 0f;
            }
            else
            {
                _fadeOutSpeed = 1.0f / TTL; // Calculate fade out speed based on TTL
            }
        }

        /// <summary>
        /// Updates the particle's state, including position, angle, TTL, and opacity.
        /// </summary>
        public void Update()
        {
            TTL--;
            Position += Velocity;
            if (ApplyGravity)
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
            Angle += AngularVelocity;          
            Opacity -= _fadeOutSpeed;
        }

        /// <summary>
        /// Draws the particle using the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the particle.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color * Opacity,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}

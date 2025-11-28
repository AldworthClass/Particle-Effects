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
    /// Manages a collection of particles, handling their creation, updating, and rendering.
    /// </summary>
    public class ParticleSystem
    {
        private Random _generator;

        /// <summary>
        /// Gets or sets the location of the particle emitter.
        /// </summary>
        public Vector2 EmitterLocation { get; set; }

        private Vector2 _direction;
        private List<Particle> _particles;
        private List<Texture2D> _textures;

        private float _particleDensity; // value between 0-1 a percentage
        private float _rotationSpeed;   // Speed at which particles rotate
        private float _angleSpread;     // Spread of particles in radians
        private float _particleSpeed;   // Speed particles travel in pixels per frame
        private float _lifetime;        // Approx lifetime in seconds with some randomness built in
        private int _maxParticles;      // the maximum number of particles allowed
        private float _gravity;
        int _minParticleSize;
        int _maxParticleSize;
        private Color _color;
        private bool _enabled;
        private bool _randomizeColor;
        private bool _randomizeParticleSize;
        private bool _randomizeRotation;
        private bool _fadeOut;
        private bool _applyGravity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystem"/> class with default parameters.
        /// </summary>
        /// <param name="textures">A list of textures to use for particles.</param>
        /// <param name="location">The location of the emitter.</param>
        public ParticleSystem(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _generator = new Random();

            _direction = Vector2.Zero;
            _particleDensity = 1f;
            _maxParticles = 500;
            _enabled = true;
            _randomizeColor = true;
            _rotationSpeed = 0f;
            _randomizeRotation = true;
            _randomizeParticleSize = true;
            _minParticleSize = 1;
            _maxParticleSize = 5;
            _angleSpread = MathHelper.PiOver2;
            _lifetime = 2f;
            _particleSpeed = 1f;
            _fadeOut = false;
            _applyGravity = false;
            _gravity = 0.1f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystem"/> class with custom parameters.
        /// </summary>
        /// <param name="textures">A list of textures to use for particles.</param>
        /// <param name="location">The location of the emitter.</param>
        /// <param name="direction">The direction in which particles are emitted.</param>
        /// <param name="speed">The speed of emitted particles.</param>
        /// <param name="angleOfSpread">The angle of spread around the direction vector, in radians.</param>
        /// <param name="lifetime">The lifetime of particles, in seconds.</param>
        /// <param name="color">The color of the particles.</param>
        public ParticleSystem(List<Texture2D> textures, Vector2 location, Vector2 direction, float speed, float angleOfSpread, float lifetime, Color color)
        {
            _randomizeColor = false;
            _direction = direction;
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _generator = new Random();
            _particleDensity = 0.2f;
            _maxParticles = 500;
            _randomizeParticleSize = true;
            _minParticleSize = 1;
            _maxParticleSize = 5;
            _direction = new Vector2(1, 1);
            _angleSpread = MathHelper.PiOver2;
            _color = color;
            _particleSpeed = speed;
            _lifetime = lifetime;
            _applyGravity = false;
            _gravity = 0.1f;
        }

        /// <summary>
        /// Generates a new particle with randomized properties based on the system's settings.
        /// </summary>
        /// <returns>A new <see cref="Particle"/> instance.</returns>
        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[_generator.Next(_textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 particleDirection;
            if (_direction == Vector2.Zero) // When no direction specified, angle of spread is 360 degrees
            {
                particleDirection = new Vector2(
                    (float)(_generator.NextDouble() * 2 - 1),
                    (float)(_generator.NextDouble() * 2 - 1));
            }
            else  // determines a random direction based on direction and the anglle of spread
            {
                _direction.Normalize();
                particleDirection = GetRandomRotatedVector(_direction, _angleSpread / 2);
            }

           //// Determines velocity of particles generated
           //Vector2 velocity = new Vector2(
           //            1f * (float)(_generator.NextDouble() * 2 - 1),
           //            1f * (float)(_generator.NextDouble() * 2 - 1));

            float angle = 0;
            float angularVelocity;
            if (_randomizeRotation)
                angularVelocity = 0.1f * (float)(_generator.NextDouble() * 2 - 1);
            else
                angularVelocity = _rotationSpeed; // No rotation if not randomized

            if (_randomizeColor)
            {
                _color = new Color(
                    (float)_generator.NextDouble(),
                    (float)_generator.NextDouble(),
                    (float)_generator.NextDouble());
            }

            float size = (float)_generator.NextDouble();
            int ttl = (int)Math.Round(60 * _lifetime + _generator.Next(-5, 5)); // Adds a bit of randomness for particle lifetime

            return new Particle(texture, position, particleDirection * _particleSpeed, angle, angularVelocity, _color, size, ttl, _fadeOut, _applyGravity, _gravity);
        }

        /// <summary>
        /// Rotates a vector by a given angle in radians.
        /// </summary>
        /// <param name="v">The vector to rotate.</param>
        /// <param name="angleRadians">The angle in radians.</param>
        /// <returns>The rotated vector.</returns>
        public Vector2 RotateVector(Vector2 v, float angleRadians)
        {
            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);
            return new Vector2(
            v.X * cos - v.Y * sin,
            v.X * sin + v.Y * cos
            );
        }

        /// <summary>
        /// Rotates a vector by a random angle offset within the specified range.
        /// </summary>
        /// <param name="baseDirection">The base direction vector.</param>
        /// <param name="maxAngleOffsetRadians">The maximum angle offset in radians.</param>
        /// <returns>The rotated vector.</returns>
        public Vector2 GetRandomRotatedVector(Vector2 baseDirection, float maxAngleOffsetRadians)
        {
            Random rand = new Random();
            float offset = (float)(rand.NextDouble() * 2 - 1) * maxAngleOffsetRadians; // -max to +max
            return RotateVector(baseDirection, offset);
        }

        /// <summary>
        /// Updates all particles in the system, adding new ones and removing expired ones.
        /// </summary>
        public void Update()
        {
            int total = 5;
            if (_enabled)
            {
                for (int i = 0; i < total * _particleDensity; i++)
                {
                    _particles.Add(GenerateNewParticle());
                }
            }
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Update();
                if (_particles[i].TTL <= 0 || _particles[i].Opacity <= 0)
                {
                    _particles.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Draws all particles in the system using the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the particles.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        // Accessor properties for particle system parameters

        /// <summary>
        /// Gets or sets the direction in which particles are emitted.
        /// </summary>
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public bool ApplyGravity
        {
            get { return _applyGravity; }
            set { _applyGravity = value; }
        }


        /// <summary>
        /// Gets or sets the gravity applied to particles.
        /// </summary>
        public float Gravity
        {
            get { return _gravity; }
            set { _gravity = value; }
        }
        /// <summary>
        /// Gets or sets the density of particles generated per update.
        /// </summary>
        public float ParticleDensity
        {
            get { return _particleDensity; }
            set { _particleDensity = value; }
        }
        

        /// <summary>
        /// Gets or sets the maximum number of particles in the system.
        /// </summary>
        public int MaxParticles
        {
            get { return _maxParticles; }
            set { _maxParticles = value; }
        }

        /// <summary>
        /// Gets or sets the angle of spread in radians.
        /// </summary>
        public float AngleSpread
        {
            get { return _angleSpread; }
            set { _angleSpread = value; }
        }

        /// <summary>
        /// Gets or sets the color of the particles.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the speed at which particles move away from the emitter.
        /// </summary>
        public float ParticleSpeed
        {
            get { return _particleSpeed; }
            set { _particleSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the minimum size of particles.
        /// </summary>
        public int MinParticleSize
        {
            get { return _minParticleSize; }
            set
            {
                if (value > _maxParticleSize)
                    _minParticleSize = _maxParticleSize;
                else
                    _minParticleSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of particles.
        /// </summary>
        public int MaxParticleSize
        {
            get { return _maxParticleSize; }
            set
            {
                if (value < _minParticleSize)
                {
                    _maxParticleSize = _minParticleSize;
                }
                else
                    _maxParticleSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the color of particles.
        /// </summary>
        public bool RandomizeColor
        {
            get { return _randomizeColor; }
            set { _randomizeColor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the size of particles.
        /// </summary>
        public bool RandomizeParticleSize
        {
            get { return _randomizeParticleSize; }
            set
            {
                _randomizeParticleSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the particle system is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Gets or sets the lifetime of particles in seconds.
        /// </summary>
        public float Duration
        {
            get { return _lifetime; }
            set { _lifetime = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the rotation of particles.
        /// </summary>
        public bool RandomizeRotation
        {
            get { return _randomizeRotation; }
            set { _randomizeRotation = value; }
        }

        /// <summary>
        /// Gets or sets the rotation speed of particles in radians per second.
        /// </summary>
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether particles should fade out over time.
        /// </summary>
        public bool FadeOut
        {
            get { return _fadeOut; }
            set { _fadeOut = value; }
        }
    }
}

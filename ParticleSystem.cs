using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Effects
{
    public class ParticleSystem
    {
        private Random _generator;
        public Vector2 EmitterLocation { get; set; }

        private Vector2 _direction;
        private List<Particle> _particles;
        private List<Texture2D> _textures;


        private float _particleDensity; // a percent, controlling how many particles are generated per update
        private float _rotationSpeed; // Speed of rotation in radians per second
        private float _angleSpread; // angle of spread around the direction vector in radians
        private float _particleSpeed;
        private float _lifetime; // Lifetime of particles in seconds (not currently used in this implementation)


        private int _maxParticles;

        int _minParticleSize; // Minimum size of particles
        int _maxParticleSize; // Maximum size of particles


        private Color _color;

        private bool _enabled; // Whether the particle system is enabled or not
        private bool _randomizeColor; // Whether to randomize the color of particles
        private bool _randomizeParticleSize; // Whether to randomize the size of particles
        private bool _randomizeRotation;
        private bool _fadeOut;



        // Creates with default values for direction, angle of spread, lifetime, speed, and color
        public ParticleSystem(List<Texture2D> textures, Vector2 location)
        {

            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _generator = new Random();

            _direction = Vector2.Zero; // Defaults to all directions
            _particleDensity = 1f; // Adjust this value to control the density of particles
            _maxParticles = 500; // Maximum number of particles in the system

            _enabled = true;
            _randomizeColor = true;
            _rotationSpeed = 0f; // No rotation by default
            _randomizeRotation = true; // Default to randomizing rotation

            _randomizeParticleSize = true; // Default to randomizing particle size
            _minParticleSize = 1;
            _maxParticleSize = 5; // Maximum size of particles

            _angleSpread = MathHelper.PiOver2;// MathHelper.PiOver4;

            _lifetime = 2f; // Approximate lifetime of particles in seconds

            _particleSpeed = 1f;
            _fadeOut = false; // Default to no fade out


        }
        // Allows user to speficy multiple values
        public ParticleSystem(List<Texture2D> textures, Vector2 location, Vector2 direction, float speed, float angleOfSpread, float lifetime, Color color)
        {

            _randomizeColor = false; // Color is set by the user, so no need to randomize
            _direction = direction;
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _generator = new Random();
            //_direction = Vector2.Zero;
            _particleDensity = 0.2f; // Adjust this value to control the density of particles
            _maxParticles = 500; // Maximum number of particles in the system

            _randomizeParticleSize = true; // Default to randomizing particle size
            _minParticleSize = 1;
            _maxParticleSize = 5; // Maximum size of particles

            _direction = new Vector2(1, 1);
            _angleSpread = MathHelper.PiOver2;// MathHelper.PiOver4;

            _color = color;

            _particleSpeed = speed;

            _lifetime = lifetime; // Approximate lifetime of particles in seconds

        }

        private Particle GenerateNewParticle()
        {

            Texture2D texture = _textures[_generator.Next(_textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 particleDirection;
            if (_direction == Vector2.Zero)
            {
                // Randomly choose a direction if none is set
                particleDirection = new Vector2(
                    (float)(_generator.NextDouble() * 2 - 1),
                    (float)(_generator.NextDouble() * 2 - 1));
            }
            else // Allows control for direction and spread of angles
            {
                float newDirection;
                int minAngle, maxAngle, angleRange;
                // Normalize the direction to ensure consistent particle movement
                _direction.Normalize();

                // Randomly rotate the direction vector within the specified angle spread
                particleDirection = GetRandomRotatedVector(_direction, _angleSpread / 2);

            }

            Vector2 velocity = new Vector2(
                        1f * (float)(_generator.NextDouble() * 2 - 1),
                        1f * (float)(_generator.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity;
            if (_randomizeRotation)
                angularVelocity = 0.1f * (float)(_generator.NextDouble() * 2 - 1);
            else
                angularVelocity = _rotationSpeed; // No rotation if not randomized

            if (_randomizeColor)
            {
                // Randomize color if the flag is set
                _color = new Color(
                    (float)_generator.NextDouble(),
                    (float)_generator.NextDouble(),
                    (float)_generator.NextDouble());
            }



            float size = (float)_generator.NextDouble();
            int ttl = (int)Math.Round(60 * _lifetime + _generator.Next(-5, 5)); // Adds a bit of randomness assuming 60FPS

            return new Particle(texture, position, particleDirection * _particleSpeed, angle, angularVelocity, _color, size, ttl, _fadeOut);
        }

        // Rotates a directional Vector by a given angle in radians
        public Vector2 RotateVector(Vector2 v, float angleRadians)
        {
            float cos = (float)Math.Cos(angleRadians);
            float sin = (float)Math.Sin(angleRadians);
            return new Vector2(
            v.X * cos - v.Y * sin,
            v.X * sin + v.Y * cos
            );
        }

        // Rotates a directional vector by a random angle offset within the specified range
        public Vector2 GetRandomRotatedVector(Vector2 baseDirection, float maxAngleOffsetRadians)
        {
            Random rand = new Random();
            float offset = (float)(rand.NextDouble() * 2 - 1) * maxAngleOffsetRadians; // -max to +max
            return RotateVector(baseDirection, offset);
        }

        // Adds and removes particles from the system
        public void Update()
        {

            // The number of particles to be added per frame at a density of 1.0
            int total = 5;
            if (_enabled)   // Only generates particles when enabled
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
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public float ParticleDensity
        {
            get { return _particleDensity; }
            set { _particleDensity = value; }
        }
        public int MaxParticles
        {
            get { return _maxParticles; }
            set { _maxParticles = value; }
        }

        // Angle of spread in radians
        public float AngleSpread
        {
            get { return _angleSpread; }
            set { _angleSpread = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public float ParticleSpeed
        {
            get { return _particleSpeed; }
            set { _particleSpeed = value; }
        }
        public int MinParticleSize
        {
            get { return _minParticleSize; }
            set
            {
                if (value > _maxParticleSize)
                    _minParticleSize = _maxParticleSize; // Ensure min is not greater than max
                else
                    _minParticleSize = value;
            }
        }
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
        public bool RandomizeColor
        {
            get { return _randomizeColor; }
            set { _randomizeColor = value; }
        }
        public bool RandomizeParticleSize
        {
            get { return _randomizeParticleSize; }
            set
            {
                _randomizeParticleSize = value;
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        public float Duration
        {
            get { return _lifetime; }
            set { _lifetime = value; }

        }
        public bool RandomizeRotation
        {
            get { return _randomizeRotation; }
            set { _randomizeRotation = value; }
        }
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }

        }
        public bool FadeOut
        {
            get { return _fadeOut; }
            set { _fadeOut = value; } // Allows particles to fade out over time

        }
    }
}

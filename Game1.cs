using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Particle_Effects
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D starTexture, circleTexture, diamondTexture;
        ParticleSystem particleSystem;

        Vector2 direction;

        KeyboardState KeyboardState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(circleTexture);
            textures.Add(starTexture);
            textures.Add(diamondTexture);
            particleSystem = new ParticleSystem(textures, new Vector2(400, 240));
            //particleSystem.Enabled = false; // Start with the particle system disabled
            particleSystem.ParticleDensity = 0.5f;
            particleSystem.Duration = 2f; // Duration in seconds
            particleSystem.ParticleSpeed = 1.5f;
            particleSystem.RotationSpeed = -0.3f;
            //particleSystem.RandomizeRotation = false;
            particleSystem.FadeOut = true; // Enable fade out effect
            particleSystem.AngleSpread = MathHelper.Pi;

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            starTexture = Content.Load<Texture2D>("star");
            circleTexture = Content.Load<Texture2D>("circle");
            diamondTexture = Content.Load<Texture2D>("diamond");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            direction = Vector2.Zero;
            if (KeyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (KeyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;
            if (KeyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (KeyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (KeyboardState.IsKeyDown(Keys.Space))
                particleSystem.AngleSpread = 0.2f;
            else
                particleSystem.AngleSpread = MathHelper.PiOver4;

            if (KeyboardState.IsKeyDown(Keys.R))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Red; // Toggle the particle system on/off
            }
            if (KeyboardState.IsKeyDown(Keys.G))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Green; // Toggle the particle system on/off
            }

            if (KeyboardState.IsKeyDown(Keys.Enter))
            {
                particleSystem.RandomizeColor = true; // Toggle random color
                
            }

            particleSystem.Direction = direction;

            particleSystem.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            particleSystem.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            particleSystem.Draw(_spriteBatch);
            _spriteBatch.Begin();
            _spriteBatch.Draw(circleTexture, new Rectangle(10, 10, 100, 100), Color.White   );
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
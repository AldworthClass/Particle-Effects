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

        KeyboardState keyboardState, previousKeyboardState;
        MouseState mouseState, previousMouseState;

        SpriteFont instructionsFont;

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
            particleSystem.ParticleSpeed = 2f;
            particleSystem.RotationSpeed = -0.3f;
            //particleSystem.RandomizeRotation = false;
            particleSystem.FadeOut = true; // Enable fade out effect
            particleSystem.AngleSpread = MathHelper.PiOver2;
            //particleSystem.ApplyGravity = true;

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            starTexture = Content.Load<Texture2D>("star");
            circleTexture = Content.Load<Texture2D>("circle");
            diamondTexture = Content.Load<Texture2D>("diamond");
            instructionsFont = Content.Load<SpriteFont>("InstructionsFont");
        }

        protected override void Update(GameTime gameTime)
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            direction = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;
            if (keyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;

            // Remembers Direction
            if (direction != Vector2.Zero)
                particleSystem.Direction = direction;
            if (keyboardState.IsKeyDown(Keys.Space))
                direction = Vector2.Zero;

            // Color
            if (keyboardState.IsKeyDown(Keys.D1))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Red; // Toggle the particle system on/off
            }
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Blue; // Toggle the particle system on/off
            }
            if (keyboardState.IsKeyDown(Keys.D3))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Green; // Toggle the particle system on/off
            }
            if (keyboardState.IsKeyDown(Keys.D4))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.White; // Toggle the particle system on/off
            }

            // Toggle random color
            if (keyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyDown(Keys.R))
                particleSystem.RandomizeColor = !particleSystem.RandomizeColor;
          
            // Toggle gravity
            if (keyboardState.IsKeyDown(Keys.G) && previousKeyboardState.IsKeyDown(Keys.G))
                particleSystem.ApplyGravity = !particleSystem.ApplyGravity; // Toggle random color

            // Adjust angle of spread
            // Decrease
            if (keyboardState.IsKeyDown(Keys.A) && mouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
            {
                particleSystem.AngleSpread = MathHelper.Clamp(particleSystem.AngleSpread - 0.2f, 0f, MathHelper.TwoPi);

            }
            // Increase
            if (keyboardState.IsKeyDown(Keys.A) && mouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
            {
                particleSystem.AngleSpread = MathHelper.Clamp(particleSystem.AngleSpread + 0.2f, 0f, MathHelper.TwoPi);

            }


            // Adjust Speed
            // Decrease
            if (keyboardState.IsKeyDown(Keys.S) && mouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
            {
                particleSystem.ParticleSpeed -= 0.1f;
                if (particleSystem.ParticleSpeed <= 0f)
                    particleSystem.ParticleSpeed = 0.1f;

            }
            // Increase
            if (keyboardState.IsKeyDown(Keys.S) && mouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
            {
                particleSystem.ParticleSpeed += 0.1f;

            }


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


            _spriteBatch.DrawString(instructionsFont, "Press arrow keys to change direction", new Vector2(10, 10), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Press number keys to view different colors", new Vector2(10, 30), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Press r to toggle random colors on/off", new Vector2(10, 50), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Press g to toggle gravity on/off", new Vector2(10, 70), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Hold A and Mouse wheel to narrow/widen angle of spread", new Vector2(10, 90), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Hold S and Mouse wheel to increase/decrease particle speed", new Vector2(10, 110), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Hold L and Mouse wheel to increase/decrease particle lifetime", new Vector2(10, 130), Color.Yellow);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
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
            particleSystem.ParticleSpeed = 3.5f;
            particleSystem.RotationSpeed = -0.3f;
            //particleSystem.RandomizeRotation = false;
            particleSystem.FadeOut = true; // Enable fade out effect
            particleSystem.AngleSpread = MathHelper.Pi;
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
            if (keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;
            if (keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.Space))
                particleSystem.AngleSpread = 0.2f;
            else
                particleSystem.AngleSpread = MathHelper.PiOver4;

            if (keyboardState.IsKeyDown(Keys.R))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Red; // Toggle the particle system on/off
            }
            if (keyboardState.IsKeyDown(Keys.G))
            {
                particleSystem.RandomizeColor = false; // Toggle random color
                particleSystem.Color = Color.Green; // Toggle the particle system on/off
            }

            if (keyboardState.IsKeyDown(Keys.Enter))
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


            _spriteBatch.DrawString(instructionsFont, "Press arrow keys to change direction", new Vector2(10, 10), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Press number keys to view different colors", new Vector2(10, 30), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Press r to toggle random colors on/off", new Vector2(10, 10), Color.Yellow);
            _spriteBatch.DrawString(instructionsFont, "Press g to toggle gravity on/off", new Vector2(10, 10), Color.Yellow);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
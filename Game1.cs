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

            base.Draw(gameTime);
        }
    }
}
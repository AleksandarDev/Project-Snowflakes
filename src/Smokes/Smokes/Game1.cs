using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Snowflake.Core;

namespace Smokes {
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game {
		GraphicsDeviceManager graphicsDeviceManager;
		SpriteBatch spriteBatch;

		const string FONT_DEBUG_ASSET_NAME = "Fonts/fontDebug";
		SpriteFont fontDebug;

		MouseState previousMouseState;
		KeyboardState previousKeyboardState;

		SmokeSource source;


		public Game1() {
			graphicsDeviceManager = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
#if DEBUG
			graphicsDeviceManager.PreferredBackBufferWidth = 1024;
			graphicsDeviceManager.PreferredBackBufferHeight = 768;
			graphicsDeviceManager.IsFullScreen = false;
			graphicsDeviceManager .ApplyChanges();

			this.IsMouseVisible = true;
#else
			graphicsDeviceManager.PreferredBackBufferWidth = graphicsDeviceManager.GraphicsDevice.DisplayMode.Width;
			graphicsDeviceManager.PreferredBackBufferHeight = graphicsDeviceManager.GraphicsDevice.DisplayMode.Height;
			graphicsDeviceManager.IsFullScreen = true;
			graphicsDeviceManager.ApplyChanges();

			this.IsMouseVisible = false;
#endif

			previousKeyboardState = Keyboard.GetState();
			previousMouseState = Mouse.GetState();

			source = new SmokeSource(this, 500);
			Components.Add(source);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			fontDebug = Content.Load<SpriteFont>(FONT_DEBUG_ASSET_NAME);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			MouseState mouseState = Mouse.GetState();
			KeyboardState keyboardState = Keyboard.GetState();

			if(keyboardState.IsKeyDown(Keys.Escape))
				Exit();

			if(mouseState.LeftButton == ButtonState.Pressed &&
				previousMouseState.LeftButton == ButtonState.Released)
				source.Position = new Vector2(mouseState.X, mouseState.Y);

			base.Update(gameTime);

			previousMouseState = mouseState;
			previousKeyboardState = keyboardState;
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin();
#if DEBUG
			spriteBatch.DrawString(fontDebug, source.ToString(), new Vector2(100), Color.White);
#endif
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}

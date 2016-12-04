using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Snowflakes.Particles;


namespace Snowflakes {
	class GameStart : Game {
		GraphicsDeviceManager device;
		SpriteBatch spriteBatch;

		IntPtr windowsIntPtr;
		Dictionary<System.Windows.Forms.Screen, System.Drawing.Bitmap> screens;
		Texture2D[] screenCapturesTexture;

		private const string FONT_DEBUG_ASSET_NAME = "Fonts/fontDebug";
		private const string FILE_SETTING_NAME = "settings.txt";
		private const int MaxParticles = 512;
#if DEBUG
		SpriteFont fontDebug;
#endif

		KeyboardState previousKeyboardState;
		KeyboardState currentKeyboardState;
		MouseState previousMouseState;
		MouseState currentMouseState;

		ParticleSnowflakeSource snowSource;


		public GameStart(IntPtr window, int windowWidth, int windowHeight, Dictionary<System.Windows.Forms.Screen, System.Drawing.Bitmap> screens) {
			device = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			this.windowsIntPtr = window;

			device.PreparingDeviceSettings +=
				new EventHandler<PreparingDeviceSettingsEventArgs>(device_PreparingDeviceSettings);
			System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged +=
				new EventHandler(GameStart_VisibleChanged);

			device.PreferredBackBufferHeight = windowHeight;
			device.PreferredBackBufferWidth = windowWidth;

			this.screens = screens;
		}

        void device_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = windowsIntPtr;
        }
        private void GameStart_VisibleChanged(object sender, EventArgs e)
        {
                if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
					System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
        }

		protected override void Initialize() {
			currentKeyboardState = previousKeyboardState = Keyboard.GetState();
			currentMouseState = previousMouseState = Mouse.GetState();

			int particles = MaxParticles;
			try {
				particles = Int32.Parse(System.IO.File.ReadAllText(FILE_SETTING_NAME));
			}
			catch (Exception) {
				try {
					System.IO.File.WriteAllText(FILE_SETTING_NAME, MaxParticles.ToString());
				}
				catch (Exception) {
					System.Diagnostics.Debug.WriteLine("Can't create new settings file!");
				}
			}

			snowSource = new ParticleSnowflakeSource(this, particles * (screens.Keys.Count + 1));
			ParticleSnowflake.ParticleSource = snowSource;
			Components.Add(snowSource);

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
#if DEBUG
			fontDebug = Content.Load<SpriteFont>(FONT_DEBUG_ASSET_NAME);
#else
			screenCapturesTexture = new Texture2D[screens.Count];
			for(int index = 0; index < screens.Count; index++) {
				screenCapturesTexture[index] = BitmapToTexture2D(GraphicsDevice, screens.Skip(index).First().Value);
			}
#endif
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime) {
			previousMouseState = currentMouseState;
			previousKeyboardState = currentKeyboardState;
			currentMouseState = Mouse.GetState();
			currentKeyboardState = Keyboard.GetState();


			if (currentKeyboardState.IsKeyDown(Keys.Add) &&
				!previousKeyboardState.IsKeyDown(Keys.Add))
				snowSource.ActivateParticles(10);

#if !DEBUG
			if(currentKeyboardState != previousKeyboardState ||
				Math.Abs(previousMouseState.X - currentMouseState.X) > 3 || 
				Math.Abs(previousMouseState.Y - currentMouseState.Y) > 3)
				Exit();
#endif

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

			for(int index = 0; index < screens.Count; index++) {
				System.Windows.Forms.Screen screen = screens.Skip(index).First().Key;
				spriteBatch.Draw(screenCapturesTexture[index], new Vector2(screen.Bounds.X, screen.Bounds.Y), Color.White);
			}

#if DEBUG
			spriteBatch.DrawString(fontDebug, "Window handle: " + windowsIntPtr.ToString(), new Vector2(20f), Color.White);
			spriteBatch.DrawString(fontDebug, snowSource.ToString(), new Vector2(20f, 50f), Color.White);
#endif

			spriteBatch.End();

			base.Draw(gameTime);
		}

		// This is used for transformation of screen capture to usable texture
#if !DEBUG
		public static Texture2D BitmapToTexture2D(
				GraphicsDevice GraphicsDevice,
				System.Drawing.Bitmap image) {
			// Buffer size is size of color array multiplied by 4 because 
			// each pixel has four color bytes
			int bufferSize = image.Height * image.Width * 4;

			// Create new memory stream and save image to stream so 
			// we don't have to save and read file
			System.IO.MemoryStream memoryStream =
				new System.IO.MemoryStream(bufferSize);
			image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

			// Creates a texture from IO.Stream - our memory stream
			Texture2D texture = Texture2D.FromStream(
				GraphicsDevice, memoryStream);

			return texture;
		}
#endif
	}
}

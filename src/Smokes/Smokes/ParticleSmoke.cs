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
using Snowflake.Core;

namespace Smokes {
	class ParticleSmoke : Particle {
		private const string DEFAULT_TEXTURE_LOCATION =
			"Textures/smoke"; 
		private const int MAX_SMOKE_SIZE = 56;
		private const int MIN_SMOKE_SIZE = 30;
		private const int MAX_ROTATION = 50;
		private const int MIN_ROTATION = 1;
		Color color = Color.LightGray;

		protected static Random Random = new Random();

		public SmokeSource ParticleSource;

		public override GraphicsDevice GraphicsDevice {
			get { return ParticleSource.GraphicsDevice; }
		}
		public override SpriteBatch SpriteBatch { get { return ParticleSource.SpriteBatch; } }


		#region Static texture implementation

		private static string particlesTextureAssetName = DEFAULT_TEXTURE_LOCATION;
		public static string ParticlesTextureAssetName {
			get { return ParticleSmoke.GetParticlesTextureAssetName(); }
			set { ParticleSmoke.SetParticlesTextureAssetName(value); }
		}
		protected static string GetParticlesTextureAssetName() {
			return ParticleSmoke.particlesTextureAssetName;
		}
		protected static void SetParticlesTextureAssetName(string assetName) {
			if(!String.IsNullOrEmpty(assetName))
				ParticleSmoke.particlesTextureAssetName = assetName;
		}
		public override string GetTextureAssetName() {
			return ParticleSmoke.ParticlesTextureAssetName;
		}
		public override void SetTextureAssetName(string assetName) {
			ParticleSmoke.ParticlesTextureAssetName = assetName;
		}

		public static Texture2D particlesTexture;
		public static Texture2D ParticlesTexture {
			get { return GetParticlesTexture(); }
			set { SetParticlesTexture(value); }
		}
		protected static void SetParticlesTexture(Texture2D value) {
			if(value != null) {
				ParticleSmoke.particlesTexture = value;
			}
		}
		private static Texture2D GetParticlesTexture() {
			return ParticleSmoke.particlesTexture;
		}
		public override Texture2D GetTexture() {
			return ParticleSmoke.ParticlesTexture;
		}
		public override void SetTexture(Texture2D texture) {
			ParticleSmoke.ParticlesTexture = texture;
			base.SetTexture(texture);
		}

		#endregion


		public override void BringToLife() {
			BringToLife(Position);
		}
		public override void BringToLife(Vector2 position) {
			BringToLife(position, Velocity);
		}
		public override void BringToLife(Vector2 position, Vector2 velocity) {
			BringToLife(position, velocity, CalculateSize(velocity));
		}
		public override void BringToLife(Vector2 position, Vector2 velocity, Vector2 size) {
			BringToLife(position, velocity, size, CalculateLifetime(velocity, GraphicsDevice.Viewport.Height, position.Y));
		}
		public override void BringToLife(Vector2 position, Vector2 velocity, Vector2 size, int lifetime) {
			Position = position;
			Velocity = velocity;
			Size = size;
			Lifetime = (lifetime <= 0 ? Int32.MaxValue : lifetime);
			ResetLifetimeTotal();

			State = ParticleStates.Live;
		}

		public override void Kill() {
			State = ParticleStates.Dead;
		}

		public override void Update(GameTime gameTime) {
			Rotation += Random.Next(MIN_ROTATION, MAX_ROTATION) / 1000f;

			if(LifetimeProgress * 100 > 50) {
				color.A = (byte)((LifetimeProgress - 0.5) * Byte.MaxValue);
			}

			base.Update(gameTime);
		}
		
		public override void Draw(GameTime gameTime) {
			if (IsLive)
				SpriteBatch.Draw(Texture, Position, null, color, Rotation, Origin, Scale, SpriteEffects.None, 0);
		}

		private Vector2 CalculateSize(Vector2 velocity) {
			float speed = velocity.Y + velocity.X / 2;
			return new Vector2(speed / 2f);
		}
		private int CalculateLifetime(Vector2 velocity, int viewportHeight, float currentHeight) {
			return (int)((viewportHeight - currentHeight) / velocity.Y * 260);
		}

		private static Vector2 GetRandomSize() {
			return new Vector2(Random.Next(MIN_SMOKE_SIZE, MAX_SMOKE_SIZE));
		}

		internal void RandomizeParticle(Vector2 Position) {
			this.Position = Position;
			Size = GetRandomSize();
			Velocity = new Vector2(0, -50);
			Lifetime = CalculateLifetime(Velocity, GraphicsDevice.Viewport.Height, Position.Y);
		}
	}
}

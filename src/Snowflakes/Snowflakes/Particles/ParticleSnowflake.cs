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

namespace Snowflakes.Particles
{
	public class ParticleSnowflake : Particle
	{
		private const string DEFAULT_TEXTURE_LOCATION =
			"Textures/imgSnowflakeTexture";
		private const int MAX_FLAKE_SIZE = 20;
		private const int MIN_FLAKE_SIZE = 7;

		protected static Random Random = new Random();
		public static ParticleSource<ParticleSnowflake> ParticleSource { get; set; }

		public override GraphicsDevice GraphicsDevice {
			get { return ParticleSource.GraphicsDevice; }
		}
		public override SpriteBatch SpriteBatch {
			get { return ParticleSource.SpriteBatch; }
		}

		#region Static texture implementation

		private static string particlesTextureAssetName = DEFAULT_TEXTURE_LOCATION;
		public static string ParticlesTextureAssetName {
			get { return ParticleSnowflake.GetParticlesTextureAssetName(); }
			set { ParticleSnowflake.SetParticlesTextureAssetName(value); }
		}
		protected static string GetParticlesTextureAssetName() {
			return ParticleSnowflake.particlesTextureAssetName;
		}
		protected static void SetParticlesTextureAssetName(string assetName) {
			if (!String.IsNullOrEmpty(assetName))
				ParticleSnowflake.particlesTextureAssetName = assetName;
		}
		public override string GetTextureAssetName() {
			return ParticleSnowflake.ParticlesTextureAssetName;
		}
		public override void SetTextureAssetName(string assetName) {
			ParticleSnowflake.ParticlesTextureAssetName = assetName;
		}

		public static Texture2D particlesTexture;
		public static Texture2D ParticlesTexture {
			get { return GetParticlesTexture(); }
			set { SetParticlesTexture(value); }
		}
		protected static void SetParticlesTexture(Texture2D value) {
			if(value != null)
				ParticleSnowflake.particlesTexture = value;
		}
		private static Texture2D GetParticlesTexture() {
			return ParticleSnowflake.particlesTexture;
		}
		public override Texture2D GetTexture() {
			return ParticleSnowflake.ParticlesTexture;
		}
		public override void SetTexture(Texture2D texture) {
			ParticleSnowflake.ParticlesTexture = texture;			
			base.SetTexture(texture);
		}

		#endregion


		public ParticleSnowflake() {
			
		}
		

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

		public static Vector2 CalculateSize(Vector2 velocity) {
			float speed = velocity.Y + velocity.X / 2;
			return new Vector2(speed / 2f);
		}
		public static int CalculateLifetime(Vector2 velocity, float viewportHeight, float currentHeight) {
			return (int)((viewportHeight - currentHeight) / Math.Abs(velocity.Y) * 260);
		}

		public void RandomizeParticle() {
			Position = GetRandomPosition();
			Size = GetRandomSize();
			Velocity = GetRandomVelocity();
			Lifetime = CalculateLifetime(Velocity, GraphicsDevice.Viewport.Height, Position.Y);
		}
		private Vector2 GetRandomPosition() {
			Vector2 device = new Vector2(
				GraphicsDevice.Viewport.Width, 
				GraphicsDevice.Viewport.Height);
			return new Vector2((float)Random.NextDouble(), 0) * device;
		}
		private Vector2 GetRandomVelocity() {
			int sideNormal = Random.Next(-2, 1) >= 0 ? 1 : -1;
			Vector2 velocityNormal = new Vector2((float)Random.Next(1,20), (float)Random.Next(30,100));
			velocityNormal.Normalize();
			velocityNormal.X *= sideNormal;

			float speed = GetRandomSpeed();
			Vector2 velocity = velocityNormal * speed;
			return velocity;
		}
		private float GetRandomSpeed() {
			return (((float)Math.Pow(10.0, Math.Sqrt(4.0) * Math.Log10((double)((Size.X + Size.Y) / 4f)))) + (((float)Random.NextDouble()) * Random.Next(1, 3)));
		}
		private Vector2 GetRandomSize() {
			return new Vector2(Random.Next(MIN_FLAKE_SIZE, MAX_FLAKE_SIZE));
		}
	}
}
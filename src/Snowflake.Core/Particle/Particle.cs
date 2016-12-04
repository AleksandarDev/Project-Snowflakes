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
using System.Diagnostics.Contracts;

namespace Snowflake.Core {
	public abstract class Particle : IParticleGraphics, IParticleLife, IParticleMovement {
		#region Particle graphics

		#region Variables

		public abstract GraphicsDevice GraphicsDevice { get; }
		public abstract SpriteBatch SpriteBatch { get; }

		public string TextureAssetName {
			get { return GetTextureAssetName(); }
			set { SetTextureAssetName(value); }
		}
		public abstract string GetTextureAssetName();
		public abstract void SetTextureAssetName(string assetName);

		public Texture2D Texture {
			get { return GetTexture(); }
			set { SetTexture(value); }
		}
		public abstract Texture2D GetTexture();
		public virtual void SetTexture(Texture2D texture) {
			RecalculateGraphics();
		}

		private Vector2 scale = Vector2.One;
		public Vector2 Scale {
			get { return GetScale(); }
			protected set {
				if(value.X >= 0f && value.Y >= 0f)
					this.scale = value;
			}
		}
		public virtual Vector2 GetScale() {
			return this.scale;
		}

		protected virtual void CalculateScale() {
			this.Scale = CalculateScale(
				Texture.Width, Texture.Height,
				Size.X, Size.Y);
		}
		protected static Vector2 CalculateScale(Vector2 textureSize, Vector2 particleSize) {
			return CalculateScale(
				textureSize.X, textureSize.Y,
				particleSize.X, particleSize.Y);
		}
		protected static Vector2 CalculateScale(float textureWidth, float textureHeight, float particleWidth, float particleHeight) {
			if(textureWidth == 0 || textureHeight == 0 ||
				particleWidth == 0 || particleHeight == 0)
				return Vector2.One;

			return new Vector2(
				particleWidth / textureWidth,
				particleHeight / textureHeight);
		}

		private Vector2 size = Vector2.Zero;
		public Vector2 Size {
			get { return GetSize(); }
			set { SetSize(value); }
		}
		public virtual void SetSize(Vector2 value) {
			this.size = value;
			RecalculateGraphics();
		}
		public virtual Vector2 GetSize() {
			return this.size;
		}

		private Vector2 origin = Vector2.Zero;
		public Vector2 Origin {
			get { return GetOrigin(); }
			set { SetOrigin(value); }
		}
		public virtual Vector2 GetOrigin() {
			return this.origin;
		}
		public virtual void SetOrigin(Vector2 origin) {
			this.origin = origin;
		}

		protected void CalculateOrigin() {
			Origin = CalculateOrigin(
				Texture.Width, Texture.Height);
		}

		protected static Vector2 CalculateOrigin(int textureWidth, int TextureHeight) {
			if(textureWidth == 0 || TextureHeight == 0)
				return Vector2.One;
			return new Vector2(textureWidth / 2, TextureHeight / 2);
		}

		protected virtual void RecalculateGraphics() {
			CalculateScale();
			CalculateOrigin();
		}



		#endregion

		#region Methods

		public virtual void Draw(GameTime gameTime) {
			if(IsLive)
				SpriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);
		}

		#endregion

		#endregion

		#region Particle movement

		#region Variables

		private Vector2 velocity = Vector2.Zero;
		public Vector2 Velocity {
			get { return GetVelocity(); }
			set { SetVelocity(value); }
		}
		public virtual void SetVelocity(Vector2 value) {
			this.velocity = value;
		}
		public virtual Vector2 GetVelocity() {
			return this.velocity;
		}

		private Vector2 position = Vector2.Zero;
		public Vector2 Position {
			get { return GetPosition(); }
			set { SetPosition(value); }
		}
		public virtual void SetPosition(Vector2 value) {
			this.position = value;
		}
		public virtual Vector2 GetPosition() {
			return this.position;
		}

		private float rotation;
		public float Rotation {
			get { return GetRotation(); }
			set { SetRotation(value); }
		}
		public virtual float GetRotation() {
			return this.rotation;
		}
		public virtual void SetRotation(float rotation) {
			this.rotation = (float)(rotation % (2 * Math.PI));
		}

		#endregion

		#region Methods

		public virtual void Update(GameTime gameTime) {
			if(IsLive && State != ParticleStates.Frozen) {
				this.LifetimeTotal += gameTime.ElapsedGameTime.Milliseconds;
				if(LifetimeTotal >= Lifetime) Kill();

				Position += Velocity / gameTime.ElapsedGameTime.Milliseconds;
			}
		}

		#endregion

		#endregion

		#region Particle life

		#region Variables

		private int lifetime = Int32.MaxValue;
		public int Lifetime {
			get { return GetLifetime(); }
			set { SetLifetime(value); }
		}
		public void SetLifetime(int value) {
			if (value >= 0)
				this.lifetime = value;
		}
		public int GetLifetime() {
			return this.lifetime;
		}

		private int lifetimeTotal = 0;
		public int LifetimeTotal {
			get { return GetLifetimeTotal(); }
			protected set {
				if(value >= 0)
					this.lifetimeTotal = value;
			}
		}
		public void ResetLifetimeTotal() {
			this.lifetimeTotal = 0;
		}
		public int GetLifetimeTotal() {
			return this.lifetimeTotal;
		}

		public float LifetimeProgress {
			get { return GetLifetimeProgress(); }
		}
		public float GetLifetimeProgress() {
			return (float)LifetimeTotal / Lifetime;
		}

		private ParticleStates state = ParticleStates.Dead;
		public ParticleStates State {
			get { return GetState(); }
			protected set { this.state = value;	}
		}
		public ParticleStates GetState() {
			return this.state;
		}

		public bool IsLive {
			get { return State == ParticleStates.Live; }
		}
		public bool IsDead {
			get { return State == ParticleStates.Dead; }
		}

		#endregion
		
		#region Methods

		public abstract void BringToLife();
		public abstract void BringToLife(Vector2 position);
		public abstract void BringToLife(Vector2 position, Vector2 velocity);
		public abstract void BringToLife(Vector2 position, Vector2 velocity, Vector2 size);
		public abstract void BringToLife(Vector2 position, Vector2 velocity, Vector2 size, int lifetime);

		public abstract void Kill();

		#endregion

		#endregion

	}
}

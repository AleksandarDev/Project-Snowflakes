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
using System.Threading.Tasks;

namespace Snowflake.Core
{
	public class ParticleSource<T> : DrawableGameComponent, IParticleSource<T> 
		where T : IParticleGraphics, IParticleLife, IParticleMovement, new() {
		private bool isInitialized = false;
		private SpriteBatch spriteBatch;
		private Queue<T> particleQueue;
		private List<T> particleActive;

		public SpriteBatch SpriteBatch {
			get { return this.spriteBatch; }
		}
		public Queue<T> ParticleQueue {
			get { return this.particleQueue; }
			set { this.particleQueue = value; }
		}
		public List<T> ParticleActive {
			get { return this.particleActive; }
			set { this.particleActive = value; }
		}

		int maxParticles = 0;
		public int MaxParticles {
			get { return maxParticles; }
			set { SetMaxParticles(value); }
		}


		public ParticleSource(Game game, int maxParticles)
			: base(game) {
				MaxParticles = maxParticles;
		}


		public override void Initialize() {
			base.Initialize();
			isInitialized = true;
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			LoadParticleGraphics();

			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {
			//Parallel.ForEach(ParticleActive, (particle) => {
			//    particle.Update(gameTime);
			//});
			foreach(T particle in particleActive) {
				particle.Update(gameTime);
			}
			HandleDeadParticles();
			
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			if(spriteBatch != null) {
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
				foreach(T particle in ParticleActive) {
					particle.Draw(gameTime);
				}
				spriteBatch.End();
			}
			
			base.Draw(gameTime);
		}

		private void LoadParticleGraphics() {
			// Creates dictionary where we store texture locations and textures
			Dictionary<string, Texture2D> loadedTexture =
				new Dictionary<string, Texture2D>();
			// Goese trough all particles that doesn't have loaded texture
			foreach(T particle in ParticleQueue) {
				if(particle.Texture == null) {
					// If we didn't already load a texture for that particle then we 
					// load texture from particle texture location
					if(!loadedTexture.ContainsKey(particle.TextureAssetName))
						loadedTexture.Add(
							particle.TextureAssetName,
							Game.Content.Load<Texture2D>(particle.TextureAssetName));
					// We set particle texture to suitable preloaded texture
					particle.Texture = loadedTexture[particle.TextureAssetName];
				}
			}
		}

		public virtual void SetMaxParticles(int value) {
			this.maxParticles = value;

			if(ParticleQueue == null)
				ParticleQueue = new Queue<T>(value);
			if(ParticleActive == null)
				ParticleActive = new List<T>();

			int toAdd = value - ParticleQueue.Count;
			for(int index = 0; index < toAdd; index++) {
				ParticleQueue.Enqueue(new T());
			}
			if (isInitialized)
				LoadParticleGraphics();
		}

		public virtual void ActivateParticles(int numberOfParticles){
			if(numberOfParticles > ParticleQueue.Count) 
				numberOfParticles = ParticleQueue.Count;
			for(int index = 0; index < numberOfParticles; index++) {
				T particle = ParticleQueue.Dequeue();
				particle.BringToLife();
				ParticleActive.Add(particle);
			}
		}
		public virtual void HandleDeadParticles() {
			for(int index = 0; index < ParticleActive.Count; index++) {
				if(ParticleActive[index].IsDead) {
					ParticleQueue.Enqueue(ParticleActive[index]);
					ParticleActive.RemoveAt(index--);
				}
			}
		}

		public virtual void HandleRunningSlow() {
			int forDequeue = Math.Min(MaxParticles / 1000, ParticleQueue.Count);
			for(int i = 0; i < forDequeue; i++)
				ParticleQueue.Dequeue();
		}

		public override string ToString() {
			return String.Format(
				"ParticleSource<{0}>:\nActive particles: {1}\nParticles in queue: {2}",
				typeof(T).BaseType.ToString(), ParticleActive.Count, ParticleQueue.Count);
		}
	}
}

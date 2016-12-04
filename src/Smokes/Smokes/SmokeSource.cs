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
	class SmokeSource : ParticleSource<ParticleSmoke> {
		public Vector2 Position { get; set; }


		public SmokeSource(Game game, int maxParticles)
			: base(game, maxParticles) {
		}

		public override void Update(GameTime gameTime) {
			ActivateParticles(1);
			
			base.Update(gameTime);
		}


		public override void SetMaxParticles(int value) {
			base.SetMaxParticles(value);
			foreach(ParticleSmoke particle in ParticleQueue) {
				particle.ParticleSource = this;
			}
		}

		public override void ActivateParticles(int numberOfParticles) {
			int particlesStart = ParticleActive.Count;
			base.ActivateParticles(numberOfParticles);
			int particlesEnd = ParticleActive.Count;
			for(int index = ParticleActive.Count - 1; index >= ParticleActive.Count - (particlesEnd - particlesStart); index--) {
				ParticleActive[index].RandomizeParticle(Position);
			}
		}
	}
}

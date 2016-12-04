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

namespace Snowflakes.Particles {
	class ParticleSnowflakeSource : ParticleSource<ParticleSnowflake> {
		private int counter = 0;


		public ParticleSnowflakeSource(Game game, int maxParticles)
			: base(game, maxParticles) {
		}


		public override void Update(GameTime gameTime) {
			int random = 0;
			random = (new Random()).Next(15, Math.Max(20, ParticleQueue.Count / 10));
			if(++counter % random == 0) {
				ActivateParticles(1);
				counter = 0;
			}
			
			base.Update(gameTime);
		}

		public override void ActivateParticles(int numberOfParticles) {
			int particlesStart = ParticleQueue.Count;
			base.ActivateParticles(numberOfParticles);
			int particlesEnd = ParticleQueue.Count;
			for(int index = ParticleActive.Count - 1; index >= ParticleActive.Count - (particlesStart - particlesEnd); index--) {
				ParticleActive[index].RandomizeParticle();
			}
		}

		public override void HandleDeadParticles() {
			int startNumberParticles = ParticleActive.Count;
			base.HandleDeadParticles();
			int endNumberParticles = ParticleActive.Count;
			ActivateParticles(startNumberParticles - endNumberParticles);
		}
	}
}

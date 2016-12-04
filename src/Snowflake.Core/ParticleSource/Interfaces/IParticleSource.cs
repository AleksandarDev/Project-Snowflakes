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

namespace Snowflake.Core {
	public interface IParticleSource<T> {

		SpriteBatch SpriteBatch { get; }

		Queue<T> ParticleQueue { get; set; }
		List<T> ParticleActive { get; set; }

		int MaxParticles { get; set; }
		void SetMaxParticles(int value);

		void ActivateParticles(int numberOfParticles);
		void HandleDeadParticles();

		void HandleRunningSlow();
	}
}

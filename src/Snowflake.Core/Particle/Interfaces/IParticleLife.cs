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

namespace Snowflake.Core
{
	public interface IParticleLife
	{
		#region Variables

		#region Lifetime

		#region Lifetime

		int Lifetime { get; set; }
		void SetLifetime(int value);
		int GetLifetime();

		#endregion

		#region Lifetime Total

		int LifetimeTotal { get; }
		void ResetLifetimeTotal();
		int GetLifetimeTotal();

		#endregion

		#region LifetimeProgress

		float LifetimeProgress { get; }
		float GetLifetimeProgress();

		#endregion

		#endregion

		#region State

		ParticleStates State { get; }
		ParticleStates GetState();

		bool IsLive { get; }
		bool IsDead { get; }

		#endregion

		#endregion

		#region Methods

		#region Life

		void BringToLife();
		void BringToLife(Vector2 position);
		void BringToLife(Vector2 position, Vector2 velocity);
		void BringToLife(Vector2 position, Vector2 velocity, Vector2 size);
		void BringToLife(Vector2 position, Vector2 velocity, Vector2 size, int lifetime);

		void Kill();

		#endregion

		#endregion
	}
}

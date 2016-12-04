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
	public interface IParticleMovement
	{
		#region Variables

		#region Velocity

		Vector2 Velocity { get; set; }
		void SetVelocity(Vector2 value);
		Vector2 GetVelocity();

		#endregion

		#region Position

		Vector2 Position { get; set; }
		void SetPosition(Vector2 value);
		Vector2 GetPosition();

		#endregion

		#region Rotation

		float Rotation { get; set; }
		float GetRotation();
		void SetRotation(float rotation);

		#endregion

		#endregion

		#region Methods

		#region Update

		void Update(GameTime gameTime);

		#endregion

		#endregion
	}
}

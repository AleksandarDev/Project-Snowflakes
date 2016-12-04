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
	public interface IParticleGraphics
	{
		#region Variables

		#region Graphics reference

		GraphicsDevice GraphicsDevice { get; }
		SpriteBatch SpriteBatch { get; }

		#endregion

		#region Texture asset name

		string TextureAssetName { get; set; }
		string GetTextureAssetName();
		void SetTextureAssetName(string assetName);

		#endregion

		#region Texture

		Texture2D Texture { get; set; }
		Texture2D GetTexture();
		void SetTexture(Texture2D texture);

		#endregion

		#region Scale

		Vector2 Scale { get; }
		Vector2 GetScale();

		#endregion

		#region Size

		Vector2 Size { get; set; }
		void SetSize(Vector2 value);
		Vector2 GetSize();

		#endregion

		#region Origin

		Vector2 Origin { get; set; }
		Vector2 GetOrigin();
		void SetOrigin(Vector2 origin);

		#endregion

		#endregion

		#region Methods

		void Draw(GameTime gameTime);

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snowflakes
{
    static class Program
    {
        static void Main(string[] args)
        {
			if (args.Length > 0)
			{
				string firstArgument = args[0].ToLower().Trim();
				string secondArgument = null;
		
				// Handle cases where arguments are separated by colon. 
				// Examples: /c:1234567 or /P:1234567
				if (firstArgument.Length > 2)
				{
					secondArgument = firstArgument.Substring(3).Trim();
					firstArgument = firstArgument.Substring(0, 2);
				}
				else if (args.Length > 1)
					secondArgument = args[1];

				if (firstArgument == "/c")           // Configuration mode
				{
					
				}
				else if (firstArgument == "/p")      // Preview mode
				{
					
				}
				else if (firstArgument == "/s")      // Full-screen mode
				{
					ShowGame();
				}  
				else    // Undefined argument 
				{
					return;
				}
			}
			else    // No arguments
			{
				ShowGame();
			}            
        }
		private static void ShowGame()
		{
			Form gameForm = new Form();
			gameForm.ShowInTaskbar = false;

			Dictionary<Screen, System.Drawing.Bitmap> screens =
				new Dictionary<Screen, System.Drawing.Bitmap>();
#if !DEBUG
			gameForm.FormBorderStyle = FormBorderStyle.None;

			int sumaScreenWidth = 0;
			int maxScreenHeight = 0;
			foreach(Screen screen in Screen.AllScreens) {
				sumaScreenWidth += screen.Bounds.Width;
				maxScreenHeight = Math.Max(maxScreenHeight, screen.Bounds.Bottom);
				screens.Add(screen, CaptureScreen(screen));
			}

			gameForm.Width = sumaScreenWidth;
			gameForm.Height = maxScreenHeight;
			gameForm.TopMost = true;
#else
			gameForm.Width = 1024;
			gameForm.Height = 768;
#endif
			
			GameStart game = new GameStart(gameForm.Handle, gameForm.Width, gameForm.Height, screens);

			gameForm.FormClosed += (sender, e) => game.Exit();
			gameForm.Show();
			
			game.Run();
			game.Dispose();
		}

		private static System.Drawing.Bitmap CaptureScreen(Screen screen) {
			System.Drawing.Bitmap BMP = new System.Drawing.Bitmap(
									screen.Bounds.Width,
									screen.Bounds.Height,
									System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Graphics GFX = System.Drawing.Graphics.FromImage(BMP);
			GFX.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y,
								0, 0,
								screen.Bounds.Size,
								System.Drawing.CopyPixelOperation.SourceCopy);

			return BMP;
		}
    }
}


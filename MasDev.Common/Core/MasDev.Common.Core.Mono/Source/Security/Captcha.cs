using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace MasDev.Security
{
	public static class Captcha
	{
		static readonly Random _random = new Random ();



		public static RandomImage Generate ()
		{
			var random = _random.Next (1000, 9999).ToString ();
			return Generate (random);
		}



		public static RandomImage Generate (string captchaText)
		{
			return new RandomImage (captchaText, 320, 100);
		}
	}





	public class RandomImage
	{
		public RandomImage ()
		{
		}



		public string Text
		{
			get { return _text; }
		}



		public Bitmap Image
		{
			get { return _image; }
		}



		public int Width
		{
			get { return _width; }
		}



		public int Height
		{
			get { return _height; }
		}



		readonly string _text;
		int _width;
		int _height;

		Bitmap _image;

		readonly Random _random = new Random ();



		public RandomImage (string s, int width, int height)
		{
			_text = s;
			SetDimensions (width, height);
			GenerateImage ();
		}



		public void Dispose ()
		{
			GC.SuppressFinalize (this);
			Dispose (true);
		}



		protected virtual void Dispose (bool disposing)
		{
			if (disposing)
				_image.Dispose ();
		}



		void SetDimensions (int width, int height)
		{
			if (width <= 0)
				throw new ArgumentOutOfRangeException ("width", width, 
					"Argument out of range, must be greater than zero.");
			if (height <= 0)
				throw new ArgumentOutOfRangeException ("height", height, 
					"Argument out of range, must be greater than zero.");
			_width = width;
			_height = height;
		}



		void GenerateImage ()
		{
			var bitmap = new Bitmap (_width, _height, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage (bitmap);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			var rect = new Rectangle (0, 0, _width, _height);
			var hatchBrush = new HatchBrush (HatchStyle.Trellis, Color.Black, Color.White);
			g.FillRectangle (hatchBrush, rect);
			SizeF size;
			float fontSize = rect.Height + 1;
			Font font;

			do
			{
				fontSize--;
				font = new Font (FontFamily.GenericMonospace, fontSize, FontStyle.Bold);
				size = g.MeasureString (_text, font);
			} while (size.Width > rect.Width - rect.Width * 0.2f);

			WriteText (g, font, rect);
		
			int m = Math.Max (rect.Width, rect.Height);
			for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
			{
				int x = _random.Next (rect.Width);
				int y = _random.Next (rect.Height);
				int w = _random.Next (m / 50);
				int h = _random.Next (m / 50);
				g.FillEllipse (hatchBrush, x, y, w, h);
			}

			font.Dispose ();
			hatchBrush.Dispose ();
			g.Dispose ();
			_image = bitmap;
		}



		void WriteText (Graphics g, Font font, Rectangle rect)
		{
			var rand = new Random ();
			var textBrush = new SolidBrush (Color.DarkGray);
			const float tik = 1;
			var xSoFar = tik;
			var i = 0;
			foreach (var character in _text)
			{
				var letter = character.ToString ();
				var txt = g.MeasureString (letter, font);
				var y = rand.Next (0, i++ % 2 == 0 ? 10 : 30);
				g.DrawString (letter, font, textBrush, xSoFar, y);
				xSoFar += (txt.Width + tik);
			}
		}
	}
}


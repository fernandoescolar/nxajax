/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
/// nxAjax Util Objects
/// </summary>
namespace nxAjax.Utils
{
    /// <summary>
    /// CaptchaImage Generator and use utility
    /// </summary>
    public class CaptchaImage
    {

        private int mHeight;
        private int mWidth;
        private Random mRand;
        private DateTime mGeneratedAt;
        private string mRandomText;
        private int mRandomTextLength;
        private string mRandomTextChars;
        private string mFontFamilyName;
        private FontWarpFactor mFontWarp;
        private BackgroundNoiseLevel mBackgroundNoise;
        private LineNoiseLevel mLineNoise;
        private string mGuid;
        private string mFontWhitelist;

        #region "Public Enums"

        /// <summary>
        /// Amount of random font warping to apply to rendered text
        /// </summary>
        public enum FontWarpFactor
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// Amount of background noise to add to rendered image
        /// </summary>
        public enum BackgroundNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// Amount of curved line noise to add to rendered image
        /// </summary>
        public enum LineNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        #endregion

        #region "Public Properties"

        /// <summary>
        /// Returns a GUID that uniquely identifies this Captcha
        /// </summary>
        public string UniqueId
        {
            get { return mGuid; }
        }

        /// <summary>
        /// Returns the date and time this image was last rendered
        /// </summary>
        public DateTime RenderedAt
        {
            get { return mGeneratedAt; }
        }

        /// <summary>
        /// Font family to use when drawing the Captcha text. If no font is provided, a random font will be chosen from the font whitelist for each character.
        /// </summary>
        public string Font
        {
            get { return mFontFamilyName; }
            set
            {
                try
                {
                    Font font1 = new Font(value, 12f);
                    mFontFamilyName = value;
                    font1.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    mFontFamilyName = FontFamily.GenericSerif.Name;
                }
            }
        }

        /// <summary>
        /// Amount of random warping to apply to the Captcha text.
        /// </summary>
        public FontWarpFactor FontWarp
        {
            get { return mFontWarp; }
            set { mFontWarp = value; }
        }

        /// <summary>
        /// Amount of background noise to apply to the Captcha image.
        /// </summary>
        public BackgroundNoiseLevel BackgroundNoise
        {
            get { return mBackgroundNoise; }
            set { mBackgroundNoise = value; }
        }

        /// <summary>
        /// Amount of line noise to apply to the Captcha image.
        /// </summary>
        public LineNoiseLevel LineNoise
        {
            get { return mLineNoise; }
            set { mLineNoise = value; }
        }

        /// <summary>
        /// A string of valid characters to use in the Captcha text. 
        /// A random character will be selected from this string for each character.
        /// </summary>
        public string TextChars
        {
            get { return mRandomTextChars; }
            set
            {
                mRandomTextChars = value;
                mRandomText = GenerateRandomText();
            }
        }

        /// <summary>
        /// Number of characters to use in the Captcha text. 
        /// </summary>
        public int TextLength
        {
            get { return mRandomTextLength; }
            set
            {
                mRandomTextLength = value;
                mRandomText = GenerateRandomText();
            }
        }

        /// <summary>
        /// Returns the randomly generated Captcha text.
        /// </summary>
        public string Text
        {
            get { return mRandomText; }
        }

        /// <summary>
        /// Width of Captcha image to generate, in pixels 
        /// </summary>
        public int Width
        {
            get { return mWidth; }
            set
            {
                if ((value <= 60))
                {
                    throw new ArgumentOutOfRangeException("width", value, "width must be greater than 60.");
                }
                mWidth = value;
            }
        }

        /// <summary>
        /// Height of Captcha image to generate, in pixels 
        /// </summary>
        public int Height
        {
            get { return mHeight; }
            set
            {
                if (value <= 30)
                {
                    throw new ArgumentOutOfRangeException("height", value, "height must be greater than 30.");
                }
                mHeight = value;
            }
        }

        /// <summary>
        /// A semicolon-delimited list of valid fonts to use when no font is provided.
        /// </summary>
        public string FontWhitelist
        {
            get { return mFontWhitelist; }
            set { mFontWhitelist = value; }
        }

        #endregion

        /// <summary>
        /// Creates a new CaptchaImage utility object
        /// </summary>
        public CaptchaImage()
        {
            mRand = new Random();
            mFontWarp = FontWarpFactor.Low;
            mBackgroundNoise = BackgroundNoiseLevel.Low;
            mLineNoise = LineNoiseLevel.None;
            mWidth = 180;
            mHeight = 50;
            mRandomTextLength = 5;
            mRandomTextChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
            mFontFamilyName = "";
            // -- a list of known good fonts in on both Windows XP and Windows Server 2003
            mFontWhitelist = "arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;" + "georgia;lucida console;lucida sans unicode;mangal;microsoft sans serif;palatino linotype;" + "sylfaen;tahoma;times new roman;trebuchet ms;verdana";
            mRandomText = GenerateRandomText();
            mGeneratedAt = DateTime.Now;
            mGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        public Bitmap RenderImage()
        {
            return GenerateImagePrivate();
        }

        /// <summary>
        /// Returns a random font family from the font whitelist
        /// </summary>
        private string RandomFontFamily()
        {
            //-- small optimization so we don't have to split for each char
            if (static_RandomFontFamily_ff == null)
            {
                static_RandomFontFamily_ff = mFontWhitelist.Split(';');
            }
            return static_RandomFontFamily_ff[mRand.Next(0, static_RandomFontFamily_ff.Length)];
        }
        static string[] static_RandomFontFamily_ff = null;

        /// <summary>
        /// generate random text for the CAPTCHA
        /// </summary>
        private string GenerateRandomText()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(mRandomTextLength);
            int maxLength = mRandomTextChars.Length;
            for (int n = 0; n <= mRandomTextLength - 1; n++)
            {
                sb.Append(mRandomTextChars.Substring(mRand.Next(maxLength), 1));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        private PointF RandomPoint(int xmin, int xmax, ref int ymin, ref int ymax)
        {
            return new PointF(mRand.Next(xmin, xmax), mRand.Next(ymin, ymax));
        }

        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        private PointF RandomPoint(Rectangle rect)
        {
            int top = rect.Top;
            int bottom = rect.Bottom;
            return RandomPoint(rect.Left, rect.Width, ref top, ref bottom);
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        private GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);
            return gp;
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size 
        /// </summary>
        private Font GetFont()
        {
            float fsize = 0;
            string fname = mFontFamilyName;
            if (string.IsNullOrEmpty(fname))
            {
                fname = RandomFontFamily();
            }
            switch (this.FontWarp)
            {
                case FontWarpFactor.None:
                    fsize = Convert.ToInt32(mHeight * 0.7);
                    break;
                case FontWarpFactor.Low:
                    fsize = Convert.ToInt32(mHeight * 0.8);
                    break;
                case FontWarpFactor.Medium:
                    fsize = Convert.ToInt32(mHeight * 0.85);
                    break;
                case FontWarpFactor.High:
                    fsize = Convert.ToInt32(mHeight * 0.9);
                    break;
                case FontWarpFactor.Extreme:
                    fsize = Convert.ToInt32(mHeight * 0.95);
                    break;
            }
            return new Font(fname, fsize, FontStyle.Bold);
        }

        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        private Bitmap GenerateImagePrivate()
        {
            Font fnt = null;
            Rectangle rect = default(Rectangle);
            Brush br = default(Brush);
            Bitmap bmp = new Bitmap(mWidth, mHeight, PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //-- fill an empty white rectangle
            rect = new Rectangle(0, 0, mWidth, mHeight);
            br = new SolidBrush(Color.White);
            gr.FillRectangle(br, rect);

            int charOffset = 0;
            double charWidth = mWidth / mRandomTextLength;
            Rectangle rectChar = default(Rectangle);

            foreach (char c in mRandomText)
            {
                //-- establish font and draw area
                fnt = GetFont();
                rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), mHeight);

                //-- warp the character
                GraphicsPath gp = TextPath(c.ToString(), fnt, rectChar);
                WarpText(gp, rectChar);

                //-- draw the character
                br = new SolidBrush(Color.Black);
                gr.FillPath(br, gp);

                charOffset += 1;
            }

            AddNoise(gr, rect);
            AddLine(gr, rect);

            //-- clean up unmanaged resources
            fnt.Dispose();
            br.Dispose();
            gr.Dispose();

            return bmp;
        }

        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount
        /// </summary>
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float WarpDivisor = 0;
            float RangeModifier = 0;

            switch (mFontWarp)
            {
                case FontWarpFactor.None:
                    return;
                case FontWarpFactor.Low:
                    WarpDivisor = 6;
                    RangeModifier = 1;
                    break;
                case FontWarpFactor.Medium:
                    WarpDivisor = 5;
                    RangeModifier = 1.3f;
                    break;
                case FontWarpFactor.High:
                    WarpDivisor = 4.5f;
                    RangeModifier = 1.4f;
                    break;
                case FontWarpFactor.Extreme:
                    WarpDivisor = 4;
                    RangeModifier = 1.5f;
                    break;
            }

            RectangleF rectF = default(RectangleF);
            rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / WarpDivisor);
            int wrange = Convert.ToInt32(rect.Width / WarpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * RangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * RangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wrange * RangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hrange * RangeModifier);
            int bottom;

            if (left < 0) left = 0;
            if (top < 0) top = 0;
            if (width > this.Width) width = this.Width;
            if (height > this.Height) height = this.Height;

            bottom = top + hrange;
            PointF leftTop = RandomPoint(left, left + wrange, ref top, ref bottom);
            PointF rightTop = RandomPoint(width - wrange, width, ref top, ref bottom);
            bottom = height - hrange;
            PointF leftBottom = RandomPoint(left, left + wrange, ref bottom, ref height);
            PointF rightBottom = RandomPoint(width - wrange, width, ref bottom, ref height);

            PointF[] points = new PointF[] { leftTop, rightTop, leftBottom, rightBottom };
            Matrix m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }

        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        private void AddNoise(Graphics graphics1, Rectangle rect)
        {
            int density = 0;
            int size = 0;

            switch (mBackgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
            }

            SolidBrush br = new SolidBrush(Color.Black);
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
            {
                graphics1.FillEllipse(br, mRand.Next(rect.Width), mRand.Next(rect.Height), mRand.Next(max), mRand.Next(max));
            }
            br.Dispose();
        }

        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>
        private void AddLine(Graphics graphics1, Rectangle rect)
        {

            int length = 0;
            float width = 0;
            int linecount = 0;

            switch (mLineNoise)
            {
                case LineNoiseLevel.None:
                    return;
                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(mHeight / 31.25);
                    // 1.6
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(mHeight / 27.7777);
                    // 1.8
                    linecount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(mHeight / 25);
                    // 2.0
                    linecount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(mHeight / 22.7272);
                    // 2.2
                    linecount = 3;
                    break;
            }

            PointF[] pf = new PointF[length + 1];
            Pen p = new Pen(Color.Black, width);

            for (int l = 1; l <= linecount; l++)
            {
                for (int i = 0; i <= length; i++)
                {
                    pf[i] = RandomPoint(rect);
                }
                graphics1.DrawCurve(p, pf, 1.75f);
            }

            p.Dispose();
        }

    }
}

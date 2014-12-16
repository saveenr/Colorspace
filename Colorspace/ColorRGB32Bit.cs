namespace Colorspace
{
    public struct ColorRGB32Bit
    {
        private readonly byte _alpha;
        private readonly byte _r;
        private readonly byte _g;
        private readonly byte _b;

        public ColorRGB32Bit(byte alpha, byte r, byte g, byte b)
        {
            _alpha = alpha;
            _r = r;
            _g = g;
            _b = b;
        }

        public ColorRGB32Bit(byte r, byte g, byte b) :
            this( 0xff, r,g,b)
        {
        }

        public ColorRGB32Bit(short a, short r, short g, short b)
        {
            _alpha = (byte)a;
            _r = (byte)r;
            _g = (byte)g;
            _b = (byte)b;
        }

        public ColorRGB32Bit(byte alpha, ColorRGB32Bit c) :
            this( alpha, c.R, c.G, c.B)
        {
        }


        public ColorRGB32Bit(short r, short g, short b):
            this((byte)0xff, (byte)r, (byte)g, (byte)b)
        {
        }

        public ColorRGB32Bit(int rgb) :
            this((uint)rgb)
        {
        }

        public ColorRGB32Bit(uint rgb)
        {
            this._alpha = (byte)((rgb & 0xff000000) >> 24);
            this._r = (byte)((rgb & 0x00ff0000) >> 16);
            this._g = (byte)((rgb & 0x0000ff00) >> 8);
            this._b = (byte)((rgb & 0x000000ff) >> 0);
        }

        public ColorRGB32Bit(ColorRGB color)
        {
            this._alpha = (byte)(System.Math.Round(color.Alpha * 255));
            this._r = (byte)(System.Math.Round(color.R * 255));
            this._g = (byte)(System.Math.Round(color.G * 255));
            this._b = (byte)(System.Math.Round(color.B * 255));
        }

        public byte Alpha
        {
            get { return _alpha; }
        }

        public byte R
        {
            get { return _r; }
        }

        public byte G
        {
            get { return _g; }
        }

        public byte B
        {
            get { return _b; }
        }

        public override string ToString()
        {
            var s = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}({1},{2},{3},{4})",
                                  typeof(ColorRGB32Bit).Name, this._alpha, this._r, this._g, this._b);
            return s;
        }

        public static explicit operator int(ColorRGB32Bit color)
        {
            return color.ToInt();
        }

        public static explicit operator ColorRGB32Bit(int rgbint)
        {
            return new ColorRGB32Bit(rgbint);
        }

        public string ToWebColorString()
        {
            if (this.Alpha == 0xff)
            {
                const string format_string_rgb = "#{0:x2}{1:x2}{2:x2}";
                string color_string = string.Format(System.Globalization.CultureInfo.InvariantCulture, format_string_rgb,
                                                    this.R, this.G, this.B);
                return color_string;
            }
            else
            {
                const string format_string_rgba = "#{0:x2}{1:x2}{2:x2}{3:x2}";
                string color_string = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                                    format_string_rgba, this.Alpha, this.R, this.G, this.B);
                return color_string;
            }
        }

        public override bool Equals(object other)
        {
            return other is ColorRGB32Bit && Equals((ColorRGB32Bit)other);
        }

        public static bool operator ==(ColorRGB32Bit lhs, ColorRGB32Bit rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ColorRGB32Bit lhs, ColorRGB32Bit rhs)
        {
            return !lhs.Equals(rhs);
        }

        private bool Equals(ColorRGB32Bit other)
        {
            return (this._alpha == other._alpha && this._r == other._r && this._g == other._g && this._b == other._b);
        }

        public override int GetHashCode()
        {
            return ToInt();
        }

        public int ToInt()
        {
            return (int)this.ToUInt();
        }

        public uint ToUInt()
        {
            return (uint)((this._alpha << 24) | (this._r << 16) | (this._g << 8) | (this._b));
        }

        /// <summary>
        /// Parses a web color string of form "#ffffff"
        /// </summary>
        /// <param name="webcolor"></param>
        /// <returns></returns>
        public static ColorRGB32Bit ParseWebColorString(string webcolor)
        {
            var outputcolor = TryParseWebColorString(webcolor);
            if (!outputcolor.HasValue)
            {
                string s = string.Format("Failed to parse color string \"{0}\"", webcolor);
                throw new ColorException(s);
            }

            return outputcolor.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// Sample usage:
        ///
        /// System.Drawing.Color c;
        /// bool result = TryParseRGBWebColorString("#ffffff", ref c);
        /// if (result)
        /// {
        ///    //it was correctly parsed
        /// }
        /// else
        /// {
        ///    //it was not correctly parsed
        /// }
        ///
        /// </example>
        /// <param name="webcolor"></param>
        ///<returns></returns>
        public static ColorRGB32Bit? TryParseWebColorString(string webcolor)
        {
            // fail if string is null
            if (webcolor == null)
            {
                return null;
            }

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }


            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // strip leading # if it is there
            while (webcolor.StartsWith("#"))
            {
                webcolor = webcolor.Substring(1);
            }

            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // fail if string doesn't have exactly 6 digits (this means alpha was not provided)
            if (webcolor.Length != 6)
            {
                return null;
            }

            int current_color = 0;
            bool result = System.Int32.TryParse(webcolor, System.Globalization.NumberStyles.HexNumber, null, out current_color);

            if (webcolor.Length == 6)
            {
                // only six digits were given so add alpha
                current_color = (int)(((uint)current_color) | ((uint) 0xff000000));
            }

            if (!result)
            {
                // fail if parsing didn't work
                return null;
            }

            // at this point parsing worked

            // the integer value is converted directly to an rgb value

            var the_color = new ColorRGB32Bit(current_color);
            return the_color;
        }


    }
}
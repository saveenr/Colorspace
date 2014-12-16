using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS = Colorspace;

namespace ColorspaceTest
{
    [TestClass]
    public class TestColors
    {
        private static CS.RGBWorkingSpaces spaces = new CS.RGBWorkingSpaces();
        private static CS.RGBWorkingSpace SRGB_D65_2 = spaces.SRGB_D65_Degree2;

        public void test_rgb_to_hsl(double r, double g, double b, double h, double s, double l)
        {
            var rgb = new CS.ColorRGB(r, g, b);
            var hsl = new CS.ColorHSL(rgb);
            double delta = 0.0000000000000001;
            Assert.AreEqual(h, hsl.H, delta);
            Assert.AreEqual(s, hsl.S, delta);
            Assert.AreEqual(l, hsl.L, delta);
        }

        public void test_rgb_to_hsv(double r, double g, double b, double h, double s, double v)
        {
            var rgb = new CS.ColorRGB(r, g, b);
            var hsv = new CS.ColorHSV(rgb);
            double delta = 0.0000000000000001;
            Assert.AreEqual(h, hsv.H, delta);
            Assert.AreEqual(s, hsv.S, delta);
            Assert.AreEqual(v, hsv.V, delta);
        }

        [TestMethod]
        public void TestRGBToHSLHSV()
        {
            test_rgb_to_hsl(1.0, 0.0, 0.0, 0.0, 1.0, 0.5);
            test_rgb_to_hsl(0.5, 1.0, 0.5, 120.0/360.0, 1.0, 0.75);
            test_rgb_to_hsl(0.0, 0.0, 0.5, 240.0/360.0, 1.0, 0.25);

            test_rgb_to_hsv(1.0, 0.0, 0.0, 0.0, 1.0, 1.0);
            test_rgb_to_hsv(0.5, 1.0, 0.5, 120.0/360.0, 0.5, 1.0);
            test_rgb_to_hsv(0.0, 0.0, 0.5, 240.0/360.0, 1.0, 0.5);
        }

        private static void RoundTrip(CS.ColorRGB rgb, System.Func<CS.ColorRGB,CS.ColorRGB> fx , double delta)
        {
            var c1 = fx(rgb);

            Assert.AreEqual(rgb.R, c1.R, delta);
            Assert.AreEqual(rgb.G, c1.G, delta);
            Assert.AreEqual(rgb.B, c1.B, delta);
            
        }

        private static CS.ColorRGB rtxyz(CS.ColorRGB c0)
        {
            var c1 = new CS.ColorXYZ(c0, SRGB_D65_2);
            var c2 = new CS.ColorRGB(c1, SRGB_D65_2);
            return c2;
        }

        private static CS.ColorRGB rtlab(CS.ColorRGB c0)
        {
            var c1 = new CS.ColorXYZ(c0, SRGB_D65_2);
            var c2 = new CS.ColorLAB(c1, SRGB_D65_2);
            var c3 = new CS.ColorXYZ(c2, SRGB_D65_2);
            var c4 = new CS.ColorRGB(c3, SRGB_D65_2);
            return c4;
        }


        [TestMethod]
        public void rgb_xyz_roundtrip()
        {
            double delta = 0.001;
            RoundTrip(new CS.ColorRGB(1.0, 0.0, 0.0), rtxyz,delta);
            RoundTrip(new CS.ColorRGB(1.0, 1.0, 0.0), rtxyz,delta);
            RoundTrip(new CS.ColorRGB(0.0, 1.0, 0.0), rtxyz,delta);
            RoundTrip(new CS.ColorRGB(0.0, 1.0, 1.0), rtxyz,delta);
            RoundTrip(new CS.ColorRGB(0.0, 0.0, 1.0), rtxyz,delta);
            RoundTrip(new CS.ColorRGB(1.0, 0.0, 1.0), rtxyz,delta);
            RoundTrip(new CS.ColorRGB(0.0, 0.0, 0.0), rtxyz,delta);
        }

        [TestMethod]
        public void rgb_lab_roundtrip()
        {
            double delta = 0.1;

            RoundTrip(new CS.ColorRGB(1.0, 0.0, 0.0), rtlab,delta);
            RoundTrip(new CS.ColorRGB(1.0, 1.0, 0.0), rtlab,delta);
            RoundTrip(new CS.ColorRGB(0.0, 1.0, 0.0), rtlab,delta);
            RoundTrip(new CS.ColorRGB(0.0, 1.0, 1.0), rtlab,delta);
            RoundTrip(new CS.ColorRGB(0.0, 0.0, 1.0), rtlab,delta);
            RoundTrip(new CS.ColorRGB(1.0, 0.0, 1.0), rtlab,delta);
            RoundTrip(new CS.ColorRGB(0.0, 0.0, 0.0), rtlab,delta);
        }



        [TestMethod]
        public void TestADD()
        {
            var c0 = CS.ColorRGB32Bit.ParseWebColorString("#ff0000");
            var c1 = new CS.ColorRGB(c0);
            var hsl = new CS.ColorHSL(c1);
            var hsl2 = hsl.Add(-2.3, 0, 0);
        }

        [TestMethod]
        public void TestColorEquality()
        {
            Assert.AreEqual(new CS.ColorRGB(0.2, 0.4, 0.8), new CS.ColorRGB(0.2, 0.4, 0.8));
        }

        [TestMethod]
        public void TestColorSizes()
        {
            Assert.AreEqual(32, System.Runtime.InteropServices.Marshal.SizeOf(new CS.ColorRGB(0.2, 0.4, 0.8)));
            Assert.AreEqual(32, System.Runtime.InteropServices.Marshal.SizeOf(new CS.ColorHSL(0.2, 0.4, 0.8)));
            Assert.AreEqual(32, System.Runtime.InteropServices.Marshal.SizeOf(new CS.ColorHSV(0.2, 0.4, 0.8)));
            Assert.AreEqual(32, System.Runtime.InteropServices.Marshal.SizeOf(new CS.ColorXYZ(0.2, 0.4, 0.8)));
            Assert.AreEqual(32, System.Runtime.InteropServices.Marshal.SizeOf(new CS.ColorLAB(0.2, 0.4, 0.8)));
            Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(new CS.ColorRGB32Bit(0, 0, 0)));
        }

        [TestMethod]
        public void TestHueRotation_HSL()
        {
            // Spin 0xff0000 around by hue +1.0 - it should end up in 0x00ffff
            var c1 = new CS.ColorRGB32Bit(255, 0, 0);
            var c2 = new CS.ColorRGB(c1);
            var c3 = new CS.ColorHSL(c2);
            var c4 = new CS.ColorHSL(0.5, c3.S, c3.L);
            var c5 = new CS.ColorRGB(c4);
            var c6 = new CS.ColorRGB32Bit(c5);
            Assert.AreEqual(255, c6.Alpha);
            Assert.AreEqual(0, c6.R);
            Assert.AreEqual(255, c6.G);
            Assert.AreEqual(255, c6.B);
        }

        [TestMethod]
        public void TestHueRotation_HSV()
        {
            // Spin 0xff0000 around by hue +1.0 - it should end up in 0x00ffff
            var c1 = new CS.ColorRGB32Bit(255, 0, 0);
            var c2 = new CS.ColorRGB(c1);
            var c3 = new CS.ColorHSV(c2);
            var c4 = new CS.ColorHSV(0.5, c3.S, c3.V);
            var c5 = new CS.ColorRGB(c4);
            var c6 = new CS.ColorRGB32Bit(c5);
            Assert.AreEqual(255, c6.Alpha);
            Assert.AreEqual(0, c6.R);
            Assert.AreEqual(255, c6.G);
            Assert.AreEqual(255, c6.B);
        }

        [TestMethod]
        public void SpotCheckXYZToLAB()
        {
            var xyz = new CS.ColorXYZ(41.240, 21.260, 1.930);
            var lab = new CS.ColorLAB(xyz, SRGB_D65_2);

            Assert.AreEqual(53.233,lab.L, 0.01);
            Assert.AreEqual(67.22, lab.B, 0.01);
            Assert.AreEqual(80.10, lab.A, 0.01);
        }

        [TestMethod]
        public void SpotCheckLABToXYZ()
        {
            var lab = new CS.ColorLAB(53.233, 80.10, 67.22);
            var xyz = new CS.ColorXYZ(lab, SRGB_D65_2);

            Assert.AreEqual(41.240, xyz.X, 0.01);
            Assert.AreEqual(21.260, xyz.Y, 0.01);
            Assert.AreEqual(1.930,  xyz.Z, 0.01);
        }

        [TestMethod]
        public void SpotCheckRGBToXYZ()
        {
            var xyz = new CS.ColorXYZ(new CS.ColorRGB(1, 0, 0), SRGB_D65_2);

            Assert.AreEqual(41.240, xyz.X, 0.01);
            Assert.AreEqual(21.260, xyz.Y, 0.01);
            Assert.AreEqual(1.930,  xyz.Z, 0.01);
        }

        [TestMethod]
        public void SpotCheckRGBToLAB()
        {
            var xyz = new CS.ColorXYZ(new CS.ColorRGB(1, 0, 0), SRGB_D65_2);
            var lab = new CS.ColorLAB(xyz, SRGB_D65_2);

            Assert.AreEqual(53.233, lab.L, 0.1);
            Assert.AreEqual(80.10,  lab.A, 0.1);
            Assert.AreEqual(67.22,  lab.B, 0.1);
        }

        [TestMethod]
        public void SpotCheckLABToRGB()
        {
            var lab = new CS.ColorLAB(53.233, 80.10, 67.22);
            var xyz = new CS.ColorXYZ(lab, SRGB_D65_2);
            var rgb = new CS.ColorRGB(xyz, SRGB_D65_2);

            Assert.AreEqual(1, rgb.R, 0.01);
            Assert.AreEqual(0, rgb.G, 0.01);
            Assert.AreEqual(0, rgb.G, 0.01);

        }

        [TestMethod]
        public void SpotCheckXYZToRGB()
        {
            var xyz = new CS.ColorXYZ(41.240, 21.260, 1.930);
            var rgb = new CS.ColorRGB(xyz, SRGB_D65_2);

            Assert.AreEqual(1, rgb.R, 0.01);
            Assert.AreEqual(0, rgb.G, 0.01);
            Assert.AreEqual(0, rgb.G, 0.01);
        }

        [TestMethod]
        public void SpotCheckXYZToRGB2()
        {
            var xyz = new CS.ColorXYZ(87.54, 92.879, 107.903);
            var rgb = new CS.ColorRGB(xyz, SRGB_D65_2);

            Assert.AreEqual(0.9411, rgb.R, 0.01);
            Assert.AreEqual(0.97257, rgb.G, 0.01);
            Assert.AreEqual(0.99992, rgb.B, 0.01);
        }

        [TestMethod]
        public void SpotCheckXYZToRGB3()
        {
            var xyz = new CS.ColorXYZ(95.050, 100.00, 108.900);
            var rgb = new CS.ColorRGB(xyz, SRGB_D65_2);

            Assert.AreEqual(1,          rgb.R, 0.01);
            Assert.AreEqual(1,          rgb.G, 0.01);
            Assert.AreEqual(0.99992,    rgb.B, 0.01);
        }
    }
}
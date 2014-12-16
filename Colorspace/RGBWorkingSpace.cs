namespace Colorspace
{
    public class RGBWorkingSpace
    {
        public string Name { get; private set; }
        public Illuminant ReferenceWhite { get; private set; }
        public double[,] XYZToRGBMatrix { get; private set; }
        public double[,] RGBTOXYZMatrix { get; private set; }

        public RGBWorkingSpace(string name, Illuminant refwhite, double[,] xyz_to_rgb, double[,] rgb_to_xyz)
        {
            this.Name = name;
            this.ReferenceWhite = refwhite;
            this.XYZToRGBMatrix = xyz_to_rgb;
            this.RGBTOXYZMatrix = rgb_to_xyz;
        }
    }
}
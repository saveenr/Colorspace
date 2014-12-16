namespace Colorspace
{
    public class Illuminant
    {
        public string Name { get; private set; }
        public ColorXYZ ColorXYZ { get; private set; }
        public int Degree { get; private set; }

        public Illuminant(string name, int degree, ColorXYZ xyz)
        {
            this.Name = name;
            this.Degree = degree;
            this.ColorXYZ = xyz;
        }
        
    }
}
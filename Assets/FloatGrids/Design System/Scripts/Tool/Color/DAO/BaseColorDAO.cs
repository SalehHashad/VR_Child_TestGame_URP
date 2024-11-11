namespace floatgrids
{
    [System.Serializable]
    public class BaseColorDAO
    {
        public string name;
        public float[] color;

        public BaseColorDAO(BaseColor baseColor)
        {
            name = baseColor.Name;
            color = new float[4];
            color[0] = baseColor.Color.r;
            color[1] = baseColor.Color.g;
            color[2] = baseColor.Color.b;
            color[3] = baseColor.Color.a;
        }
    }
}

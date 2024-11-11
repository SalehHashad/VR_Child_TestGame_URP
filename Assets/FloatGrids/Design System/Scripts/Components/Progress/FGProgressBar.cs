namespace floatgrids
{
    public class FGProgressBar : FGSlider
    {
        public void UpdateValue(float value)
        {
            _slider.value = value;
        }
    }
}
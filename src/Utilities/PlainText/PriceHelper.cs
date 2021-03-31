namespace Utilities.PlainText
{
    public static class PriceHelper
    {
        public static string GetDisplayedPrice(decimal price)
        {
            return price.ToString("N2");
        }
    }
}

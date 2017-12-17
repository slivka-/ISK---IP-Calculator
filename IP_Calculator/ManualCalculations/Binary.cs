namespace IP_Calculator.ManualCalculations
{
    static class Binary
    {
        public static string byte2String(byte b)
        {
            string bin = string.Empty;
            byte value = 128;
            byte shift = 7;
            while (value > 0)
            {
                bin += ((b & value)>>shift).ToString();
                value /= 2;
                --shift;
            }
            return bin;
        }
    }
}

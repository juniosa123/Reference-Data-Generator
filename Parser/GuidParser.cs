using System;

namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Parser
{
    public class GuidParser
    {
        public static string ParseOracleRaw(string text)
        {
            byte[] bytes = ParseHex(text);
            Guid newGuid = new Guid(bytes);
            return Guid.Parse(newGuid.ToString()).ToString();
        }

        public static byte[] ParseHex(string text)
        {
            // Not the most efficient code in the world, but
            // it works...
            byte[] ret = new byte[text.Length / 2];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
            }
            return ret;
        }
        public static string GuidToRaw(Guid guid)
        {
            return BitConverter.ToString(guid.ToByteArray()).Replace("-", "");
        }

    }
}

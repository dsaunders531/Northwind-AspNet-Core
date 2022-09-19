namespace tools.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Encrypt a string with the provider.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encryptionProvider"></param>
        /// <returns></returns>
        public static string Encrypt(this string value, IEncryptionProvider encryptionProvider)
        {
            return encryptionProvider.Encrypt(value);
        }

        /// <summary>
        /// Decrypt a string with the specified provider. Use the same provider you encypted the string with.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encryptionProvider"></param>
        /// <returns></returns>
        public static string Decrypt(this string value, IEncryptionProvider encryptionProvider)
        {
            return encryptionProvider.Decrypt(value);
        }
    }
}

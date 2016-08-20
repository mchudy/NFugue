using System;
using NFugue.Staccato.Subparsers;

namespace NFugue.Providers
{
    public class KeyProviderFactory
    {
        private static readonly Lazy<IKeyProvider> keyProvider = new Lazy<IKeyProvider>(() => new SignatureSubparser());

        public static IKeyProvider GetKeyProvider()
        {
            return keyProvider.Value;
        }
    }
}

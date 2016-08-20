using NFugue.Providers;
using Staccato.Subparsers;
using System;

namespace Staccato
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

using System.Security.Cryptography;

namespace RolandK.TimeTrack.Service.Security.Services;

public class DefaultCspNonceGenerator : ICspNonceGenerator
{
    private readonly Lazy<string> _nonce = new(() =>
    {
        Span<char> nonceBytes = stackalloc char[32];
        RandomNumberGenerator.GetHexString(nonceBytes, true);

        return new string(nonceBytes);
    });
    
    public string GetNonceForCurrentScope()
    {
        return _nonce.Value;
    }
}
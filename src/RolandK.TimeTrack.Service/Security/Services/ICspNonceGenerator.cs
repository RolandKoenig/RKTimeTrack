namespace RolandK.TimeTrack.Service.Security.Services;

public interface ICspNonceGenerator
{
    string GetNonceForCurrentScope();
}
namespace HangarDesk.Final.Application;
internal sealed class AuthorizationException : Exception
{
    public AuthorizationException() : base("Bu işlem için gerekli yetkiye sahip değilsiniz.") { }
}

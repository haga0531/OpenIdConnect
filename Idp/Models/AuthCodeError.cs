namespace Idp.Models;

// https://www.rfc-editor.org/rfc/rfc6749.html#section-4.1.2.1
public enum AuthCodeError
{
    InvalidRequest,
    UnauthorizedClient,
    AccessDenied,
    UnsupportedResponseType,
    InvalidScope,
    ServerError,
    TemporaryUnavailable,
    LoginRequired,
    AccountSelectionRequired,
    ConsentRequired,
    InvalidRequestUri,
    InvalidRequestObject,
    RequestNotSupported,
    RequestUriNotSupported,
    RegistrationNotSupported
}

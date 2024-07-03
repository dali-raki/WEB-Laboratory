namespace Ids.Shared.Exceptions;

public class InvalidAnneeEnseignementException : Exception
{
    public InvalidAnneeEnseignementException(string paramValue) :
        base($"La valeur {paramValue} n'est pas une année universitaire valide")
    {
    }
}
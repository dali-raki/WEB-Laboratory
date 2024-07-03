using System.Collections;
using System.Runtime.Serialization;

namespace Ids.Shared.Exceptions;

[Serializable]
internal class ModelValidationException : Exception
{
    public ModelValidationException()
    {
    }

    public ModelValidationException(string message) : base(message)
    {
    }

    public ModelValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ModelValidationException(string modelName, string message, IDictionary errors)
        : base($"{modelName} : {message}")
    {
        AddData(errors);
    }

    protected ModelValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    private void AddData(IDictionary dictionary)
    {
        if (dictionary != null)
        {
            foreach (DictionaryEntry item in dictionary)
            {
                Data.Add(item.Key, item.Value);
            }
        }
    }

    private void AddData(string key, params string[] values) =>
        Data.Add(key, values.ToList());
}
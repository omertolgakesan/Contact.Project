using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
    public interface IObjectFormatConverter
    {
        T JsonToObject<T>(string jsonString) where T : class, new();
        string ObjectToJson<T>(T entityObject) where T : class, new();
        T ParseObjectDataArray<T>(byte[] rawBytes) where T : class, new();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
    public interface IMapperBase
    {
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source);
    }
}

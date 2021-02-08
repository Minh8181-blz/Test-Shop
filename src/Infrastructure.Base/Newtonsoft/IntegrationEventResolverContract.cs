using Application.Base.SeedWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.Base.Newtonsoft
{
    class IntegrationEventResolverContract : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (typeof(IntegrationEvent).IsAssignableFrom(member.DeclaringType))
            {
                prop.Writable = true;
            }

            return prop;
        }
    }
}

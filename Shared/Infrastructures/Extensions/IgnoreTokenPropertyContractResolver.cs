using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Shared.Features
{
    public class IgnoreTokenPropertyContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (member != null)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                // Ignore Token property
                if (member.Name == "Token" || member.Name == "token")
                {
                    property.ShouldSerialize = instance => false;
                }

                return property;
            }
            else
            {
                return null;
            }
        }
    }
}

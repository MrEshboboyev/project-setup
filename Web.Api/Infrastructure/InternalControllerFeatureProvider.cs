using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Web.Api.Infrastructure;

internal sealed class InternalControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        Assembly assembly = typeof(Program).Assembly;

        foreach (TypeInfo type in assembly.DefinedTypes)
        {
            if (type is { IsClass: true, IsAbstract: false }
                && typeof(ControllerBase).IsAssignableFrom(type)
                && type.IsNotPublic) // internal bo'lishi kerak
            {
                feature.Controllers.Add(type);
            }
        }
    }
}

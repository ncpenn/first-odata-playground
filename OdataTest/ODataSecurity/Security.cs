using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Routing.Conventions;

namespace OdataTest.ODataSecurity
{
    /// <summary>
    /// OData metadata controller which requires authorization.
    /// </summary>
    [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
    public class SecuredMetadataController : MetadataController
    {
    }

    /// <summary>
    /// Convention for $metadata which requires authorization.
    /// </summary>
    public class SecuredMetadataRoutingConvention : MetadataRoutingConvention
    {
        public static void ReplaceMetadataRoutingConvention(ODataOptions opt)
        {
            var toRemove = opt.Conventions.OfType<MetadataRoutingConvention>().FirstOrDefault();
            opt.Conventions.Remove(toRemove);
            opt.Conventions.Insert(0, new SecuredMetadataRoutingConvention());
        }

        private static readonly TypeInfo MetadataTypeInfo = typeof(SecuredMetadataController).GetTypeInfo();

        public override bool AppliesToController(ODataControllerActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // This convention only applies to "SecuredMetadataController".
            return context.Controller.ControllerType == MetadataTypeInfo;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MyNet.Application.Common.Attributes;

namespace MyNet.Infrastructure.Security
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(HasPermissionAttribute.POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var parts = policyName.Substring(HasPermissionAttribute.POLICY_PREFIX.Length)
                    .Split(new[] { HasPermissionAttribute.SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    var function = parts[0];
                    var action = parts[1];

                    var policy = new AuthorizationPolicyBuilder();
                    policy.AddRequirements(new PermissionRequirement(function, action));
                    return Task.FromResult<AuthorizationPolicy?>(policy.Build());
                }
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}

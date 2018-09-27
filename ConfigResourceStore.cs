using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;

namespace IdentityServerDemo
{
    public class ConfigResourceStore : IResourceStore
    {
        private readonly IOptionsMonitor<SecuritySettings> _securitySettings;

        public ConfigResourceStore(IOptionsMonitor<SecuritySettings> securitySettings)
        {
            _securitySettings = securitySettings;
        }
        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns></returns>
        public Task<Resources> GetAllResourcesAsync()
        {
            var security = _securitySettings.CurrentValue;
            var result = new Resources(security.IdentityResources, security.ApiResources);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Finds the API resource by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var api = from a in _securitySettings.CurrentValue.ApiResources
                where a.Name == name
                select a;
            return Task.FromResult(api.FirstOrDefault());
        }

        /// <summary>
        /// Finds the identity resources by scope.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">names</exception>
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));

            var identity = from i in _securitySettings.CurrentValue.IdentityResources
                where names.Contains(i.Name)
                select i;

            return Task.FromResult(identity);
        }

        /// <summary>
        /// Finds the API resources by scope.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">names</exception>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));

            var api = from a in _securitySettings.CurrentValue.ApiResources
                let scopes = (from s in a.Scopes where names.Contains(s.Name) select s)
                where scopes.Any()
                select a;

            return Task.FromResult(api);
        }
    }
}
﻿using System;
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

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var identity = from i in _securitySettings.CurrentValue.IdentityResources.Union(StandardIdentityResources)
                
                where scopeNames.Contains(i.Name)
                select i;

            return Task.FromResult(identity);        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var query =
                from x in _securitySettings.CurrentValue.ApiResources.Where(x => x.s)
                where scopeNames.Contains(x.Name)
                select x;
            
            return Task.FromResult(query);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var api = from a in _securitySettings.CurrentValue.ApiResources
                let scopes = (from s in a.Scopes where scopeNames.Contains(s) select s)
                where scopes.Any()
                select a;

            return Task.FromResult(api);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns></returns>
        public Task<Resources> GetAllResourcesAsync()
        {
            var security = _securitySettings.CurrentValue;
//            var result = new Resources(security.IdentityResources, security.ApiResources);
            var result = new Resources(security.IdentityResources.Union(StandardIdentityResources), security.ApiResources);
            return Task.FromResult(result);
        }

        public List<IdentityResource> StandardIdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Profile()
            };

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

            var identity = from i in _securitySettings.CurrentValue.IdentityResources.Union(StandardIdentityResources)
                
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
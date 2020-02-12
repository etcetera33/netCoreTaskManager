using IdentityServer.Models;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer.Extensions
{
    public static class IIdentityServerExtensions
    {
        internal static IEnumerable<Client> AddInMemoryClientsWithClamis(IConfigurationSection configurationSection)
        {
            var clients = configurationSection.Get<Client[]>();

            if (clients == null)
            {
                return new List<Client>();
            }

            for (int i = 0; i < clients.Length; i++)
            {
                var claimDtos = configurationSection.GetSection(
                    FormattableString.Invariant($"{i}:Claims")).Get<ClaimDto[]>();

                if (claimDtos == null)
                {
                    continue;
                }

                var claims = claimDtos.Select(dto => new Claim(
                    dto.Type,
                    dto.Value,
                    dto.ValueType,
                    dto.Issuer,
                    dto.OriginalIssuer));

                clients[i].Claims.Add(claims);
            }

            return clients;
        }

        private static void Add(this ICollection<Claim> collection, IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                collection.Add(claim);
            }
        }
    }
}

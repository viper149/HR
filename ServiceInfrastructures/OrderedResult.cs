using System;
using System.Collections.Generic;
using System.Linq;

namespace DenimERP.ServiceInfrastructures
{
    public static class OrderedResult<TEntity>
    {
        public static IQueryable<TEntity> GetOrderedResult(string sortColumnDirection, string sortColumn, IReadOnlyList<string> navigationPropertyStrings,
            IQueryable<TEntity> entities)
        {
            switch (sortColumnDirection)
            {
                case "asc" when sortColumn != null && sortColumn.Contains("."):
                    {
                        var substring = sortColumn.Split(".");
                        var enumerable = substring.Skip(substring.Length - 1);

                        var subStrings = sortColumn.Split(".");
                        var x = navigationPropertyStrings.Select(e => e.ToUpperInvariant()).Contains(subStrings[0])
                            ? navigationPropertyStrings[Array.IndexOf(navigationPropertyStrings.Select(f => f.ToUpperInvariant()).ToArray(), subStrings[0])]
                            : string.Empty;

                        entities = entities.OrderBy(c => c.GetType().GetProperty(x).GetValue(c).GetType().GetProperty(subStrings[1]).GetValue(c.GetType().GetProperty(x).GetValue(c)));
                        break;
                    }
                case "asc":
                    entities = entities.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                    break;
                default:
                    {
                        if (sortColumn != null && sortColumn.Contains("."))
                        {
                            var subStrings = sortColumn.Split(".");
                            var x = navigationPropertyStrings.Select(e => e.ToUpperInvariant()).Contains(subStrings[0])
                                ? navigationPropertyStrings[Array.IndexOf(navigationPropertyStrings.Select(f => f.ToUpperInvariant()).ToArray(), subStrings[0])]
                                : string.Empty;

                            entities = entities.OrderByDescending(c => c.GetType().GetProperty(x).GetValue(c).GetType().GetProperty(subStrings[1]).GetValue(c.GetType().GetProperty(x).GetValue(c)));
                        }
                        else
                        {
                            entities = entities.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                        }

                        break;
                    }
            }

            return entities;
        }
    }
}

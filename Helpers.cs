using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advantage.API
{
    public class Helpers
    {
        private static Random _rand = new Random();

        private static string GetRandom(IList<string> items)
        {
            return items[_rand.Next(items.Count)];
        }

        internal static string MakeUniqueCustomerName(List<string> names)
        {
            var maxNames = bizPrefix.Count * bizSuffix.Count;
            if (names.Count >= maxNames)
            {
                throw new System.InvalidOperationException("Max number of unique names exceeded!!!");
            };

            var prefix = GetRandom(bizPrefix);
            var suffix = GetRandom(bizSuffix);
            var bizName = prefix + suffix;
            
            if(names.Contains(bizName))
            {
                MakeUniqueCustomerName(names);
            }
            return bizName;
        }

        internal static string MakeCustomerEmail(string customerName)
        {
            return $"contact@{customerName.ToLower()}.com";
        }

        internal static string GetRandomProvince()
        {
            return GetRandom(saProvinces);
        }

        internal static decimal GetRandomOrderTotal()
        {
            return _rand.Next(100, 5000);
        }

        internal static DateTime GetRandomOrderPlaced()
        {
            var end = DateTime.Now;
            var start = end.AddDays(-90);

            TimeSpan possibleSpan = end - start;
            TimeSpan newSpan = new TimeSpan(0, _rand.Next(0, (int)possibleSpan.TotalMinutes), 0);

            return start + newSpan;
        }

        internal static DateTime? GetRandomOrderCompleted(DateTime orderPlaced)
        {
            var now = DateTime.Now;
            var minLeadTime = TimeSpan.FromDays(7);
            var timePassed = now - orderPlaced;

            if(timePassed < minLeadTime)
            {
                return null;
            }
            return orderPlaced.AddDays(_rand.Next(7, 14));
        }

        private readonly static List<string> saProvinces = new List<string>() 
        { 
            "Gauteng",
            "Mpumalanga",
            "Kwazulu Natal",
            "Free State",
            "Eastern Cape",
            "Western Cape",
            "North-West"
        };

        private readonly static List<string> bizPrefix = new List<string>()
        {
            "Acme",
            "ABC",
            "KayMedia",
            "MTYsquare",
            "Sysinfo",
            "PrimePixesMedia",
            "KIZZ",
            "DiamondInTheRuff",
            "Capone",
            "BunzOut",
            "Cracker"
        };

        private readonly static List<string> bizSuffix = new List<string>()
        {
            "Corporation",
            "Organization",
            "Amalgamated",
            "Entity",
            "Systems",
            "Incorporated",
            "Logistices",
            "Co",
            "Goods",
            "Cleaners",
            "Transit"
        };
    }
}

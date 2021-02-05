using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ExpandoObjectSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var dynamicObject = new ExpandoObject();
            dynamicObject.TryAdd("firstName", "Nilmi");
            dynamicObject.TryAdd("lastName", "Nawalage");
            Console.WriteLine(dynamicObject.Count(c => c.Key == "firstName"));

            string json = @"{""result"":[
                               {
                                  ""firstName"":""Nilmi"",
                                  ""lastName"":""Nawalage""
                               },
                               {
                                  ""firstName"":""Uthpala"",
                                  ""lastName"":""Nawalage""
                               },
                               {
                                  ""firstName"":""Buddika"",
                                  ""lastName"":""Kulasekara""
                               }
                            ]}";

            var jobject = JObject.Parse(json);
            var apiDataAllResult = jobject["result"];

            var list = new List<ExpandoObject>();
            for (int i = 0; i < apiDataAllResult.Count(); i++)
            {
                var item = new ExpandoObject();
                item.TryAdd("firstName", apiDataAllResult[i]["firstName"]);
                item.TryAdd("lastName", apiDataAllResult[i]["lastName"]);
                list.Add(item);
            }

            var dynamicProperty = "lastName";

            var count = list.Count(g => g.Where(o=>o.Key== "firstName").Count()>0);
            var group = list.GroupBy(g => g.Where(o => o.Key == "lastName").Count() > 0).ToList();


            var newGroup= list.GroupBy(g => ((IDictionary<String, Object>)g)[dynamicProperty])
                        .Select(group => new {
                            GroupKey = group.Key,
                            Count = group.Count()
                        }).ToList();

            foreach (var line in newGroup)
            {
                Console.WriteLine("{0} {1}", line.GroupKey, line.Count);
            }

            var groupedItems =
                (from IDictionary<string, object> expando in list
                group expando by  expando[dynamicProperty] into g
                select new { KeyValue = g.Key, Count = g.Count() }).ToList();

            var groupedItemsByMultiple =
                (from IDictionary<string, object> expando in list
                 group expando by new { key1=expando[dynamicProperty], key2=expando ["firstName"]} into g
                select new { dynamicProperty = g.Key, Count = g.Count() }).ToList();


            Console.ReadLine();
        }
    }
}

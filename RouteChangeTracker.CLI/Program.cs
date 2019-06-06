using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RouteChangeTracker.Models;
using RouteChangeTracker.Models.Enums;
using RouteChangeTracker.Processor;

namespace RouteChangeTracker.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var updatedJsonPath = "updated.json";
            var originJsonPath = "origin.json";

            if (!File.Exists(originJsonPath))
            {
                var tree = new Generator().GenerateSimpleRoutes(5).ToList();
                File.WriteAllText(originJsonPath, JsonConvert.SerializeObject(tree));
                File.WriteAllText(updatedJsonPath, JsonConvert.SerializeObject(tree));
            }

            var origin = JsonConvert.DeserializeObject<Route[]>(File.ReadAllText(originJsonPath));
            var updated = JsonConvert.DeserializeObject<Route[]>(File.ReadAllText(updatedJsonPath));

            var logs = new ManualProcessor().ProcessManual(origin, updated);

            File.WriteAllText("result.json", JsonConvert.SerializeObject(logs));
        }
    }
}

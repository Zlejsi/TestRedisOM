using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Modeling;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace ConsoleApp1
{

    public class StaticIncrementStrategy : IIdGenerationStrategy
    {
        private readonly IRedisConnection _db;
        private readonly Type _type;

        public StaticIncrementStrategy(IRedisConnection db, Type type)
        {
            _db = db;
            _type = type;

        }
        public string GenerateId()
        {
            return _db.Execute("INCR", new string[] { _type.Name + ":ID" }).ToString(CultureInfo.InvariantCulture);
        }
    }

    [Document(StorageType = StorageType.Hash, IdGenerationStrategyName = "MyIncrementStrategy", Prefixes = new string[] { nameof(IntID) })]
    public class IntID
    {
        [RedisIdField] public int Id { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new ConfigurationOptions();
            options.EndPoints.Add("", 0); //provide host and port
            var provider = new RedisConnectionProvider(options);
            var _items = provider.RedisCollection<IntID>();

            DocumentAttribute.RegisterIdGenerationStrategy("MyIncrementStrategy", new StaticIncrementStrategy(provider.Connection, typeof(IntID)));

            IntID id = new() { Name = "Test"};
            _items.Insert(id);

            Console.WriteLine("");
        }
    }
}

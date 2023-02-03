using System;
using System.Collections.Generic;
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

    [Document(StorageType = StorageType.Hash,  Prefixes = new string[] { nameof(IntIDHash) })]
    public class IntIDHash
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public int[] IntArray { get; set; } = new int[5];
    }

    [Document(StorageType = StorageType.Hash, Prefixes = new string[] { nameof(StringIDHash) })]
    public class StringIDHash
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public string[] StringArray { get; set; } = new string[5];
    }
    [Document(StorageType = StorageType.Hash, Prefixes = new string[] { nameof(ListStringIDHash) })]
    public class ListStringIDHash
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public List<string> StringArray { get; set; } = new List<string>();
    }

    [Document(StorageType = StorageType.Hash, Prefixes = new string[] { nameof(ListIntIDHash) })]
    public class ListIntIDHash
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public List<int> StringArray { get; set; } = new List<int>();
    }

    [Document(StorageType = StorageType.Json, Prefixes = new string[] { nameof(IntIDJson) })]
    public class IntIDJson
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public int[] IntArray { get; set; } = new int[5];
    }

    [Document(StorageType = StorageType.Json, Prefixes = new string[] { nameof(StringIDJson) })]
    public class StringIDJson
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public string[] StringArray { get; set; } = new string[5];
    }
    [Document(StorageType = StorageType.Json, Prefixes = new string[] { nameof(ListStringIDJson) })]
    public class ListStringIDJson
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public List<string> StringArray { get; set; } = new List<string>();
    }

    [Document(StorageType = StorageType.Json, Prefixes = new string[] { nameof(ListIntIDJson) })]
    public class ListIntIDJson
    {
        [RedisIdField] public string Id { get; set; }
        [Indexed] public string Name { get; set; }
        public List<int> StringArray { get; set; } = new List<int>();
    }


    class Program
    {
        static void Main(string[] args)
        {
            var options = new ConfigurationOptions();
            options.EndPoints.Add("", 0); //provide host and port
            var provider = new RedisConnectionProvider(options);

            //var isCreated = provider.Connection.CreateIndex(typeof(IntID));
            var _items = provider.RedisCollection<IntIDHash>();
            var _items2 = provider.RedisCollection<StringIDHash>();
            var _items3 = provider.RedisCollection<ListStringIDHash>();
            var _items4 = provider.RedisCollection<ListIntIDHash>();

            var _items5 = provider.RedisCollection<IntIDJson>();
            var _items6 = provider.RedisCollection<StringIDJson>();
            var _items7 = provider.RedisCollection<ListStringIDJson>();
            var _items8 = provider.RedisCollection<ListIntIDJson>();

            //ArrayOfIntsHash(_items);  //Unable to cast object of type 'System.Int32[]' to type 'System.Collections.Generic.IEnumerable`1[System.Object]'
            //ArrayOfStringsHash(_items2); //OK
            //ListOfStringsHash(_items3); //OK
            //ListOfIntsHash(_items4); //Unable to cast object of type 'System.Collections.Generic.List`1[System.Int32]' to type 'System.Collections.Generic.IEnumerable`1[System.Object]'.

            //ArrayOfIntsJson(_items5); //OK
            //ArrayOfStringsJson(_items6); //OK
            //ListOfStringsJson(_items7); //Insert OK, but when list is cleared and refill values then only last item in array is changed
            //ListOfIntsJson(_items8); //Insert OK, but when list is cleared and refill values then only last item in array is changed



            //Console.WriteLine($"{id1.Name}");
        }

        static void ArrayOfIntsHash(IRedisCollection<IntIDHash> _items) 
        {
            IntIDHash id = new() { Name = "Joe" };
            for (int i = 0; i < 5; i++)
            {
                id.IntArray[i] = 5;
            }
            _items.Insert(id);
        }

        static void ArrayOfStringsHash(IRedisCollection<StringIDHash> _items)
        {
            StringIDHash id = new() { Name = "Joe" };
            for (int i = 0; i < 5; i++)
            {
                id.StringArray[i] = "1";
            }
            _items.Insert(id);
        }

        static void ListOfStringsHash(IRedisCollection<ListStringIDHash> _items)
        {
            ListStringIDHash id = new() { Name = "Joe" };
            for (int i = 0; i < 5; i++)
            {
                id.StringArray.Add("1");
            }
            _items.Insert(id);
        }

        static void ListOfIntsHash(IRedisCollection<ListIntIDHash> _items)
        {
            ListIntIDHash id = new() { Name = "Joe" };
            for (int i = 0; i < 5; i++)
            {
                id.StringArray.Add(1);
            }
            _items.Insert(id);
        }



        static void ArrayOfIntsJson(IRedisCollection<IntIDJson> _items)
        {
            IntIDJson id = new() { Name = "Joe" };
            //id = _items.FindById("IntIDJson:01GRB1FR3J2Z5FEYDQ4SH3Q5TE");
            for (int i = 0; i < 5; i++)
            {
                id.IntArray[i] = 5;
            }
            _items.Insert(id);
        }

        static void ArrayOfStringsJson(IRedisCollection<StringIDJson> _items)
        {
            StringIDJson id = new() { Name = "Joe" };
            for (int i = 0; i < 5; i++)
            {
                id.StringArray[i] = "1";
            }
            _items.Insert(id);
        }

        static void ListOfStringsJson(IRedisCollection<ListStringIDJson> _items)
        {
            ListStringIDJson id = new() { Name = "Joe" };
            for (int i = 0; i < 5; i++)
            {
                id.StringArray.Add("1");
            }
            var key = _items.Insert(id);

            var id2 = _items.FindById(key);
            id.StringArray.Clear();
            for (int i = 0; i < 5; i++)
            {
                id.StringArray.Add("2");
            }

            _items.Update(id);
        }

        static void ListOfIntsJson(IRedisCollection<ListIntIDJson> _items)
        {
            ListIntIDJson id = new() { Name = "Joe" };
            
            //id.StringArray.Clear();
            for (int i = 0; i < 5; i++)
            {
                id.StringArray.Add(1);
            }
            var key = _items.Insert(id);

            var id2 = _items.FindById(key);
            id.StringArray.Clear();
            for (int i = 0; i < 5; i++)
            {
                id.StringArray.Add(2);
            }

            _items.Update(id);
        }
    }
}

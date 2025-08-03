using MinPlatform.Data.Abstractions.Models;
using System;

namespace MinPlatform.Schema.Builder.Tests
{
    public class TestEntity : AbstractEntity<int>
    {
        public override int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Age { get; set; }

        public bool IsEmployee { get; set; }

        public DateTime? DOB { get; set; }
    }
}

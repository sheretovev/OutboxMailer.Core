using Iquality.Shared.OutboxMailer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Iquality.Shared.OutboxMailer.Core.Tests
{
    public class ObjectExtensionsTests
    {
        public class A
        {
            public string StringProp { get; set; }
            public int IntProp { get; set; }
            public A InnerA { get; set; }
        }

        [Fact]
        public void TestToLog()
        {
            // Arrange
            var obj = new A
            {
                StringProp = "value",
                IntProp = 1,
                InnerA = new A { StringProp = "innervalue", IntProp = 2 }
            };
            
            // Act
            var output = obj.ToLog();

            // Assert
            Assert.True(output.Contains("=value"));
            Assert.True(output.Contains("=1"));
            Assert.True(output.Contains("=innervalue"));
            Assert.True(output.Contains("=2"));
        }
    }
}

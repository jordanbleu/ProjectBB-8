using Assets.Source.TextWriterStyle;
using Assets.Source.TextWriterStyle.Factory.Base;
using System;
using System.Collections.Generic;

namespace Assets.UnitTests.Mock.TextWriterStyle
{
    class MockTextWriterStyleFactory : TextWriterStyleFactoryBase
    {
        protected override Dictionary<string, Type> CommandRepository => new Dictionary<string, Type>()
        {
            // Easy Unit Test Implementations
            { "helloworld", typeof(HelloWorldStyle) },
            { "helloworldargs", typeof(HelloWorldArgStyle) },
            
            // Actual real world implementations
            // All Text styles should contain one or more unit tests
            { "color", typeof(ColorStyle) }
        };
    }
}

using Assets.Source.TextWriterStyle.Factory.Base;
using System;
using System.Collections.Generic;

namespace Assets.Source.TextWriterStyle.Factory
{
    public class TextWriterStyleFactory : TextWriterStyleFactoryBase
    {
        private Dictionary<string, Type> _commandRepository;
        protected override Dictionary<string, Type> CommandRepository 
        {
            get 
            {
                if (_commandRepository == null)
                {
                    _commandRepository = new Dictionary<string, Type>()
                    {
                        { "color", typeof(ColorStyle) },
                        { "currentdate", typeof(CurrentDateStyle) },
                        { "endcolor", typeof(EndColorStyle) }
                    };
                }
                return _commandRepository;
            }
        }        
    }
}

using Scriban;
using Scriban.Runtime;

namespace CryptoClock.Extensions.Functions
{
    public class CustomFunctions : ScriptObject
    {
        public CustomFunctions()
        {
            var def = TemplateContext.GetDefaultBuiltinObject();

            foreach (var item in def)
            {
                SetValue(item.Key, item.Value, true);
            }

            SetValue(PriceFunctions.MemberName, new PriceFunctions(), true);
        }
    }
}

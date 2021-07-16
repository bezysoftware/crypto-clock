using Scriban;
using Scriban.Runtime;

namespace CryptoClock.Extensions
{
    public static class ScriptExtensions
    {
        public static string Render(this Template template, TemplateContext context, object model)
        {
            var scriptObject = new ScriptObject();
            if (model != null)
            {
                scriptObject.Import(model, renamer: context.MemberRenamer);
            }

            context.PushGlobal(scriptObject);
            return template.Render(context);
        }
    }
}

using EmailClient.Contracts;
using Fluid;
using Fluid.ViewEngine;
using System.Text.RegularExpressions;

namespace EmailClient
{
    public class LiquidTemplateService : ILiquidTemplateService
    {
        private readonly IFluidViewRenderer _fluidViewRenderer;
        readonly List<Type> types = [];
        public LiquidTemplateService(IFluidViewRenderer fluidViewRenderer)
        {
            this._fluidViewRenderer = fluidViewRenderer;
        }

        public async Task<string> RenderAsync<TModel>(string templateFile, TModel model, bool allowModelMembers = true)
        {

            return await RenderAsync(templateFile, new TemplateContext(model, allowModelMembers));
        }
        public async Task<string> RenderTemplateAsync<TModel>(string template, TModel model, bool allowModelMembers = true)
        {

            return await RenderTemplateAsync(template, new TemplateContext(model, allowModelMembers));
        }

        public async Task<string> RenderAsync<TModel>(string templateFile, TModel model, TemplateOptions options, bool allowModelMembers = true)
        {
            return await RenderAsync(templateFile, new TemplateContext(model, options, allowModelMembers));
        }
        public async Task<string> RenderTemplateAsync<TModel>(string template, TModel model, TemplateOptions options, bool allowModelMembers = true)
        {
            return await RenderTemplateAsync(template, new TemplateContext(model, options, allowModelMembers));
        }

        public static string ParseCustomObjectsToLiquid(string template)
        {
            var regex = "\\<code\\sclass=\"custom-field-placeholder\"\\sdata-field-id=\"([^\"]*)\"\\sdata-field-value=\"([^\"]*)\".*?>\\<[/]code>&nbsp;";
            return Regex.Replace(template, regex, "{{ $1 }}");
        }

        public async Task<string> RenderTemplateAsync(string template, TemplateContext context)
        {
            template = ParseCustomObjectsToLiquid(template);
            var parser = new FluidParser();

            if (parser.TryParse(template, out var oTemplate, out var error))
            {
                return await oTemplate.RenderAsync(context);
            }
            throw new Exception(error);
        }

        public ILiquidTemplateService RegisterType(Type t)
        {
            if (!types.Contains(t))
            {
                types.Add(t);
            }
            return this;
        }

        public async Task<string> RenderAsync(string templateFile, TemplateContext context)
        {
            if (string.IsNullOrEmpty(templateFile))
                throw new ArgumentNullException(templateFile);
            context.Options.MemberAccessStrategy.MemberNameStrategy = MemberNameStrategies.CamelCase;
            foreach (var item in types)
            {
                context.Options.MemberAccessStrategy.Register(item);
            }
            if (!templateFile.ToLower().EndsWith(".liquid"))
                templateFile += ".liquid";
            var tw = new StringWriter();
            await _fluidViewRenderer.RenderViewAsync(tw, templateFile, context);
            var result = tw.ToString();
            //await tw.FlushAsync();
            return result;
        }
    }
}

using Fluid;

namespace EmailClient.Contracts
{
    public interface ILiquidTemplateService
    {
        Task<string> RenderAsync<TModel>(string templateFile, TModel model, bool allowModelMembers = true);
        Task<string> RenderAsync<TModel>(string templateFile, TModel model, TemplateOptions options, bool allowModelMembers = true);
        Task<string> RenderAsync(string templateFile, TemplateContext context);

        ILiquidTemplateService RegisterType(Type t);

        Task<string> RenderTemplateAsync<TModel>(string template, TModel model, bool allowModelMembers = true);

        Task<string> RenderTemplateAsync<TModel>(string template, TModel model, TemplateOptions options, bool allowModelMembers = true);

        Task<string> RenderTemplateAsync(string template, TemplateContext context);
    }
}

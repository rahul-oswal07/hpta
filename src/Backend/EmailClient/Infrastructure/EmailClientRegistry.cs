using EmailClient.Contracts;
using Fluid;
using Fluid.ViewEngine;
using HPTA.Common.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace EmailClient.Infrastructure
{
    public static class EmailClientRegistry
    {
        public static void RegisterEmailClient(this IServiceCollection services, IFileProvider contentRootFileProvider, ApplicationSettings appSettings)
        {
            var options = new FluidViewEngineOptions();

            options.TemplateOptions.MemberAccessStrategy.MemberNameStrategy = MemberNameStrategies.CamelCase;
            options.PartialsFileProvider = new FileProviderMapper(contentRootFileProvider, "EmailTemplates/Partials");
            options.ViewsFileProvider = new FileProviderMapper(contentRootFileProvider, "EmailTemplates");

            options.ViewsLocationFormats.Clear();
            options.ViewsLocationFormats.Add("/{1}/{0}" + Fluid.ViewEngine.Constants.ViewExtension);
            options.ViewsLocationFormats.Add("/Shared/{0}" + Fluid.ViewEngine.Constants.ViewExtension);

            options.PartialsLocationFormats.Clear();
            options.PartialsLocationFormats.Add("{0}" + Fluid.ViewEngine.Constants.ViewExtension);
            options.PartialsLocationFormats.Add("/Partials/{0}" + Fluid.ViewEngine.Constants.ViewExtension);
            options.PartialsLocationFormats.Add("/Partials/{1}/{0}" + Fluid.ViewEngine.Constants.ViewExtension);
            options.PartialsLocationFormats.Add("/Shared/Partials/{0}" + Fluid.ViewEngine.Constants.ViewExtension);
            var renderer = new FluidViewRenderer(options);
            var templateService = new LiquidTemplateService(renderer);
            services.AddSingleton<IEmailSender, EmailSender>(services =>
            {
                return new EmailSender(templateService, appSettings.EmailClientConfig);
            });
        }
    }
}

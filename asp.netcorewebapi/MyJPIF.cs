using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asp.netcorewebapi
{
    public class MyJPIF
    {
        ///This function configures support for JSON Patch using
        ///Newtonsoft.Json while leaving the other formatters unchanged.
        ///By using AddNewtonsoftJson, we are replacing 
        ///the System.Text.Json formatters for all JSON content.
        public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
            new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}

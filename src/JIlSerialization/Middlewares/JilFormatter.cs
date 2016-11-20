using Jil;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JIlSerialization.Middlewares
{
    public class JilFormatter
    {
        public static void ReplaceFormatter(MvcOptions options)
        {
            var inputFormatter = options.InputFormatters.FirstOrDefault(f => f.GetType().Name == "JsonInputFormatter");
            options.InputFormatters.Remove(inputFormatter);
            options.InputFormatters.Insert(0, new JilInput());

            var outputFormatter = options.OutputFormatters.FirstOrDefault(f => f.GetType().Name == "JsonOutputFormatter");
            options.OutputFormatters.Remove(outputFormatter);
            options.OutputFormatters.Insert(0, new JilOutput());

        }
    }

    public class JilOutput : IOutputFormatter
    {
        readonly Options _jilOptions;

        public JilOutput()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);
        }

        public bool CanWriteResult(OutputFormatterCanWriteContext context) => true;

        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            
            using (var writer = new StreamWriter(context.HttpContext.Response.Body))
            {              
                JSON.Serialize(context.Object, writer, _jilOptions);
                await writer.FlushAsync();
            }
        }
    }

    public class JilInput : IInputFormatter
    {
        readonly Options _jilOptions;
        public JilInput()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);

        }

        public bool CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var contentType = context.HttpContext.Request.ContentType;
            return contentType == null || contentType == "application/json";
        }

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue requestContentType;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);
            using (var reader = new StreamReader(context.HttpContext.Request.Body))
            {
                var result = JSON.Deserialize(reader, type, _jilOptions);
                return await InputFormatterResult.SuccessAsync(result);
            }
        }
    }
}

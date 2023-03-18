using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Http.API.Middleware;

public class SwaggerFilter
{
}

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var name = new OpenApiArray();
            var enumData = new OpenApiArray();
            var fields = context.Type.GetFields();
            foreach (var f in fields)
            {
                if (f.Name != "value__")
                {
                    name.Add(new OpenApiString(f.Name));
                    var desAttr = f.CustomAttributes.Where(a => a.AttributeType.Name == "DescriptionAttribute").FirstOrDefault();

                    if (desAttr != null)
                    {
                        var des = desAttr.ConstructorArguments.FirstOrDefault();
                        if (des.Value != null)
                        {
                            enumData.Add(new OpenApiObject()
                            {
                                ["name"] = new OpenApiString(f.Name),
                                ["description"] = new OpenApiString(des.Value.ToString())
                            });
                        }
                    }
                }
            }
            model.Extensions.Add("x-enumNames", name);
            model.Extensions.Add("x-enumData", enumData);
        }
    }
}
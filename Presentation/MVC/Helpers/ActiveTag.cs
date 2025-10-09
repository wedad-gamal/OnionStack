using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.Helpers;

[HtmlTargetElement("a", Attributes = "active-when")]
public class ActiveTag : TagHelper
{
    public string ActiveWhen { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContextData { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrEmpty(ActiveWhen)) return Task.CompletedTask;

        var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();
        if (currentController!.Equals(ActiveWhen))
        {
            if (output.Attributes["class"].Value != null)
                output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
            else
                output.Attributes.SetAttribute("class", "active");

        }
        return Task.CompletedTask;
    }
}

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Game.Facade.Security.Captcha;

namespace Game.Facade.TagHelpers.Public
{
    /// <summary>
    /// game-captcha tag helper
    /// </summary>
    [HtmlTargetElement("game-captcha", TagStructure = TagStructure.WithoutEndTag)]
    public class GameGenerateCaptchaTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly CaptchaSettings _captchaSettings;

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="captchaSettings">Captcha settings</param>
        public GameGenerateCaptchaTagHelper(IHtmlHelper htmlHelper, CaptchaSettings captchaSettings)
        {
            _htmlHelper = htmlHelper;
            _captchaSettings = captchaSettings;
        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //generate captcha control
            var captchaControl = new GRecaptchaControl(_captchaSettings.ReCaptchaVersion)
            {
                Theme = _captchaSettings.ReCaptchaTheme,
                Id = "recaptcha",
                PublicKey = _captchaSettings.ReCaptchaPublicKey,
                Language = _captchaSettings.ReCaptchaLanguage
            };
            var captchaControlHtml = captchaControl.RenderControl();

            //tag details
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(captchaControlHtml);
        }
    }
}
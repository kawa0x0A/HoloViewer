using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MobileBlazorBindings.Elements;

namespace HoloViewer
{
    public interface ITwitterDialog
    {
        protected const string GetHashTagsScriptPath = ScriptPath + "GetHashTags.js";

        private const string ScriptPath = "./wwwroot/_content/HoloViewer/scripts/tweet/";

        Task<string[]> GetHashTags (BlazorWebView blazorWebView);

        Task Tweet (string[] hashTags);
    }
}

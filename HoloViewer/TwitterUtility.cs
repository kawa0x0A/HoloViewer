using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace HoloViewer
{
    public static class TwitterUtility
    {
        public static string[] ConvertStringToHashTagArray (string hashTagString)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.WPF:
                    return hashTagString.Replace(@"""", "").Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').Select(str => System.Web.HttpUtility.UrlDecode(str)).Distinct().ToArray();
                case Device.macOS:
                    return hashTagString.Replace(@"""", "").Replace("(", "").Replace(")", "").Replace(" ", "").Split(',').Select(str => System.Web.HttpUtility.UrlDecode(str).Trim('\n')).Distinct().ToArray();
            }

            return null;
        }

        public static string CreateTweetUrl (string[] hashTags)
        {
            var hashTagBuilder = new StringBuilder();

            for (int i = 0; i < hashTags.Length; i++)
            {
                if (i > 0)
                {
                    hashTagBuilder.Append(",");
                }

                hashTagBuilder.Append(System.Web.HttpUtility.UrlEncode(hashTags[i].Replace("#", "")));
            }

            string hashTagParamter = (hashTags.All(str => string.IsNullOrWhiteSpace(str))) ? "" : $"?hashtags={hashTagBuilder}";

            return $"https://twitter.com/intent/tweet{hashTagParamter}";
        }
    }
}

using UnityEngine;

namespace Nikiton.Cloud.Legal
{
    public enum LegalDoc { PrivacyPolicy, TermsOfUse, EULA }

    public static class LegalProvider
    {
        public static string GetText(LegalDoc doc, string locale = "ru")
        {
            var name = doc.ToString();
            var langPath = $"Legal/{name}.{locale}";
            var defPath  = $"Legal/{name}";
            var ta = Resources.Load<TextAsset>(langPath) ?? Resources.Load<TextAsset>(defPath);
            return ta ? ta.text : $"[{name} not found]";
        }
    }
}

using UnityEngine;

namespace Nikiton.Cloud.Legal
{
    public enum LegalDoc { PrivacyPolicy, TermsOfUse, EULA }

    public static class LegalProvider
    {
        public static string GetText(LegalDoc doc, string locale = "ru")
        {
            // Сначала локаль: Legal/TermsOfUse.ru → затем дефолт: Legal/TermsOfUse
            var n = doc.ToString();
            var langPath = $"Legal/{n}.{locale}";
            var defPath  = $"Legal/{n}";
            var ta = Resources.Load<TextAsset>(langPath) ?? Resources.Load<TextAsset>(defPath);
            return ta ? ta.text : $"[{n} not found]";
        }
    }
}

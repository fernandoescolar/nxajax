using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Ajax.Utils
{
    public static class Extensions
    {
#if VS2008
        public static string Replace(this string original, string pattern, string replacement, StringComparison comparisonType) 
#else
        public static string Replace(string original, string pattern, string replacement, StringComparison comparisonType) 
#endif
        { 
#if VS2008
            return original.Replace(pattern, replacement, comparisonType, -1); 
#else
            return Replace(original, pattern, replacement, comparisonType, -1); 
#endif
        } 
#if VS2008       
        public static string Replace(this string original, string pattern, string replacement, StringComparison comparisonType, int stringBuilderInitialSize) 
#else
        public static string Replace(string original, string pattern, string replacement, StringComparison comparisonType, int stringBuilderInitialSize) 
#endif
        { 
            if (original == null) { return null; } 
            if (String.IsNullOrEmpty(pattern)) { return original; } 
            int posCurrent = 0; 
            int lenPattern = pattern.Length; 
            int idxNext = original.IndexOf(pattern, comparisonType);
            StringBuilder result = new StringBuilder(stringBuilderInitialSize < 0 ? Math.Min(4096, original.Length) : stringBuilderInitialSize); 
            while (idxNext >= 0) 
            { 
                result.Append(original, posCurrent, idxNext - posCurrent); 
                result.Append(replacement); 
                posCurrent = idxNext + lenPattern; 
                idxNext = original.IndexOf(pattern, posCurrent, comparisonType); 
            } 
            result.Append(original, posCurrent, original.Length - posCurrent); 
            return result.ToString(); 
        }
    }
}

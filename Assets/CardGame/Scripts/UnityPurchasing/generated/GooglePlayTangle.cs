// WARNING: Do not modify! Generated file.

using UnityEngine.Purchasing.Security;

namespace UnityPurchasing.generated {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("E+jIXONqRSuDdgJaGpjJQrTtDkhjS1FwlcjJ0LWNkMuSPdaFXofQ/i/NJkNE0pJUVRapspZzD+2pfCr1Adl0QWlaNr0gzjjYwJQevpI76EOFN7SXhbizvJ8z/TNCuLS0tLC1tnyp/jQjb3/+wYqZaS78tbLnX6+QODawsXTy3KwZ8bV5s7TnScRQJqcnLYiRpSfmVRVD2V1TsBmYW2LifazGJU6BqJjGccf8OL6NPLdOGjVn2pc/XYBR6QjPJxUTtGQPZsYIO1MvUa+4oL8xnyIvgfBGfNQzxyXRewhaJMDOVWjoeeM+lTNDHy3AdTtlYAYdzvIo0xgRPSRV0ZY0o5YmHL03tLq1hTe0v7c3tLS1GZkTVIo2IekZvREFgJYU1re2tLW0");
        private static int[] order = new int[] { 2,7,5,13,5,11,10,12,9,9,13,11,13,13,14 };
        private static int key = 181;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

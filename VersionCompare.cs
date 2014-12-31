using System;

namespace CKANUtils
{
    public static class VersionCompare
    {

        public static int CKANPackageVersionCompare(string a, string b)
        {
            if (a == null && b == null)
            {
                return 0;
            }
            
            if (a == null)
            {
                return -1;
            }

            if (b == null)
            {
                return 1;
            }

            if (a == b)
            {
                return 0;
            }

            string epoch1, ver1, rel1;
            string epoch2, ver2, rel2;

            parseEVR(a, out epoch1, out ver1, out rel1);
            parseEVR(b, out epoch2, out ver2, out rel2);

            int ret = rpmvercmp(epoch1, epoch2);
            if (ret == 0)
            {
                ret = rpmvercmp(ver1, ver2);
                if (ret == 0 && (rel1 != null) && (rel2 != null))
                {
                    ret = rpmvercmp(rel1, rel2);
                }
            }

            return ret;
        }

        private static int rpmvercmp(string a, string b)
        {
            /* easy comparison to see if versions are identical */
            if (a == b)
            {
                return 0;
            }

            var str1 = (string)a.Clone();
            var str2 = (string)b.Clone();

            int one = 0, ptr1 = 0;
            int two = 0, ptr2 = 0;

            bool isnum = false;

            /* loop through each version segment of str1 and str2 and compare them */
            while (one < str1.Length && two < str2.Length)
            {
                /* If we ran to the end of either, we are finished with the loop */
                while (one < str1.Length && !Char.IsLetterOrDigit(str1[one]))
                {
                    one++;
                }

                while (two < str2.Length && !Char.IsLetterOrDigit(str2[two]))
                {
                    two++;
                }

                if (one >= str1.Length)
                {
                    break;
                }

                if (two >= str2.Length)
                {
                    break;
                }

                /* If the separator lengths were different, we are also finished */
                if ((one - ptr1) != (two - ptr2))
                {
                    return (one - ptr1) < (two - ptr2) ? -1 : 1;
                }

                ptr1 = one;
                ptr2 = two;

                if (Char.IsDigit(str1[ptr1]))
                {
                    while (ptr1 < str1.Length && Char.IsDigit(str1[ptr1]))
                    {
                        ptr1++;
                    }

                    while (ptr2 < str2.Length && Char.IsDigit(str2[ptr2]))
                    {
                        ptr2++;
                    }

                    isnum = true;
                }
                else
                {
                    while (ptr1 < str1.Length && Char.IsLetter(str1[ptr1]))
                    {
                        ptr1++;
                    }

                    while (ptr2 < str2.Length && Char.IsLetter(str2[ptr2]))
                    {
                        ptr2++;
                    }

                    isnum = false;
                }

                if (two == ptr2)
                {
                    return isnum ? 1 : -1;
                }

                if (isnum)
                {
                    while (str1[one] == '0')
                    {
                        one++;
                    }

                    while (str2[two] == '0')
                    {
                        two++;
                    }

                    if (str1.Substring(one).Length > str2.Substring(two).Length)
                    {
                        return 1;
                    }

                    if (str2.Substring(two).Length > str1.Substring(one).Length)
                    {
                        return -1;
                    }
                }

                int rc = one.CompareTo(two);
                if (rc > 0)
                {
                    return rc < 1 ? -1 : 1;
                }
            }

            if (one >= str1.Length && two >= str2.Length)
            {
                return 0;
            }

            if ((one >= str1.Length && !Char.IsLetter(str2[two])) || Char.IsLetter(str1[one]))
            {
                return -1;
            }

            return 1;
        }

        private static void parseEVR(string evr, out string epoch, out string version, out string release)
        {
            int s = 0;
            while (s < evr.Length && Char.IsDigit(evr[s]))
            {
                s++;
            }

            int se = evr.Substring(s).IndexOf('-');

            if (evr[s] == ':')
            {
                epoch = evr.Substring(0, s++);
                version = evr.Substring(s);
            }
            else
            {
                epoch = "0";
                version = evr;
            }

            release = se != -1 ? evr.Substring(se++) : null;
        }

    }

}

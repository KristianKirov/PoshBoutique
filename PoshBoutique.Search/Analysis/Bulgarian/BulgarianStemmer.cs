using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search.Analysis.Bulgarian
{
    public class BulgarianStemmer
    {
        public int Stem(char[] s, int len)
        {
            if (len < 4)
            {
                return len;
            }

            if (len > 5 && this.EndsWith(s, len, "ища"))
            {
                return len - 3;
            }

            len = this.RemoveArticle(s, len);
            len = this.RemovePlural(s, len);

            if (len > 3)
            {
                if (EndsWith(s, len, "я"))
                {
                    len--;
                }
                if (EndsWith(s, len, "а") ||
                    EndsWith(s, len, "о") ||
                    EndsWith(s, len, "е"))
                {
                    len--;
                }
            }

            if (len > 4 && this.EndsWith(s, len, "ен"))
            {
                s[len - 2] = 'н'; // replace with н
                len--;
            }

            if (len > 5 && s[len - 2] == 'ъ')
            {
                s[len - 2] = s[len - 1]; // replace ъN with N
                len--;
            }

            return len;
        }

        private int RemoveArticle(char[] s, int len)
        {
            if (len > 6 && this.EndsWith(s, len, "ият"))
            {
                return len - 3;
            }

            if (len > 5)
            {
                if (this.EndsWith(s, len, "ът") ||
                    this.EndsWith(s, len, "то") ||
                    this.EndsWith(s, len, "те") ||
                    this.EndsWith(s, len, "та") ||
                    this.EndsWith(s, len, "ия"))
                {
                    return len - 2;
                }
            }

            if (len > 4 && this.EndsWith(s, len, "ят"))
            {
                return len - 2;
            }

            return len;
        }

        private int RemovePlural(char[] s, int len)
        {
            if (len > 6)
            {
                if (this.EndsWith(s, len, "овци"))
                {
                    return len - 3; // replace with о
                }
                if (this.EndsWith(s, len, "ове"))
                {
                    return len - 3;
                }
                if (this.EndsWith(s, len, "еве"))
                {
                    s[len - 3] = 'й'; // replace with й

                    return len - 2;
                }
            }

            if (len > 5)
            {
                if (this.EndsWith(s, len, "ища"))
                {
                    return len - 3;
                }
                if (this.EndsWith(s, len, "та"))
                {
                    return len - 2;
                }
                if (this.EndsWith(s, len, "ци"))
                {
                    s[len - 2] = 'к'; // replace with к

                    return len - 1;
                }
                if (this.EndsWith(s, len, "зи"))
                {
                    s[len - 2] = 'г'; // replace with г

                    return len - 1;
                }

                if (s[len - 3] == 'е' && s[len - 1] == 'и')
                {
                    s[len - 3] = 'я'; // replace е with я, remove и

                    return len - 1;
                }
            }

            if (len > 4)
            {
                if (this.EndsWith(s, len, "си"))
                {
                    s[len - 2] = 'х'; // replace with х

                    return len - 1;
                }
                if (this.EndsWith(s, len, "и"))
                {
                    return len - 1;
                }
            }

            return len;
        }

        private bool EndsWith(char[] s, int len, string suffix)
        {
            int suffixLen = suffix.Length;
            if (suffixLen > len)
            {
                return false;
            }

            for (int i = suffixLen - 1; i >= 0; i--)
            {
                if (s[len - (suffixLen - i)] != suffix[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

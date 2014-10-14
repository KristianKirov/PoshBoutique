using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search.Analysis.Bulgarian
{
    public class BulgarianStemFilter : TokenFilter
    {
        private readonly BulgarianStemmer stemmer;
        private readonly ITermAttribute termAtt;

        public BulgarianStemFilter(TokenStream input)
            : base(input)
        {
            stemmer = new BulgarianStemmer();
            this.termAtt = this.AddAttribute<ITermAttribute>();
        }

        public override bool IncrementToken()
        {
            if (input.IncrementToken())
            {
                int newlen = stemmer.Stem(termAtt.TermBuffer(), termAtt.TermLength());
                termAtt.SetTermLength(newlen);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search.Analysis.Bulgarian
{
    public class BulgarianAnalyzer : Analyzer
    {
        public readonly static string DEFAULT_STOPWORD_FILE = "PoshBoutique.Search.Analysis.Bulgarian.stopwords.txt";

        private readonly HashSet<string> stoptable;

        public static readonly string STOPWORDS_COMMENT = "#";

        public static HashSet<string> GetDefaultStopSet()
        {
            return DefaultSetHolder.DEFAULT_STOP_SET;
        }

        private static class DefaultSetHolder
        {
            public static readonly HashSet<string> DEFAULT_STOP_SET;

            static DefaultSetHolder()
            {
                try
                {
                    DefaultSetHolder.DEFAULT_STOP_SET = DefaultSetHolder.LoadDefaultStopWordSet();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("Unable to load default stopword set", ex);
                }
            }

            private static HashSet<string> LoadDefaultStopWordSet()
            {
                Assembly stopwordsAssembly = typeof(BulgarianAnalyzer).Assembly;

                using (Stream stream = stopwordsAssembly.GetManifestResourceStream(BulgarianAnalyzer.DEFAULT_STOPWORD_FILE))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    IEnumerable<string> stopwordsList = result.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries).
                        Where(s => !(string.IsNullOrEmpty(s) || s.StartsWith(BulgarianAnalyzer.STOPWORDS_COMMENT)));

                    HashSet<string> defaultStopwords = new HashSet<string>(stopwordsList);

                    return defaultStopwords;
                }
            }
        }

        private readonly Version matchVersion;
        private readonly bool enableStopPositionIncrements;

        public BulgarianAnalyzer(Version matchVersion)
            : this(matchVersion, DefaultSetHolder.DEFAULT_STOP_SET)
        {
        }

        public BulgarianAnalyzer(Version matchVersion, HashSet<string> stopwords)
        {
            this.stoptable = new HashSet<string>(CharArraySet.Copy(stopwords));
            this.matchVersion = matchVersion;
            this.enableStopPositionIncrements = StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion);
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream result = new StandardTokenizer(matchVersion, reader);
            result = new StandardFilter(result);
            result = new LowerCaseFilter(result);
            result = new StopFilter(this.enableStopPositionIncrements, result, stoptable);
            result = new BulgarianStemFilter(result);

            return result;
        }

        private class SavedStreams
        {
            public Tokenizer Source { get; set; }
            public TokenStream Result { get; set; }
        };

        public override TokenStream ReusableTokenStream(string fieldName, TextReader reader)
        {
            SavedStreams streams = this.PreviousTokenStream as SavedStreams;
            if (streams == null)
            {
                streams = new SavedStreams();
                streams.Source = new StandardTokenizer(matchVersion, reader);
                streams.Result = new StandardFilter(streams.Source);
                streams.Result = new LowerCaseFilter(streams.Result);
                streams.Result = new StopFilter(false, streams.Result, stoptable);
                streams.Result = new BulgarianStemFilter(streams.Result);
                this.PreviousTokenStream = streams;
            }
            else
            {
                streams.Source.Reset(reader);
            }

            return streams.Result;
        }
    }
}

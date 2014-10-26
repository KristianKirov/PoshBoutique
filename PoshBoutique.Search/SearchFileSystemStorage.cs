using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PoshBoutique.Search.Analysis.Bulgarian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace PoshBoutique.Search
{
    public class SearchFileSystemStorage<T>
    {
        private ISearchEntryAdapter<T> searchEntryAdapter;
        private FSDirectory indexDirectory;

        public SearchFileSystemStorage(ISearchEntryAdapter<T> searchEntryAdapter)
        {
            this.searchEntryAdapter = searchEntryAdapter;
            this.indexDirectory = this.GetIndexDirectory(searchEntryAdapter.UniqueKey);
        }

        private FSDirectory GetIndexDirectory(string uniqueIndex)
        {
            string indexDir = HostingEnvironment.MapPath(string.Concat("~/SearchIndexes/", uniqueIndex));
            FSDirectory indexDyrectory = FSDirectory.Open(new DirectoryInfo(indexDir));

            if (IndexWriter.IsLocked(indexDyrectory))
            {
                IndexWriter.Unlock(indexDyrectory);
            }

            string lockFilePath = Path.Combine(indexDir, "write.lock");
            if (File.Exists(lockFilePath))
            {
                File.Delete(lockFilePath);
            }

            return indexDyrectory;
        }

        //private static string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index");
        //private static FSDirectory _directoryTemp;
        //private static FSDirectory _directory
        //{
        //    get
        //    {
        //        if (_directoryTemp == null)
        //        {
        //            _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
        //        }

        //        if (IndexWriter.IsLocked(_directoryTemp))
        //        {
        //            IndexWriter.Unlock(_directoryTemp);
        //        }

        //        var lockFilePath = Path.Combine(_luceneDir, "write.lock");
        //        if (File.Exists(lockFilePath))
        //        {
        //            File.Delete(lockFilePath);
        //        }

        //        return _directoryTemp;
        //    }
        //}

        private Document BuildIndexDocument(ISearchEntry searchEntry)
        {
            Document indexDocument = new Document();
            indexDocument.Add(new Field(searchEntry.Key.FieldName, searchEntry.Key.FieldValue, Field.Store.YES, Field.Index.NOT_ANALYZED));

            foreach (IndexField field in searchEntry.SearchFieldsData)
            {
                indexDocument.Add(
                    new Field(
                        field.FieldName,
                        field.FieldValue,
                        field.Store ? Field.Store.YES : Field.Store.NO,
                        field.Analyze ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED));
            }

            return indexDocument;
        }

        private void AddToLuceneIndex(ISearchEntry searchEntry, IndexWriter writer)
        {
            // remove older index entry
            TermQuery searchQuery = new TermQuery(new Term(searchEntry.Key.FieldName, searchEntry.Key.FieldValue));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            Document indexDocument = this.BuildIndexDocument(searchEntry);
            writer.AddDocument(indexDocument);
        }

        private Analyzer GetAnalyzer()
        {
            return new BulgarianAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }

        public void AddUpdateLuceneIndex(IEnumerable<ISearchEntry> searchEntries)
        {
            // init lucene
            Analyzer analyzer = this.GetAnalyzer();
            using (IndexWriter writer = new IndexWriter(indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (var sampleData in searchEntries)
                {
                    this.AddToLuceneIndex(sampleData, writer);
                }

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public void AddUpdateLuceneIndex(ISearchEntry searchEntry)
        {
            this.AddUpdateLuceneIndex(new ISearchEntry[1] { searchEntry });
        }

        public void ClearLuceneIndexRecord(IndexField keyField)
        {
            // init lucene
            Analyzer analyzer = this.GetAnalyzer();
            using (var writer = new IndexWriter(this.indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term(keyField.FieldName, keyField.FieldValue));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public bool ClearLuceneIndex()
        {
            try
            {
                Analyzer analyzer = this.GetAnalyzer();
                using (var writer = new IndexWriter(this.indexDirectory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Optimize()
        {
            Analyzer analyzer = this.GetAnalyzer();
            using (IndexWriter writer = new IndexWriter(this.indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();

                writer.Optimize();

                writer.Dispose();
            }
        }

        private IEnumerable<T> MapSearchHitsToItems(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return searchEntryAdapter.AssembleItems(hits.Select(hit => searcher.Doc(hit.Doc)));
        }

        private Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }

            return query;
        }

        private IEnumerable<T> SearchInternal(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
            {
                return new List<T>();
            }

            // set up lucene searcher
            using (IndexSearcher searcher = new IndexSearcher(this.indexDirectory, true))
            {
                int hits_limit = 1000;
                Analyzer analyzer = this.GetAnalyzer();

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                    Query query = this.ParseQuery(searchQuery, parser);
                    ScoreDoc[] hits = searcher.Search(query, hits_limit).ScoreDocs;
                    IEnumerable<T> results = this.MapSearchHitsToItems(hits, searcher);

                    analyzer.Close();
                    searcher.Dispose();

                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, searchEntryAdapter.GetSearchFieldNames(), analyzer);
                    Query query = this.ParseQuery(searchQuery, parser);
                    ScoreDoc[] hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    IEnumerable<T> results = this.MapSearchHitsToItems(hits, searcher);

                    analyzer.Close();
                    searcher.Dispose();

                    return results;
                }
            }
        }

        public IEnumerable<T> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<T>();
            }

            //var terms = input.Trim().Replace("-", " ").Split(' ')
            //    .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            //input = string.Join(" ", terms);

            return this.SearchInternal(input, fieldName);
        }  
    }
}

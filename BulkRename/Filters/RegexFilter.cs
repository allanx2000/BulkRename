using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BulkRename.Filters
{
    public class RegexFilter : AbstractFilter
    {
        public const string NAME = "RegexFilter";
        public RegexFilter() : base(NAME)
        {

        }

        private const string SearchFor = "SearchFor";
        private const string ReplaceWith = "ReplaceWith";

        private static List<Option> options = new List<Option>()
        {
            new Option(SearchFor, "Regex pattern to match"),
            new Option(ReplaceWith, "Text to replace with")
        };

        public override List<Option> GetOptions()
        {
            return options;
        }

        public override void Validate(List<Option> options)
        {
            var searchFor = options.First(x => x.Name == SearchFor);

            if (String.IsNullOrEmpty(searchFor.Value))
                throw new Exception("SearchFor criteria cannot be empty");
        }

        public override string DoFilter(string name, List<Option> options)
        {
            //TODO: make options into a dictionary

            var searchFor = options.First(x => x.Name == SearchFor);
            var replaceWith = options.First(x => x.Name == ReplaceWith);

            Regex matcher = new Regex(searchFor.Value, RegexOptions.Singleline);

            var matches = matcher.Matches(name);

            //Sort
            Dictionary<int, Match> matchDictionary = new Dictionary<int, Match>();
            foreach (Match m in matches)
            {
                matchDictionary.Add(m.Index, m);
            }

            //Replace all matches, in reverse orders
            foreach (int m in matchDictionary.Keys.OrderByDescending(x=>x))
            {
                var match = matchDictionary[m];
                string head = name.Substring(0, match.Index);
                string tail = name.Substring(match.Index + match.Length);

                name = head + replaceWith.Value + tail; 
            }

            return name;
        }
    }
}

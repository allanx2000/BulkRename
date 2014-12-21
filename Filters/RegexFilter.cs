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

        //Option Keys
        private const string SearchFor = "SearchFor";
        private const string ReplaceWith = "ReplaceWith";

        /// <summary>
        /// Replaces a the Regex matches with a specific string
        /// </summary>
        public RegexFilter() : base(NAME)
        {

        }


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
            Option searchFor = GetOption(options, SearchFor);
            Option replaceWith = GetOption(options, ReplaceWith);

            Regex matcher = new Regex(searchFor.Value, RegexOptions.Singleline);

            var matches = matcher.Matches(name);

            //Sort the matches by index of match
            Dictionary<int, Match> matchDictionary = new Dictionary<int, Match>();
            foreach (Match m in matches)
            {
                matchDictionary.Add(m.Index, m);
            }

            //Replace all matches, in reverse orders
            //This will keep the index of the next match unchanged
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

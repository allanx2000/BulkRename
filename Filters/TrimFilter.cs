using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkRename.Filters
{
    class TrimFilter : AbstractFilter
    {
        public const string NAME = "TrimFilter";
        public TrimFilter() : base(NAME)
        {

        }

        private const string TrimChar = "TrimChar";
        public override List<Option> GetOptions()
        {
            return new List<Option>()
            {
                new Option(TrimChar, "Character to trim")
            };
        }

        public override void Validate(List<Option> options)
        {
            var tchar = GetOption(options, TrimChar);

            if (tchar.Value.Length > 1)
                throw new Exception(TrimChar + " must be a single character");
        }

        public override string DoFilter(string name, List<Option> options)
        {
            var tchar = GetOption(options, TrimChar);
            
            char chr = tchar.Value == "" ? ' ' : tchar.Value[0];

            return name.Trim(chr);
        }
    }
}

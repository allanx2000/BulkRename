using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.Filters
{
    public abstract class AbstractFilter
    {
        /// <summary>
        /// Abstract class for filters that provides public methods and helper functions
        /// </summary>
        /// <param name="name">Name/ID of the filter</param>
        protected AbstractFilter(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Validates the Filter Definition
        /// 
        /// Throws exception on errors
        /// </summary>
        /// <param name="args"></param>
        public void Validate(FilterDefinition args)
        {
            Validate(args.Options);
        }

        /// <summary>
        /// Perform the filter on the input string
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string DoFilter(string inputString, FilterDefinition args)
        {
            return DoFilter(inputString, args.Options);
        }
        
        public abstract List<Option> GetOptions();

        /// <summary>
        /// Makes sure the options values are valid.
        /// 
        /// The implementation should throw an exception on any failures
        /// </summary>
        /// <param name="options"></param>
        public abstract void Validate(List<Option> options);

        /// <summary>
        /// Performs the actual filtering on a string; a chain of filters would call this one by one with the previous filter's output
        /// </summary>
        /// <param name="inputString">A file name or string to run the filter on</param>
        /// <param name="options">Options used by the specific filter</param>
        /// <returns></returns>
        public abstract string DoFilter(string inputString, List<Option> options);

        /// <summary>
        /// Helper function to get the option of the specified name
        /// </summary>
        /// <param name="options"></param>
        /// <param name="optionName"></param>
        /// <returns>The Option object including the (Option)Name, Description, and Value</returns>
        protected Option GetOption(List<Option> options, string optionName)
        {
            return options.First(x => x.Name == optionName);
        }
    }

   

}

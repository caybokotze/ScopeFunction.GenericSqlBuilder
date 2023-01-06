using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder
{
    public interface ISelectOptions : IOptions
    {
        ISelectOptions WithPropertyCasing(Casing casing);
        ISelectOptions WithProperty(string property);
        ISelectOptions WithoutProperty(string property);
        ISelectOptions WithTop(int value);
    }

    public class SelectOptions : Options, ISelectOptions
    {
        public SelectOptions()
        {
            WithProperties = new List<string>();
            WithoutProperties = new List<string>();
            AppendAfterFrom = new List<string>();
            var configuration = GenericQueryBuilderSettings.Configuration;
            Variant = configuration.Variant;
            PropertyCase = configuration.Casing;
        }
    
        public List<string> WithProperties { get; }
        public List<string> WithoutProperties { get; }
        public Casing PropertyCase { get; private set; }
        
        public Variant Variant { get; private set; }
        
        public int? TopValue { get; private set; }
        
        public List<string> AppendAfterFrom { get; set; }
        public bool IsAppendWhere { get; set; }
        public bool IsAppendSelect { get; set; }
    
        public ISelectOptions WithPropertyCasing(Casing casing)
        {
            PropertyCase = casing;
            return this;
        }
        
        public ISelectOptions WithSqlVariant(Variant variant)
        {
            Variant = variant;
            return this;
        }

        public ISelectOptions WithProperty(string property)
        {
            WithProperties.Add(property);
            return this;
        }

        public ISelectOptions WithoutProperty(string property)
        {
            WithoutProperties.Add(property);
            return this;
        }
        
        public ISelectOptions WithTop(int value)
        {
            TopValue = value;
            return this;
        }
        
        /// <summary>
        /// Specifies the prefix which will be appended to the SELECT or WHERE clause.
        /// This method is void and should be placed at the end of the builder
        /// </summary>
        /// <param name="prefix"></param>
        public void WithPropertyPrefix(string prefix)
        {
            Prefix = prefix;
        }

        /// <summary>
        /// Specifies the prefix which will be removed from the SELECT or WHERE clause.
        /// This method is void and should be placed at the end of the builder
        /// </summary>
        public void WithoutPropertyPrefix()
        {
            IgnorePrefix = true;
        }
    }
}

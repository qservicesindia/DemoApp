using System.Collections.Generic;

namespace LoginForm.Data.Extensions
{
    public class LinqDynamicExpressionHelper
    {
        #region Linq Dynamic Operators
        public enum LinqDynamicOperator
        {
            Contains,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqualTo,
            StartsWith,
            EndsWith,
            Equals,
            NotEqual
        }
        #endregion

        #region Linq Dynamic Filter
        public class LinqDynamicFilter
        {
            #region private fields
            private LinqDynamicOperator _operator = LinqDynamicOperator.Equals;
            #endregion

            #region properties
            public string PropertyName { get; set; }
            public object SearchValue { get; set; }

            public LinqDynamicOperator Operator
            {
                get { return _operator; }
                set { _operator = value; }
            }
            #endregion
        }

        public class LinqDynamicFilters : List<LinqDynamicFilter>
        {
            public void Add(string propertyName, object searchValue, LinqDynamicOperator linqOperator = LinqDynamicOperator.Equals)
            {
                this.Add(new LinqDynamicFilter() { PropertyName = propertyName, SearchValue = searchValue, Operator = linqOperator });
            }
        }
        #endregion
    }
}

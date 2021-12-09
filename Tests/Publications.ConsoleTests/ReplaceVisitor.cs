using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Publications.ConsoleTests
{
    public class ReplaceVisitor : ExpressionVisitorEx
    {
        private readonly Expression _From;
        private readonly Expression _To;

        public ReplaceVisitor(Expression from, Expression to)
        {
            _From = from;
            _To = to;
        }

        public override Expression Visit(Expression node) => node == _From ? _To : base.Visit(node);
    }
}

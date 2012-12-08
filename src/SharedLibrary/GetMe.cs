using System;
using System.Linq.Expressions;

namespace SharedLibrary
{
    public static class GetMe
    {
        public static string FirstToUpper(this string source)
        {
            if (string.IsNullOrEmpty(source.Trim()))
                return source;

            var firstLetter = Char.ToUpper(source[0]);
            return string.Concat(firstLetter.ToString(), source.Length > 1 ? source.Substring(1) : string.Empty);
        }

        public static string FirstToLower(this string source)
        {
            if (string.IsNullOrEmpty(source.Trim()))
                return source;

            var firstLetter = Char.ToLower(source[0]);
            return string.Concat(firstLetter.ToString(), source.Length > 1 ? source.Substring(1) : string.Empty);
        }

        public static string PropertyFirstToLower<TSource>(Expression<Func<TSource, object>> getter)
        {
            return Property(getter).FirstToLower();
        }

        public static string Property<TSource>(Expression<Func<TSource, object>> getter)
        {
            var lambda = (LambdaExpression)getter;

            MemberExpression memberExpression;
            var body = lambda.Body as UnaryExpression;
            if (body != null)
            {
                var unaryExpression = body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member.Name;
        }
    }
}
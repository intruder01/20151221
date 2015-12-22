using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;

namespace OpenHTM.CLA.Extensions
{
	public static class StreamExtensions
	{
		public static void WriteProperty<T> ( this StreamWriter stream, Expression<Func<T>> prop, string separator = ",," )
		{
			var body = ((MemberExpression)prop.Body);
			stream.WriteLine ( body.Member.Name + separator + 
				((FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value ));
		}

		public static void Check<T> ( Expression<Func<T>> expr )
		{
			var body = ((MemberExpression)expr.Body);
			Console.WriteLine ( "Name is: {0}", body.Member.Name );
			Console.WriteLine ( "Value is: {0}", ((FieldInfo)body.Member)
		   .GetValue ( ((ConstantExpression)body.Expression).Value ) );
		}

	}
}

using NHibernate;
using NHibernate.SqlCommand;
using System.Diagnostics;

namespace Data.Interceptors;

public sealed class OutputSqlInterceptor : EmptyInterceptor
{
    public override SqlString OnPrepareStatement(SqlString sql)
    {
        Debug.WriteLine(sql);

        return base.OnPrepareStatement(sql);
    }
}
﻿using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Migrations.Extensions;

public static class ColumnExtensions
{
    public static ICreateTableColumnOptionOrWithColumnSyntax WithIdColumn(this ICreateTableWithColumnSyntax builder)
    {
        return builder.WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity();
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax AsTimestampWithTimeZone(this ICreateTableColumnAsTypeSyntax column)
    {
        return column.AsDateTimeOffset();
    }

    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsTimestampWithTimeZone(this IAlterTableColumnAsTypeSyntax column)
    {
        return column.AsDateTimeOffset();
    }
}
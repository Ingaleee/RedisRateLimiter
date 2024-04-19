using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace HomeworkApp.Dal.Migrations
{
    [Migration(20230928000000)]
    public class AddModifiedAtAndDeletedAtFields : Migration
    {
        public override void Up()
        {
            Alter.Table("task_comments")
                .AddColumn("modified_at").AsDateTimeOffset().Nullable()
                .AddColumn("deleted_at").AsDateTimeOffset().Nullable();
        }

        public override void Down()
        {
            Delete.Column("modified_at").FromTable("task_comments");
            Delete.Column("deleted_at").FromTable("task_comments");
        }
    }
}

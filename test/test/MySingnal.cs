namespace test
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MySingnal : DbContext
    {
        public MySingnal()
            : base("name=MySingnal")
        {
        }

        public virtual DbSet<MySingnalSet> MySingnalSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

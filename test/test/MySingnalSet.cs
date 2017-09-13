namespace test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MySingnalSet")]
    public partial class MySingnalSet
    {
        public int Id { get; set; }

        public double C1 { get; set; }

        public double C2 { get; set; }

        public double C3 { get; set; }

        [Required]
        public string Target { get; set; }
    }
}

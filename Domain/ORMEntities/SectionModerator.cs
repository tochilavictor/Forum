namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SectionModerator")]
    public partial class SectionModerator
    {
        [Key]
        [Column(Order = 0)]
        public byte SectionId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateGranted { get; set; }

        public virtual Section Section { get; set; }

        public virtual User User { get; set; }
    }
}

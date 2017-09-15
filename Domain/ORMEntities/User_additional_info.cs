namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_additional_info
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [StringLength(50)]
        public string firstname { get; set; }

        [StringLength(50)]
        public string lastname { get; set; }

        [StringLength(50)]
        public string patronymic { get; set; }

        [StringLength(20)]
        public string phone { get; set; }

        [StringLength(50)]
        public string country { get; set; }

        [StringLength(50)]
        public string city { get; set; }

        [StringLength(50)]
        public string adress1 { get; set; }

        [StringLength(10)]
        public string adress2 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthdate { get; set; }

        [StringLength(250)]
        public string Describing { get; set; }

        public virtual User User { get; set; }
    }
}

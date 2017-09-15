namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Message()
        {
            Attached_Picture = new HashSet<Attached_Picture>();
            Message1 = new HashSet<Message>();
        }

        public int MessageId { get; set; }

        public int TopicId { get; set; }

        public int UserId { get; set; }

        public int? ParentMessageId { get; set; }

        [Required]
        public string Value { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Creation_date { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Last_update { get; set; }

        public bool? Contains_pictures { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attached_Picture> Attached_Picture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Message1 { get; set; }

        public virtual Message Message2 { get; set; }

        public virtual Topic Topic { get; set; }

        public virtual User User { get; set; }
    }
}

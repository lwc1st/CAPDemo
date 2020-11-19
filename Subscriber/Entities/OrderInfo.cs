using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Subscriber.EnumContainer;

#nullable disable

namespace Subscriber.Entities
{
    public partial class OrderInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(36)")]
        public Guid OrderCode { get; set; }
        [Required]
        [Column(TypeName = "varchar(36)")]
        public Guid UserCode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        public EnumStatus Status { get; set; }
    }
}

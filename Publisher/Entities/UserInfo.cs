using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Publisher.EnumContainer;

#nullable disable

namespace Publisher.Entities
{
    public partial class UserInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(36)")]
        public Guid UserCode { get; set; }
        [Required]
        [ConcurrencyCheck]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        public EnumStatus Status { get; set; }
    }
}

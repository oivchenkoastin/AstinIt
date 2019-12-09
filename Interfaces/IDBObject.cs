using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstinIt
{
    public interface IDBObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        Guid Id { get; set; }
        string Name { get; set; }
        bool Active { get; set; }
    }
}
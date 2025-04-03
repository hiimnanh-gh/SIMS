using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Models;

[Table("Faculty")]
public partial class Faculty
{
    [Key]
    public int FacultyId { get; set; }

    [StringLength(100)]
    public string FacultyName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

}

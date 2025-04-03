using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Models;
public partial class Major
{
    [Key]
    [Column("MajorID")]
    public int MajorId { get; set; }

    [StringLength(100)]
    public string MajorName { get; set; } = null!;

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [InverseProperty("Major")]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    [ForeignKey("DepartmentId")]
    [InverseProperty("Majors")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Major")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [ForeignKey("MajorId")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

}

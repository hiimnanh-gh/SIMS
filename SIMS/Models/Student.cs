using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Models;

public partial class Student
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("MajorID")]
    public int MajorId { get; set; }

    [Column("ClassID")]
    public int? ClassId { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Students")]
    public virtual Class? Class { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Student")]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    [ForeignKey("MajorId")]
    [InverseProperty("Students")]
    public virtual Major Major { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual User User { get; set; } = null!;


    


}

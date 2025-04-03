using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Models;

public partial class Grade
{
    [Key]
    [Column("GradeID")]
    public int GradeId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("CourseID")]
    public int CourseId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GradeDate { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Grades")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Grades")]
    public virtual Student Student { get; set; } = null!;
}

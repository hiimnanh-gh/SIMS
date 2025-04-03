using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Models;

public partial class Schedule
{
    [Key]
    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [Column("ClassID")]
    public int ClassId { get; set; }

    [Column("CourseID")]
    public int CourseId { get; set; }

    [Column("FacultyID")]
    public int FacultyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndTime { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Schedules")]
    public virtual Class Class { get; set; } = null!;

    [ForeignKey("CourseId")]
    [InverseProperty("Schedules")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("FacultyId")]
    [InverseProperty("Schedules")]
    public virtual User Faculty { get; set; } = null!;
}

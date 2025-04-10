﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagementWebApp.DTO;

public class Grade
{
    public int Id { get; set; }

    [Required]
    public string StudentId { get; set; }
    public Student? Student { get; set; }

    [Required]
    public int CourseId { get; set; }
    public Class? Class { get; set; }

    [Range(1, 100)]
    public int Value { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Setează data automat
    public DateTime DateAssigned { get; set; } = DateTime.UtcNow;
}
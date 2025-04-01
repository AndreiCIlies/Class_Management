﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ClassManagementWebAPI.Models;

public class Class
{

    public int Id { get; set; }
    public required string Name { get; set; }
    public  required DateTime StartDate { get; set; }
    public required  DateTime EndDate { get; set; }
    public required string TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public List<Student> Students { get; set; } = new List<Student>();
    public List<Grade> Grades { get; set; } = new List<Grade>();
}


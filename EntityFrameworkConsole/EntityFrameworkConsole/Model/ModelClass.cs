﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkConsole.Model
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("name=SchoolDBConnectionString")
        {
           
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

    }
    public class Student
    {
        public Student() { }

        public int StudentId { get; set; }
        [Required]
        public string StudentName { get; set; }
       
    }

    public class Course
    {
        public Course()
        {
           
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }

       
    }

}
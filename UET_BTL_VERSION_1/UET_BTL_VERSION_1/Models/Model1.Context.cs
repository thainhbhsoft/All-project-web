﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UET_BTL_VERSION_1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UetSurveyEntities : DbContext
    {
        public UetSurveyEntities()
            : base("name=UetSurveyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ContentSurvey> ContentSurvey { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentDetail> StudentDetail { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}

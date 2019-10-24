﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseOnline.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class STUDYONLINEEntities : DbContext
    {
        public STUDYONLINEEntities()
            : base("name=STUDYONLINEEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Coursework> Courseworks { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamConfig> ExamConfigs { get; set; }
        public DbSet<ExamTest> ExamTests { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }
        public DbSet<TestBatch> TestBatches { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
    }
}

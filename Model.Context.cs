﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Assesmentpaksod
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<M_Answer> M_Answer { get; set; }
        public virtual DbSet<M_Customers> M_Customers { get; set; }
        public virtual DbSet<M_Subject> M_Subject { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<T_Question> T_Question { get; set; }
        public virtual DbSet<T_QuestionDetail> T_QuestionDetail { get; set; }
        public virtual DbSet<T_Questionservay> T_Questionservay { get; set; }
    }
}

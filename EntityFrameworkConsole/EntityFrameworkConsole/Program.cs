﻿using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using EntityFrameworkConsole.Model;

namespace EntityFrameworkConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter command : (relation,entitystate,changetrack)");
            var cmd = Console.ReadLine();
            switch (cmd)
            {
                case "relation":
                    Relations();
                    break;
                case "entitystate":
                    EfEntityState();
                    break;
                case "changetrack":
                    EfChangeTrack();
                    break;
                default:
                    break;

            }

        }

        private static void Relations()
        {
            Console.WriteLine("Please enter command : (11,1m,mm)");
            var cmd = Console.ReadLine();
            switch (cmd)
            {
                case "11":
                    break;
                case "1m":
                    OneToMany();
                    break;
                case "mm":
                    break;
                default:
                    break;

            }
        }

        private static void OneToMany()
        {
            OneToManyInsert();
            //OneToManyModified();
            //OneToManyDelete();
            //Test1();
        }

        private static void Test1()
        {
            using (var ctx = new SchoolContext())
            {
                var dfs = ctx.Students.ToList();
            }
        }

        private static void OneToManyInsert()
        {
            Standard standard = new Standard();
            standard.Description = "Standard";
            Student stu1 = new Student { StudentName = "Ahasan",StudentTypeId = StudentType.General};
            Student stu2 = new Student { StudentName = "Hasam", StudentTypeId = StudentType.General };
            Student stu3 = new Student { StudentName = "Arif", StudentTypeId = StudentType.General };
            standard.Students.Add(stu1);
            standard.Students.Add(stu2);
            standard.Students.Add(stu3);

            using (var ctx = new SchoolContext())
            {
                ctx.Database.Log = Logger.Log;
                ctx.Standards.Add(standard);
                var result=ctx.SaveChanges();
                var test = ctx.Tests.ToList();
                Console.WriteLine("Insert completed.");
                Console.ReadLine();
                //var st = ctx.Students.FirstOrDefault();
                //var sta = st.Standard;
                //var dd = sta.Students;
            }
        }
        private static void OneToManyModified()
        {
            using (var ctx = new SchoolContext())
            {
                var standard = ctx.Standards.AsNoTracking().FirstOrDefault();
                if (standard!=null)
                {
                    
                    standard.Description = standard.Description+"_M";
                    foreach (Student student in standard.Students)
                    {
                        student.StudentName = student.StudentName + "_M";
                    }
                    //standard.Students.Clear();
                    //Student stu1 = new Student { StudentName = "Atique" };
                    //standard.Students.Add(stu1);
                    var standard1 = ctx.Standards.AsNoTracking().FirstOrDefault();
                    standard1.Students.ToList<Student>().ForEach(std => ctx.Entry(std).State = EntityState.Deleted);
                    ctx.SaveChanges();
                    ctx.Entry(standard).State=EntityState.Modified;
                    standard.Students.ToList<Student>().ForEach(std => ctx.Entry(std).State = EntityState.Added);
                    
                   
                    ctx.SaveChanges();
                    Console.WriteLine("Modified completed.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No modified data.");
                    Console.ReadLine();
                }
               
               
                
            }
        }
        private static void OneToManyDelete()
        {
            using (var ctx = new SchoolContext())
            {
                var standard = ctx.Standards.FirstOrDefault();
                if (standard != null)
                {

                    //ctx.Entry(standard).State = EntityState.Deleted;
                    //ctx.Standards.Remove(standard);
                    standard.Students.ToList<Student>().ForEach(std => ctx.Entry(std).State = EntityState.Deleted);
                    ctx.SaveChanges();
                    Console.WriteLine("Delete completed.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No deleted data.");
                    Console.ReadLine();
                }



            }
        }

        private static void EfEntityState()
        {
        }

        private static void EfChangeTrack()
        {
            using (var ctx=new SchoolContext())
            {
                
            }

        }
        private static void DisplayTrackedEntities(DbChangeTracker changeTracker)
        {
            Console.WriteLine("");

            var entries = changeTracker.Entries();
            foreach (var entry in entries)
            {
                Console.WriteLine("Entity Name: {0}", entry.Entity.GetType().FullName);
                Console.WriteLine("Status: {0}", entry.State);
            }
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------");
        }

        public  class Logger
        {
            public static void Log(string message)
            {
                Debug.Write(message);
            }
        }
    }
}

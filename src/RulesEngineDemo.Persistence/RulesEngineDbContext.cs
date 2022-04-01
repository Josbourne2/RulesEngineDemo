using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RulesEngine.Models;
using RulesEngineDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RulesEngineDemo.Persistence
{
    public class RulesEngineDbContext : DbContext
    {
        public RulesEngineDbContext(DbContextOptions<RulesEngineDbContext> options) : base(options) { }

        private JsonSerializerOptions _jsonSerializerOptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //As of 9/2021 with .NET 5, SQLite does not handle FK cascading deletes
            //options.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{System.IO.Path.DirectorySeparatorChar}RulesEngineEditor.db");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };

        }

        public DbSet<WorkflowContainer> Workflows { get; set; }
        //public DbSet<WorkflowData> Workflows { get; set; }
        //public DbSet<RuleData> Rules { get; set; }
        //public DbSet<ScopedParamData> ScopedParams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            int nameColumnLength = 255;
            int jsonColumnLength = 4000;

            modelBuilder.Entity<WorkflowContainer>(entity => {
                entity.ToTable("Workflows");
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();

                entity.Property(n => n.Workflow)
                    .IsRequired()
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, _jsonSerializerOptions),
                        v => JsonSerializer.Deserialize<Workflow>(v, _jsonSerializerOptions))
                    .HasJsonConversion()
                    .HasColumnType($"VARCHAR({jsonColumnLength})");

               



                //modelBuilder.Entity<WorkflowData>(entity => {
                //    entity.ToTable("Workflow");
                //    entity.HasKey(k => k.Id);
                //    entity.Property(p => p.Id).ValueGeneratedOnAdd();

                //    entity.Property(n => n.WorkflowName)
                //        .IsRequired()
                //        .HasMaxLength(nameColumnLength)
                //        .HasColumnType($"VARCHAR({nameColumnLength})");

                //    entity.Property(i => i.WorkflowsToInject)
                //          .HasConversion(splitStringConverter)
                //          .HasMaxLength(1024)
                //          .HasColumnType("VARCHAR(1024)");

                //    entity.HasMany(r => r.Rules).WithOne().OnDelete(DeleteBehavior.ClientCascade);
                //    entity.HasMany(g => g.GlobalParams).WithOne().OnDelete(DeleteBehavior.ClientCascade);
                //});

                //modelBuilder.Entity<RuleData>(entity => {
                //    entity.ToTable("Rule");
                //    entity.HasKey(k => k.Id);
                //    entity.Property(p => p.Id).ValueGeneratedOnAdd();

                //    entity.Property(n => n.RuleName)
                //        .IsRequired()
                //        .HasMaxLength(nameColumnLength)
                //        .HasColumnType($"VARCHAR({nameColumnLength})");

                //    entity.Property(o => o.Operator)
                //        .HasColumnType("VARCHAR(25)")
                //        .HasMaxLength(25);

                //    entity.Property(e => e.Expression)
                //        .HasColumnType("VARCHAR(4000)")
                //        .HasMaxLength(4000);

                //    entity.Ignore(i => i.IsSuccess);
                //    entity.Ignore(i => i.ErrorMessage);
                //    entity.Ignore(i => i.ExceptionMessage);
                //    entity.Property(i => i.WorkflowsToInject)
                //          .HasConversion(splitStringConverter)
                //          .HasMaxLength(1024)
                //          .HasColumnType("VARCHAR(1024)");

                //    //TODO: Remove when RE version >= 4
                //    entity.Ignore(i => i.WorkflowRulesToInject);

                //    entity.Property(b => b.Properties)
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, _jsonSerializerOptions),
                //        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, _jsonSerializerOptions))
                //    .HasJsonConversion()                
                //    .HasMaxLength(jsonColumnLength)
                //    .HasColumnType($"VARCHAR({jsonColumnLength})");

                //    entity.Property(p => p.Actions)
                //    .HasConversion(
                //        v => JsonSerializer.Serialize(v, _jsonSerializerOptions),
                //        v => JsonSerializer.Deserialize<RuleActions>(v, _jsonSerializerOptions))
                //    .HasColumnType($"VARCHAR({jsonColumnLength})")
                //    .HasMaxLength(jsonColumnLength);

                //    entity.Property(s => s.SuccessEvent)
                //        .HasColumnType("VARCHAR(255)")
                //        .HasMaxLength(255);

                //    entity.HasMany(r => r.Rules).WithOne().OnDelete(DeleteBehavior.ClientCascade);
                //    entity.HasMany(g => g.LocalParams).WithOne().OnDelete(DeleteBehavior.ClientCascade);
                //});

                //modelBuilder.Entity<ScopedParamData>(entity => {
                //    entity.ToTable("ScopedParam");
                //    entity.HasKey(k => k.Id);
                //    entity.Property(p => p.Id).ValueGeneratedOnAdd();

                //    entity.Property(n => n.Name)
                //        .IsRequired()
                //        .HasMaxLength(nameColumnLength)
                //        .HasColumnType($"VARCHAR({nameColumnLength})");

                //    entity.Property(e => e.Expression)
                //        .IsRequired()
                //        .HasColumnType("VARCHAR(4000)")
                //        .HasMaxLength(4000);
            });
        }
    }
    public static class JSONHelper
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };

            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<T>(v, options) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, options) == JsonSerializer.Serialize(r, options),
                v => v == null ? 0 : JsonSerializer.Serialize(v, options).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, options), options)
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}

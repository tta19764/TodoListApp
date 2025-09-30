using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Data
{
    /// <summary>
    /// Database context for the To-Do List application, integrating ASP.NET Core Identity for user management.
    /// </summary>
    public class TodoListDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for to-do list roles.
        /// </summary>
        public DbSet<TodoListRole> TodoListRoles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for to-do lists.
        /// </summary>
        public DbSet<TodoList> TodoLists { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for to-do tasks.
        /// </summary>
        public DbSet<TodoTask> TodoTasks { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for tags.
        /// </summary>
        public DbSet<Tag> Tags { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for comments.
        /// </summary>
        public DbSet<Comment> Comments { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for to-do list user roles.
        /// </summary>
        public DbSet<TodoListUserRole> TodoListUserRoles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for statuses.
        /// </summary>
        public DbSet<Status> Statuses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DbSet for task-tags relationships.
        /// </summary>
        public DbSet<TaskTags> TaskTags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            base.OnModelCreating(builder);

            // Configure composite keys
            _ = builder.Entity<TodoListUserRole>()
                .HasIndex(lur => new { lur.TodoListId, lur.UserId })
                .IsUnique();

            _ = builder.Entity<TaskTags>()
                .HasIndex(tt => new { tt.TagId, tt.TaskId })
                .IsUnique();

            // Configure TodoListUserRole relationships
            _ = builder.Entity<TodoListUserRole>()
                .HasOne(lur => lur.ListUser)
                .WithMany(u => u.TodoListUserRoles)
                .HasForeignKey(lur => lur.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            _ = builder.Entity<TodoListUserRole>()
                .HasOne(lur => lur.List)
                .WithMany(l => l.TodoListUserRoles)
                .HasForeignKey(lur => lur.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.Entity<TodoListUserRole>()
                .HasOne(lur => lur.ListRole)
                .WithMany(r => r.TodoListUserRoles)
                .HasForeignKey(lur => lur.TodoListRoleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent role deletion if in use

            // Configure TodoTask relationships
            _ = builder.Entity<TodoTask>()
                .HasOne(t => t.Status)
                .WithMany(s => s.TodoTasks)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent status deletion if tasks exist

            _ = builder.Entity<TodoTask>()
                .HasOne(t => t.OwnerUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            _ = builder.Entity<TodoTask>()
                .HasOne(t => t.TodoList)
                .WithMany(l => l.TodoTasks)
                .HasForeignKey(t => t.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TodoList relationships
            _ = builder.Entity<TodoList>()
                .HasOne(l => l.ListOwner)
                .WithMany(u => u.Lists)
                .HasForeignKey(l => l.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Comment relationships
            _ = builder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Configure Tag relationships
            _ = builder.Entity<Tag>()
                .HasOne(t => t.TagAuthor)
                .WithMany(u => u.UserTags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TaskTags relationships
            _ = builder.Entity<TaskTags>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.Entity<TaskTags>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Configure indexes for better performance
            _ = builder.Entity<TodoTask>()
                .HasIndex(t => t.DueDate)
                .HasDatabaseName("IX_TodoTask_DueDate");

            _ = builder.Entity<TodoTask>()
                .HasIndex(t => t.StatusId)
                .HasDatabaseName("IX_TodoTask_StatusId");

            _ = builder.Entity<Tag>()
                .HasIndex(t => new { t.UserId, t.Label })
                .IsUnique()
                .HasDatabaseName("IX_Tag_UserId_Label");

            _ = builder.Entity<Status>()
                .HasData(
                    new Status(1, "Not Started"),
                    new Status(2, "In Progress"),
                    new Status(3, "Completed"));

            _ = builder.Entity<TodoListRole>()
                .HasData(
                    new TodoListRole(1, "Viewer"),
                    new TodoListRole(2, "Editor"));
        }
    }
}

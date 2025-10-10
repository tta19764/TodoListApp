// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.TodoTasksController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.AuthController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.TodoListsController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.CustomLogs.AuthLog")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.CustomLogs.TodoListsLog")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.CustomLogs.TodoTasksLog")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.AssignedTasksController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.SearchTasksController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.TagsController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Controllers.TaskTagsController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Migrations.TagFix")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Migrations.SeedData")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Migrations.AllBaseEntity")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.Migrations.Initial")]
[assembly: SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "Doesn't work othervise.", Scope = "member", Target = "~F:TodoListApp.WebApi.Migrations.SeedData.StatusesValues")]
[assembly: SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "Doesn't work othervise.", Scope = "member", Target = "~F:TodoListApp.WebApi.Migrations.SeedData.TodoListRolesValues")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApi.CustomLogs.GlobalExceptionLog")]

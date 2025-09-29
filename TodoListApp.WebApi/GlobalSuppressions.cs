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

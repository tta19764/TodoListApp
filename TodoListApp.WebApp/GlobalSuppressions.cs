// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Controllers.AccountController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Controllers.HomeController")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.CustomLogs.AccountLog")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Data.AppUser")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Data.Migrations.AppUser")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Handlers.JwtTokenHandler")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Models.ChangePasswordViewModel")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Models.LoginViewModel")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Models.RegisterViewModel")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Models.VerifyEmailViewModel")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Apis controllers do not work when internal.", Scope = "type", Target = "~T:TodoListApp.WebApp.Services.IdentityTokenStorageService")]

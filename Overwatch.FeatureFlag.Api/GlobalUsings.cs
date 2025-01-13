// Global using directives

global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.Reflection;
global using System.Text.Json.Serialization;
global using Microsoft.AspNetCore.Mvc;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Overwatch.FeatureFlag.Api.Application.Handlers.Environments;
global using Overwatch.FeatureFlag.Api.ErrorHandling;
global using Overwatch.FeatureFlag.Api.Persistence;
global using Environment = Overwatch.FeatureFlag.Interface.Models.Environment;

global using Overwatch.FeatureFlag.Api.Endpoints.Environments;
global using Overwatch.FeatureFlag.Api.Endpoints.Features;
global using Overwatch.FeatureFlag.Api.Endpoints.Versions;
global using Overwatch.FeatureFlag.Interface;
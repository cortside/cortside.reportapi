using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Cortside.Health.Controllers;
using Cortside.SqlReportApi.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cortside.SqlReportApi.WebApi.IntegrationTests.Tests {
    public class DependencyInjectionTest : IClassFixture<IntegrationTestFactory<Startup>> {
        private readonly IntegrationTestFactory<Startup> fixture;

        public DependencyInjectionTest(IntegrationTestFactory<Startup> fixture) {
            this.fixture = fixture;
        }

        [Fact]
        public void VerifyControllerResolution() {
            var controllersAssembly = typeof(HealthController).Assembly;
            var controllers = controllersAssembly.ExportedTypes.Where(x => typeof(ControllerBase).IsAssignableFrom(x) && !x.IsAbstract).ToList();

            controllersAssembly = typeof(AuthorizationController).Assembly;
            controllers.AddRange(controllersAssembly.ExportedTypes.Where(x => typeof(ControllerBase).IsAssignableFrom(x) && !x.IsAbstract));

            var activator = fixture.Services.GetService<IControllerActivator>();
            var serviceProvider = fixture.Services.GetService<IServiceProvider>();
            var errors = new Dictionary<Type, Exception>();

            var count = 0;
            var min = long.MaxValue;
            var max = long.MinValue;
            long total = 0;
            var slowest = string.Empty;

            foreach (var controllerType in controllers) {
                try {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var actionContext = new ActionContext(
                        new DefaultHttpContext {
                            RequestServices = serviceProvider
                        },
                        new RouteData(),
                        new ControllerActionDescriptor {
                            ControllerTypeInfo = controllerType.GetTypeInfo()
                        });
                    var controller = activator.Create(new ControllerContext(actionContext));

                    stopwatch.Stop();

                    if (stopwatch.ElapsedMilliseconds > max) {
                        max = stopwatch.ElapsedMilliseconds;
                        slowest = controller.GetType().ToString();
                    }
                    if (stopwatch.ElapsedMilliseconds < min) {
                        min = stopwatch.ElapsedMilliseconds;
                    }
                    count++;
                    total += stopwatch.ElapsedMilliseconds;

                    if (stopwatch.ElapsedMilliseconds > 100) {
                        Console.Out.WriteLine($"Resolved controller {controller.GetType()} in {stopwatch.ElapsedMilliseconds}ms");
                    }
                } catch (Exception ex) {
                    Console.Out.WriteLine($"Failed to resolve controller {controllerType} due to {ex}");
                    errors.Add(controllerType, ex);
                }
            }
            Console.Out.WriteLine($"The slowest controller to resolve was {slowest} in {max}ms");
            Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors.Select(x => $"Failed to resolve controller {x.Key.Name} due to {x.Value}")));
        }
    }
}

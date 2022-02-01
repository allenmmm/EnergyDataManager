using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Xunit;

namespace given_an_energy_data_manager.web.tests
{
    public class when_creating_http_error_responses
    {
        public static IEnumerable<object[]> TestExceptionData =>
        new List<object[]> {
            new object[] {
                new DbUpdateConcurrencyException(), HttpStatusCode.Conflict
            },
            new object[] {
                new DbUpdateException(), HttpStatusCode.InternalServerError
            },
            new object[] {
                new ArgumentOutOfRangeException(), HttpStatusCode.BadRequest
            },
            new object[] {
                new InvalidProgramException(), HttpStatusCode.InternalServerError
            },
            new object[]
            {
                new FileNotFoundException(), HttpStatusCode.InternalServerError
            }
        };

        private readonly ActionContext _ActionContext;
        public when_creating_http_error_responses()
        {
            _ActionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
        }

        [Theory]
        [MemberData(nameof(TestExceptionData))]
        public void then_create_error_response_from_exception(
            Exception exceptionEXP,
            HttpStatusCode statusCodeEXP)
        {
            //ARRANGE
            var exceptionContext = new ExceptionContext(
                _ActionContext,
                new Mock<IList<IFilterMetadata>>().Object)
            {
                Exception = exceptionEXP
            };

            //ACT
            var sut = new
                EnergyDataManager.Web.DomainServices.HttpErrorResponse(exceptionContext);

            //ASSERT
            sut.Path.Should()
                .Be(exceptionContext.HttpContext.Request.Path);
            sut.StatusCode.Should().Be(statusCodeEXP);
            sut.Error.Should().NotBeEmpty();
            sut.Message.Should().NotBeEmpty();
        }

        [Fact]
        public void then_serialise_error_response()
        {
            //ARRANGE
            var exceptionContext = new ExceptionContext(
                _ActionContext,
                new Mock<IList<IFilterMetadata>>().Object)
            {
                Exception = new ArgumentNullException()
            };

            var sut = new
                EnergyDataManager.Web.DomainServices.HttpErrorResponse(exceptionContext);

            //ACT 
            var responseACT = sut.SerialiseResponse();

            //ASSERT
            responseACT.Should().Contain("400");

        }

        [Theory]
        [MemberData(nameof(TestExceptionData))]
        public void handle_exception_event(
               Exception exceptionEXP,
               HttpStatusCode statusCodeEXP)
        {
            //ARRANGE
            _ActionContext.HttpContext.Response.Body = new MemoryStream();
            var exceptionContext = new ExceptionContext(
                _ActionContext,
                new Mock<IList<IFilterMetadata>>().Object)
            {
                Exception = exceptionEXP
            };

            var sut =
                new EnergyDataManager.Web.Filters.ExceptionFilter();

            //ACT
            sut.OnException(exceptionContext);

            //ASSERT
            exceptionContext.HttpContext.Response.Body.Position = 0;
            exceptionContext.ExceptionHandled.Should().BeTrue();
            var bodyContentACT = "";
            using (var sr = new StreamReader
                (exceptionContext.HttpContext.Response.Body))
                bodyContentACT = sr.ReadToEnd();

            var httpErrorResponseACT = JsonSerializer.
                Deserialize<EnergyDataManager.Web.DomainServices.HttpErrorResponse>(bodyContentACT);

            httpErrorResponseACT.StatusCode.Should().Be(statusCodeEXP);
            httpErrorResponseACT.Path.Should()
                .Be(exceptionContext.HttpContext.Request.Path);
            httpErrorResponseACT.Error.Should().NotBeEmpty();
            httpErrorResponseACT.Message.Should().NotBeEmpty();
            exceptionContext.HttpContext.Response.StatusCode
                .Should().Be((int)statusCodeEXP);
            exceptionContext.HttpContext.Response.ContentType.ToString()
                .Should().Contain("application/json");
        }
    }
}

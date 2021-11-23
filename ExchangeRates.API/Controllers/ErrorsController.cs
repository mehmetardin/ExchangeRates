using ExchangeRates.API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("error")]
    public ErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error; 
        var code = 500; // Internal Server Error by default

        if (exception is ArgumentOutOfRangeException) 
            code = 400; // Bad Request
        if (exception is KeyNotFoundException)
            code = 400; // Bad Request

        Response.StatusCode = code; 

        return new ErrorResponse { Error = context.Error.Message.ToString() }; 
    }
}
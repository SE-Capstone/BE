﻿using Capstone.Api.Common.ResponseApi.Controllers;
using Capstone.Api.Common.ResponseApi.Model;
using Capstone.Api.Module.Projects.Request;
using Capstone.Application.Module.Auth.Command;
using Capstone.Application.Module.Auths.Command;
using Capstone.Application.Module.Auths.Response;
using Capstone.Application.Module.Projects.Command;
using Capstone.Application.Module.Projects.Query;
using Capstone.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone.Api.Module.Projects.Controlers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [Authorize(Roles = "ADD_PROJECT")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand request)
        {
            var result = await _mediator.Send(request);
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseOk(result.Data);
            else
            {
                if (result.StatusCode == 404)
                    return ResponseNotFound(messageResponse: result.ErrorMessage);
                return ResponseBadRequest(messageResponse: result.ErrorMessage);

            }
        }

        [HttpPut("{id}")]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [Authorize(Roles = "UPDATE_PROJECT")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectRequest request)
        {
            var result = await _mediator.Send(new UpdateProjectCommand(id, request.Name, request.Code, request.Description, request.StartDate, request.EndDate, request.LeadId, request.Status));
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseOk(result.Data);
            else
            {
                if (result.StatusCode == 404)
                    return ResponseNotFound(messageResponse: result.ErrorMessage);
                return ResponseBadRequest(messageResponse: result.ErrorMessage);

            }
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        public async Task<IActionResult> GetListProject(int? pageIndex,int? pageSize, bool? isVisible, ProjectStatus? status, string? search )
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _mediator.Send(new GetListProjectQuery(pageIndex, pageSize, isVisible, status, token, search));
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseOk(result.Data, result.Paging);
            else
            {
                return ResponseBadRequest(messageResponse: result.ErrorMessage);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [Authorize(Roles = "DELETE_PROJECT")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _mediator.Send(new DeleteProjectCommand() { Id = id});
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseNoContent();
            else
            {
                return ResponseNotFound(messageResponse: result.ErrorMessage);
            }
        }

        [HttpGet("{id}")]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [Authorize(Roles = "GET_DETAIL_PROJECT")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var result = await _mediator.Send(new GetDetailProjectQuery() { Id = id });
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseOk(dataResponse: result.Data);
            else
            {
                return ResponseNotFound(messageResponse: result.ErrorMessage);
            }
        }

        [HttpPut("{id}/visible/toggle")]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [Authorize(Roles = "TOGGLE_VISIBLE_PROJECT")]
        public async Task<IActionResult> ToggleVisible(Guid id)
        {
            var result = await _mediator.Send(new ToggleProjectCommand() { Id = id });
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseOk(dataResponse: result.Data);
            else
            {
                return ResponseNotFound(messageResponse: result.ErrorMessage);
            }
        }

        [HttpPost("members")]
        [SwaggerResponse(400, "Fail", typeof(ResponseFail))]
        [Authorize(Roles = "ADD_MEMBER_TO_PROJECT")]
        public async Task<IActionResult> AddMember(AddMembersToProject request)
        {
            var result = await _mediator.Send(request);
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return ResponseNoContent();
            else
            {
                if(result.StatusCode == 404)
                    return ResponseNotFound(messageResponse: result.ErrorMessage);
                else
                    return ResponseBadRequest(messageResponse: result.ErrorMessage);
            }
        }
    }
}

using Conways.Service.Application.Abstractions;
using Conways.Service.Application.Boards.CreateBoard;
using Conways.Service.Application.Boards.GetBoard;
using Conways.Service.Domain.Boards;
using Conways.Service.HttpApi.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace Showcase.ShopManagement.HttpApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateBoardResponse>> CreateBoardAsync
    (
        [FromBody] CreateBoardRequest request,
        [FromServices] ICommandHandler<CreateBoardCommand, CreateBoardResult> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateBoardCommand(request.InitialGrid);

        var result = await handler.HandleAsync(command, cancellationToken);

        return Ok(new CreateBoardResponse(result.BoardId.Value));
    }

    [HttpGet("{boardId}")]
    public async Task<ActionResult<GetBoardResponse>> GetBoardAsync
    (
        [FromRoute] Guid boardId,
        [FromServices] IQueryHandler<GetBoardQuery, GetBoardResult> service
    )
    {
        var query = new GetBoardQuery(new BoardId(boardId));

        var result = await service.HandleAsync(query, CancellationToken.None);

        return Ok(GetBoardResponse.From(result.CurrentState));
    }
}
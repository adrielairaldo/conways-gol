using Conways.Service.Application.Abstractions;
using Conways.Service.Application.Boards.AdvanceBoard;
using Conways.Service.Application.Boards.CreateBoard;
using Conways.Service.Application.Boards.GetBoard;
using Conways.Service.Application.Boards.SimulateUntilConclusion;
using Conways.Service.Domain.Boards;
using Conways.Service.HttpApi.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace Showcase.ShopManagement.HttpApi.Controllers;

/// <summary>
/// Provides endpoints to manage and simulate Conway's Game of Life boards.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly ILogger<BoardsController> _logger;

    public BoardsController(ILogger<BoardsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Creates a new board with an initial grid configuration.
    /// </summary>
    /// <param name="request">The initial grid setup.</param>
    /// <param name="handler">The command handler for creating boards.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created board.</returns>
    /// <response code="200">Returns the unique ID of the created board.</response>
    /// <response code="400">If the initial grid is invalid (empty or inconsistent rows).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateBoardResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CreateBoardResponse>> CreateBoardAsync
    (
        [FromBody] CreateBoardRequest request,
        [FromServices] ICommandHandler<CreateBoardCommand, CreateBoardResult> handler,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("HTTP Request: Creating a new board.");

        var command = new CreateBoardCommand(new Grid(request.InitialGrid));

        var result = await handler.HandleAsync(command, cancellationToken);

        return Ok(new CreateBoardResponse(result.BoardId.Value));
    }

    /// <summary>
    /// Retrieves the current state and generation of a specific board.
    /// </summary>
    /// <param name="boardId">The unique ID of the board.</param>
    /// <param name="service">The query handler for retrieving boards.</param>
    /// <returns>The current grid and generation number.</returns>
    /// <response code="200">Returns the board state.</response>
    /// <response code="404">If the board ID does not exist.</response>
    [HttpGet("{boardId:guid}")]
    [ProducesResponseType(typeof(GetBoardResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GetBoardResponse>> GetBoardAsync
    (
        [FromRoute] Guid boardId,
        [FromServices] IQueryHandler<GetBoardQuery, GetBoardResult> service
    )
    {
        _logger.LogInformation("HTTP Request: Fetching board {BoardId}.", boardId);

        var query = new GetBoardQuery(new BoardId(boardId));

        var result = await service.HandleAsync(query, CancellationToken.None);

        return Ok(GetBoardResponse.From(result.CurrentState));
    }

    /// <summary>
    /// Manually advances the board by a specific number of steps.
    /// </summary>
    /// <param name="boardId">The ID of the board to advance.</param>
    /// <param name="request">Number of steps to move forward.</param>
    /// <param name="handler">The command handler for advancing boards.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns the updated board state.</response>
    /// <response code="404">If the board was not found.</response>
    [HttpPost("{boardId:guid}/advance")]
    [ProducesResponseType(typeof(AdvanceBoardResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AdvanceBoardResponse>> AdvanceBoardAsync
    (
        [FromRoute] Guid boardId,
        [FromBody] AdvanceBoardRequest request,
        [FromServices] ICommandHandler<AdvanceBoardCommand, AdvanceBoardResult> handler,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("HTTP Request: Advancing board {BoardId} by {Steps} steps.", boardId, request.Steps);

        var command = new AdvanceBoardCommand
        (
            BoardId: new BoardId(boardId),
            Steps: request.Steps
        );

        var result = await handler.HandleAsync(command, cancellationToken);

        return Ok(AdvanceBoardResponse.From(result.CurrentState));
    }

    /// <summary>
    /// Runs a simulation until the board stabilizes, starts repeating, or hits a limit.
    /// </summary>
    /// <param name="boardId">The ID of the board to simulate.</param>
    /// <param name="request">Maximum iterations allowed for this simulation.</param>
    /// <param name="handler">The command handler for simulation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns the final state and the reason the simulation stopped.</response>
    /// <response code="404">If the board was not found.</response>
    [HttpPost("{boardId:guid}/simulate-to-end")]
    [ProducesResponseType(typeof(SimulateUntilConclusionResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<SimulateUntilConclusionResponse>> SimulateUntilConclusionAsync
    (
        [FromRoute] Guid boardId,
        [FromBody] SimulateUntilConclusionRequest request,
        [FromServices] ICommandHandler<SimulateUntilConclusionCommand, SimulateUntilConclusionResult> handler,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("HTTP Request: Simulating board {BoardId} to conclusion.", boardId);

        var command = new SimulateUntilConclusionCommand
        (
            BoardId: new BoardId(boardId),
            MaxIterations: request.MaxIterations
        );

        var result = await handler.HandleAsync(command, cancellationToken);

        return Ok(SimulateUntilConclusionResponse.From(result.SimulationResult));
    }
}
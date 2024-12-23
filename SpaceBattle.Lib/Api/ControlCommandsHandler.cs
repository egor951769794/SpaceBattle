namespace SpaceBattle.Lib;

using Hwdtech;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class RequestsHandler : ControllerBase
{
	private readonly ShardedThreadsMessagesInterpreter threadsInterpreter;

	public RequestsHandler(ShardedThreadsMessagesInterpreter threadsInterpreter)
	{
		this.threadsInterpreter = threadsInterpreter;
	}

    [HttpPost("/game/migrateOrder")]
	public IActionResult PrepareGame([FromHeader(Name = "Game-Id")] string gameId, [FromBody] MigrateOrderBody orderBody)
	{
        threadsInterpreter.sendGameMigration(orderBody.threadId, gameId, orderBody.serverUrl);

		return Ok();
	}
	[HttpPost("/game/migrate")]
	public IActionResult MigrateGame([FromHeader(Name = "Thread-Id")] string threadId, [FromBody] string serializedGame)
	{
        threadsInterpreter.sendSerializedGame(serializedGame, threadId);

		return Ok();
	}
}
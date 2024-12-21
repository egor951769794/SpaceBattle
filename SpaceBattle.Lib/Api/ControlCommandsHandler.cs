namespace SpaceBattle.Lib;

using Hwdtech;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class SpaceBattleController : ControllerBase
{
	private readonly ShardedThreadsMessagesInterpreter threadsInterpreter;

	public SpaceBattleController(ShardedThreadsMessagesInterpreter threadsInterpreter)
	{
		this.threadsInterpreter = threadsInterpreter;
	}

    [HttpPost("/game/migrateOrder")]
	public IActionResult PrepareGame([FromHeader(Name = "Game-Id")] string gameId, [FromBody] string serverUrl, [FromBody] string threadId)
	{
        threadsInterpreter.sendGameMigration(threadId, gameId, serverUrl);

		return Ok();
	}
	[HttpPost("/game/migrate")]
	public IActionResult MigrateGame([FromHeader(Name = "Thread-Id")] string threadId, [FromBody] string serializedGame)
	{
        threadsInterpreter.sendSerializedGame(serializedGame, threadId);

		return Ok();
	}
}
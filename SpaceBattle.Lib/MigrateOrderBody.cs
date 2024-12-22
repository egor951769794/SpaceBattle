namespace SpaceBattle.Lib;


public class MigrateOrderBody
{
	public string serverUrl {get; set;}
    public string threadId {get; set;}
    public MigrateOrderBody(string serverUrl, string threadId)
    {
        this.serverUrl = serverUrl;
        this.threadId = threadId;
    }
}
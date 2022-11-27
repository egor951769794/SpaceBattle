namespace SpaceBattle.Lib;

public interface IStrategy {
    object Execute(params object[] args); 
}

public class IoC {
    static Dictionary<string, IStrategy> store;

    static IoC() {
        store = new Dictionary<string, IStrategy>();
        store["IoC.Add"] = new IoCAddStrategy(store);
        store["IoC.Resolve"] = new IoCAddStrategy(store);
    }
    public static T Resolve<T> (string key, params object[] args) {
        return (T) store["IoC.Resolve"].Execute(key, args);
    }
}

class IoCAddCommand : ICommand {
    private IDictionary<string, IStrategy> store;
    private string key;
    private IStrategy strategy;
    public IoCAddCommand(IDictionary<string, IStrategy> store, string key, IStrategy strategy) {
        this.store = store;
        this.key = key;
        this.strategy = strategy;
    }
    public void Execute() {
        this.store[this.key] = this.strategy;
    }
}

class IoCAddStrategy : IStrategy {
    private IDictionary<string, IStrategy> store;
    public IoCAddStrategy(IDictionary<string, IStrategy> store) {
        this.store = store;
    }
    public object Execute(params object[] args) {
        string key = (string) args[0];
        IStrategy strategy = (IStrategy) args[1];
        return new IoCAddCommand(this.store, key, strategy);
    }
}
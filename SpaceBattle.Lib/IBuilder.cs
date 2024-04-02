namespace SpaceBattle.Lib;

public interface IBuilder
{
    IBuilder Config(string param, params object[] argv);

    object GetOrCreate();
}

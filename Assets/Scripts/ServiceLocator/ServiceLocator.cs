using System.Collections.Generic;
using System;

/*
* The idea here is to have a kind of Dependency Injection, but here we only set type to instance and initialize whichever class has IInitializable.
* Since there is no real injection, but each class is responsible of getting its dependencies, it is cold a provider.
* It is important to know that this class Awake runs before the Awake of all the other classes, the same goes to the Start.
* This way we have a good idea of the game logic initiation this.Awake -> all other Awakes -> thisStart -> all other Starts
* and can use this for the initiation of dependencies in the game.
* 
* Since this is not a MonoBehaviour class and cannot run on itself, see DependenciesProviderLever, which is responsible for calling Awake, Start, etc.
*/
public class ServiceLocator
{
    private static ServiceLocator instance;

    private Kernel container;

    private ServiceLocator()
    {
	    container = new Kernel();
    }

    public static ServiceLocator Instance
    {
	    get
	    {
		    if (instance == null)
		    {
			    instance = new ServiceLocator();
		    }

		    return instance;
	    }
    }

    public void Clear()
    {
	    instance.container.Clear();
    }

    public void Awake(Dictionary<Type, object> monobehaviours)
    {
	    //container.Add<TicksManager>(monobehaviours[typeof(TicksManager)] as TicksManager);
	    //container.Add<TranslationsManager>(new TranslationsManager());
    }

    public void Start()
    {
	    var initilizables = container.GetAllOfType<IInitializable>();

	    for (int i = 0; i < initilizables.Count; i++)
	    {
		    initilizables[i].Initilize(Instance);
		    //Debug.Log("Initialized: " + initilizables[i].GetType().Name);
	    }
    }

    public void LateStart()
    {
	    var initilizables = container.GetAllOfType<ILateInitializable>();

	    for (int i = 0; i < initilizables.Count; i++)
	    {
		    initilizables[i].LateInitilize(Instance);
		    //Debug.Log("Late Initialized: " + initilizables[i].GetType().Name);
	    }
    }

    public void Dispose()
    {
	    var initilizables = container.GetAllOfType<IIsDisposable>();

	    for (int i = 0; i < initilizables.Count; i++)
	    {
		    initilizables[i].Dispose();
		    //Debug.Log("Disposed: " + initilizables[i].GetType().Name);
	    }
    }

    public void AddInstanceOfType<T>(T instance)
	    where T : class
    {
	    container.Add<T>(instance);
    }

    public T GetInstanceOfType<T>()
	    where T: class
    {
	    return container.GetInstanceOfType<T>();
    }

    public List<T> GetInstancesOfType<T>()
		where T : class
	{
		return container.GetAllOfType<T>();
	}
}

public interface IInitializable
{
void Initilize(ServiceLocator dependenciesProvider);
}

public interface ILateInitializable
{
void LateInitilize(ServiceLocator dependenciesProvider);
}

public interface IIsDisposable
{
void Dispose();
}
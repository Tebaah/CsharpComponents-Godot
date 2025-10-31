using Godot;
using System;

/// <summary>
/// StateMachine<T> gestiona la transición y delegación de eventos entre diferentes estados de un nodo controlado en Godot.
/// 
/// - Permite definir un estado por defecto y cambiar entre estados dinámicamente.
/// - Delegación centralizada de eventos de Godot al estado activo.
/// - Manejo centralizado de errores y logs.
/// - Uso de genéricos para tipado fuerte del nodo controlado y los estados.
/// 
/// Uso típico:
/// 1. Crea una subclase concreta (por ejemplo, PlayerStateMachine : StateMachine<PlayerController>) y adjúntala al nodo en la escena.
/// 2. Asigna el nodo a controlar y el estado por defecto desde el editor o por código.
/// 3. Los estados deben heredar de StateBase<T> y estar presentes como hijos de la máquina de estados.
/// 4. Llama a SwitchToTheNewState("NombreDelEstado") para cambiar de estado en tiempo de ejecución.
/// </summary>
public partial class StateMachine<T> : Node where T : Node
{
    #region Variables
    /// <summary>
    /// Nodo que será controlado por la máquina de estados. Se exporta como Node para compatibilidad con Godot.
    /// </summary>
    [Export] public Node NodeToBeControlled { get; set; }
    /// <summary>
    /// Estado por defecto que se activa al iniciar la máquina de estados. Se exporta como Node para compatibilidad con Godot.
    /// </summary>
    [Export] public Node DefaultNodeState { get; set; }
    /// <summary>
    /// Estado actualmente activo, con tipado fuerte.
    /// </summary>
    public StateBase<T> currentNodeState = null;

    #endregion

    #region Godot Methods

    public override void _Ready()
    {
        if (NodeToBeControlled == null)
        {
            GD.PrintErr("Node to be controlled is not assigned");
            return;
        }

        if (DefaultNodeState != null)
        {
            CallDeferred(nameof(StartTheDefaultState));
        }
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Inicia el estado actual, asignando referencias y llamando el método de inicio del estado.
    /// </summary>
    public void StartThecurrentNodeState()
    {
        if (currentNodeState == null || NodeToBeControlled == null)
        {
            HandleError("currentNodeState o NodeToBeControlled es null en StateStart");
            return;
        }

        LogStateChange($"StateMachine {NodeToBeControlled.Name} State Start {currentNodeState.Name}");

        currentNodeState.Setup(NodeToBeControlled, this);
        currentNodeState.AtTheStartOfTheState();
    }

    /// <summary>
    /// Inicia el estado por defecto.
    /// </summary>
    public void StartTheDefaultState()
    {
        currentNodeState = DefaultNodeState as StateBase<T>;
        StartThecurrentNodeState();
    }

    /// <summary>
    /// Cambia al estado indicado por nombre.
    /// </summary>
    /// <param name="newState">Nombre del nuevo estado</param>
    public void SwitchToTheNewState(string newState)
    {
        if (currentNodeState != null && currentNodeState.HasMethod("AtTheEndOfTheState"))
        {
            currentNodeState.AtTheEndOfTheState();
        }

        var stateNode = GetNode(newState);
        if (stateNode is StateBase<T> newStateInstance)
        {
            currentNodeState = newStateInstance;
            newStateInstance.Setup(NodeToBeControlled, this);
        }
        else
        {
            HandleError($"Failed to change to state: {newState} (type mismatch)");
            return;
        }

        StartThecurrentNodeState();
    }

    public override void _Process(double delta)
    {
        DelegateEvent(() => currentNodeState?.OnProcess(delta));
    }

    public override void _PhysicsProcess(double delta)
    {
        DelegateEvent(() => currentNodeState?.OnPhysicsProcess(delta));
    }

    public override void _Input(InputEvent @event)
    {
        DelegateEvent(() => currentNodeState?.OnInput(@event));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        DelegateEvent(() => currentNodeState?.OnUnhandledInput(@event));
    }

    public void _UnhandledKeyInput(InputEventKey @event)
    {
        DelegateEvent(() => currentNodeState?.OnUnhandledKeyInput(@event));
    }

    /// <summary>
    /// Centraliza la gestión de errores.
    /// </summary>
    /// <param name="message">Mensaje de error</param>
    private void HandleError(string message)
    {
        GD.PrintErr(message);
        // Aquí podrías lanzar una excepción o emitir una señal personalizada
    }

    /// <summary>
    /// Centraliza la impresión de logs de cambio de estado.
    /// </summary>
    /// <param name="message">Mensaje a imprimir</param>
    private void LogStateChange(string message)
    {
        GD.Print(message);
        // Aquí podrías integrar un sistema de logging más avanzado
    }

    /// <summary>
    /// Centraliza la delegación de eventos a los estados.
    /// </summary>
    /// <param name="action">Acción a delegar</param>
    private void DelegateEvent(Action action)
    {
        try
        {
            action?.Invoke();
        }
        catch (Exception ex)
        {
            HandleError($"Error delegando evento: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Obtiene el estado actual activo.
    /// </summary>
    /// <returns>El estado actual como StateBase&lt;T&gt;.</returns>
    public StateBase<T> GetcurrentNodeState()
    {
        return currentNodeState;
    }

    #endregion
}

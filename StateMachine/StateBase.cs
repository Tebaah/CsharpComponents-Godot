using Godot;
using System;

/// <summary>
/// Clase base abstracta y genérica para los estados de una máquina de estados en Godot.
/// 
/// - Define la interfaz y la infraestructura para asociar un nodo controlado de tipo específico (T) y la máquina de estados correspondiente.
/// - Los estados concretos deben heredar de esta clase y especializar el tipo de nodo que controlan.
/// 
/// Métodos virtuales permiten definir el comportamiento al entrar, salir y durante la ejecución del estado.
/// </summary>
public abstract partial class StateBase<T> : Node where T : Node
{
    #region Variables
    /// <summary>
    /// Nodo controlado por este estado. El tipo es específico para cada estado concreto.
    /// </summary>
    public T ControlledNode { get; protected set; }
    /// <summary>
    /// Referencia a la máquina de estados que gestiona este estado.
    /// </summary>
    public StateMachine<T> Machine { get; protected set; }
    #endregion

    #region Godot Methods
    // Métodos de Godot pueden ser sobreescritos por estados concretos si es necesario.
    #endregion

    #region Custom Methods
    /// <summary>
    /// Asigna el nodo controlado y la máquina de estados a este estado.
    /// </summary>
    /// <param name="node">Nodo a controlar</param>
    /// <param name="machine">Máquina de estados</param>
    public virtual void Setup(Node node, StateMachine<T> machine)
    {
        ControlledNode = node as T;
        Machine = machine;

        if (ControlledNode == null)
            GD.PrintErr("ControlledNode is not of expected type " + typeof(T).Name);
    }

    /// <summary>
    /// Llamado al entrar en el estado.
    /// </summary>
    public virtual void AtTheStartOfTheState() { }
    /// <summary>
    /// Llamado al salir del estado.
    /// </summary>
    public virtual void AtTheEndOfTheState() { }
    /// <summary>
    /// Llamado en cada frame para lógica general.
    /// </summary>
    public virtual void OnProcess(double delta) { }
    /// <summary>
    /// Llamado en cada frame de física.
    /// </summary>
    public virtual void OnPhysicsProcess(double delta) { }
    /// <summary>
    /// Llamado al recibir un evento de entrada.
    /// </summary>
    public virtual void OnInput(InputEvent @event) { }
    /// <summary>
    /// Llamado al recibir un evento de entrada no manejado.
    /// </summary>
    public virtual void OnUnhandledInput(InputEvent @event) { }
    /// <summary>
    /// Llamado al recibir un evento de tecla no manejado.
    /// </summary>
    public virtual void OnUnhandledKeyInput(InputEvent @event) { }
    #endregion
}
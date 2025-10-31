# State Machine (Máquina de Estados)

Una implementación genérica y tipada de máquina de estados para Godot en C#.

## 📋 Descripción

Este sistema permite gestionar diferentes estados de comportamiento para cualquier nodo en Godot de manera organizada y escalable. Utiliza genéricos para proporcionar tipado fuerte y seguridad en tiempo de compilación.

## 🏗️ Arquitectura

### Componentes principales:

1. **`StateMachine<T>`**: Controlador principal que gestiona los estados
2. **`StateBase<T>`**: Clase base abstracta para todos los estados

## 🚀 Uso básico

### 1. Crear estados personalizados

```csharp
public partial class IdleState : StateBase<PlayerController>
{
    public override void AtTheStartOfTheState()
    {
        GD.Print("Entrando al estado Idle");
        ControlledNode.Velocity = Vector2.Zero;
    }

    public override void OnProcess(double delta)
    {
        // Lógica del estado idle
        if (Input.IsActionPressed("move"))
        {
            Machine.SwitchToTheNewState("MovingState");
        }
    }
}

public partial class MovingState : StateBase<PlayerController>
{
    public override void OnPhysicsProcess(double delta)
    {
        var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        ControlledNode.Velocity = direction * ControlledNode.Speed;
        ControlledNode.MoveAndSlide();
    }
}
```

### 2. Configurar la máquina de estados

```csharp
public partial class PlayerStateMachine : StateMachine<PlayerController>
{
    // La clase hereda toda la funcionalidad necesaria
    // Solo necesitas configurar los nodos en el editor
}
```

### 3. Estructura en el editor de Godot

```
Player (PlayerController)
└── StateMachine (PlayerStateMachine)
    ├── IdleState
    ├── MovingState
    └── JumpingState
```

### 4. Configuración en el Inspector

- **Node To Control**: Asigna el nodo Player
- **Default State**: Asigna IdleState

## 🔧 Métodos disponibles

### En StateBase<T>

- `AtTheStartOfTheState()`: Se ejecuta al entrar al estado
- `AtTheEndOfTheState()`: Se ejecuta al salir del estado
- `OnProcess(double delta)`: Se ejecuta cada frame
- `OnPhysicsProcess(double delta)`: Se ejecuta cada frame de física
- `OnInput(InputEvent event)`: Maneja eventos de entrada
- `OnUnhandledInput(InputEvent event)`: Maneja entrada no procesada

### En StateMachine<T>

- `SwitchToTheNewState(string stateName)`: Cambia a un nuevo estado
- `GetCurrentState()`: Obtiene el estado actual

## 🎯 Ventajas

- ✅ **Tipado fuerte**: Evita errores en tiempo de ejecución
- ✅ **Reutilizable**: Funciona con cualquier tipo de nodo
- ✅ **Organizado**: Cada estado es una clase independiente
- ✅ **Escalable**: Fácil agregar nuevos estados
- ✅ **Integrado**: Compatible con el sistema de nodos de Godot

## 📝 Ejemplo completo

```csharp
// En tu estado personalizado
public partial class AttackState : StateBase<Enemy>
{
    private float attackDuration = 2.0f;
    private float currentTime = 0.0f;

    public override void AtTheStartOfTheState()
    {
        ControlledNode.PlayAnimation("attack");
        currentTime = 0.0f;
    }

    public override void OnProcess(double delta)
    {
        currentTime += (float)delta;
        
        if (currentTime >= attackDuration)
        {
            Machine.SwitchToTheNewState("IdleState");
        }
    }

    public override void AtTheEndOfTheState()
    {
        ControlledNode.StopAnimation();
    }
}
```

## 🏷️ Características técnicas

- Compatible con Godot 4.x
- Utiliza genéricos de C# para tipado fuerte
- Manejo centralizado de errores y logs
- Delegación automática de eventos de Godot
- Configuración visual desde el editor

---

*Desarrollado para facilitar la implementación de máquinas de estados en proyectos de Godot con C#.*

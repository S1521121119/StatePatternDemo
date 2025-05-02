```mermaid
stateDiagram-v2
    Null  --> Idle  : Create     / 
    Idle  --> Ready : Connect    / ActionConnectProcess
    Idle  --> Null  : Destroy    / 
    Ready --> Idle  : Disconnect / ActionDisconnectProcess
```
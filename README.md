# StatePatternDemo

### 設計流程與步驟

#### 0. 範例說明
本文件以 `Demo` 作為範例，用以展示狀態機的設計與實作過程。

#### 1. 定義核心概念與需求
在開始編寫程式碼之前，需先明確系統的核心需求與設計基礎：
- **原因**：清晰的需求定義是後續設計與實作的基石，能有效避免開發過程中因目標不明確而導致的反覆修改。
- **步驟**：
  1. 確定系統所需的**狀態（State）**，例如 `Null`（空狀態）、`Idle`（待機狀態）、`Ready`（就緒狀態）等。
  2. 識別觸發狀態轉換的**事件（Event）**，如 `Create`（建立）、`Connect`（連線）、`Disconnect`（斷線）、`Destroy`（銷毀）。
  3. 定義每個狀態或狀態轉換所需的**動作（Action）**，例如初始化操作或發送請求。
  4. 明確指定負責管理狀態與協調動作的**上下文（Context）**角色。
  5. 使用紙筆或文件記錄上述需求，並建立初始的資料夾與檔案結構：
```
├─DemoModule
│  │  Demo_Context.cs
│  │  Demo_Driven.cs
│  └─Interfaces
│          IDemo_Context.cs
│          IDemo_Driven.cs
│          IDemo_Event.cs
│          IDemo_State.cs
```

下方是狀態機的狀態遷移表格。
```
| CurrentState | StateEvent | NextState | Action                  |
|--------------|------------|-----------|-------------------------|
| Null         | Create     | Idle      | -                       |
| Idle         | Connect    | Ready     | ActionConnectProcess    |
| Idle         | Destroy    | Null      | -                       |
| Ready        | Disconnect | Idle      | ActionDisconnectProcess |
```
---

#### 2. 設計狀態介面 `IDemo_State`
- **原因**：狀態是狀態機的核心組成部分，定義其行為規範是實作的第一步，為後續邏輯提供基礎。
- **步驟**：
  1. 定義狀態的基本行為，例如處理事件的核心方法。
  2. 評估是否需要加入進入（`Entry`）與離開（`Exit`）狀態的動作。
- **範例**：
  "./DemoModule/Interfaces/IDemo_State.cs"
  ```csharp
  public interface IDemo_State
  {
        void HandleEvent(IDemo_Event stateEvent);
  }
  // 後續將根據需求追加內容
  ```
- **備註**：採用介面設計以確保未來的可擴展性與維護性。

---

#### 3. 設計上下文介面 `IDemo_Context`
- **原因**：上下文負責管理狀態並提供外部操作的介面，需在狀態介面定義完成後設計，因其依賴於 `IDemo_State`。
- **步驟**：
  1. 定義狀態切換的方法，例如 `SetState(IDemo_State)`。
  2. 引入執行介面 `IDemo_Driven`，以支援上下文調用具體動作。
  3. 考慮是否需支援狀態堆疊功能（如 `Become(IDemo_State)` 或 `Restore()`）。
- **範例**：
  "./DemoModule/Interfaces/IDemo_Context.cs"
  ```csharp
  public interface IDemo_Context
  {
      IDemo_Driven Driven { get; }
      void SetState(IDemo_State state);
      void HandleEvent(IDemo_Event stateEvent);
      // 後續將根據需求追加內容
  }
  ```
- **備註**：`IDemo_Context` 作為狀態的持有者，需與 `IDemo_State` 協同運作。

---

#### 4. 設計動作介面 `IDemo_Driven`
- **原因**：動作介面負責定義狀態機的執行層行為，需在狀態與上下文介面確立後設計，因其依賴前者的規範。
- **步驟**：
  1. 根據需求列舉所有可能的動作。
  2. 為每個動作定義對應的抽象方法（如 `ActionConnectProcess`）。
- **範例**：
  "./DemoModule/Interfaces/IDemo_Driven.cs"
  ```csharp
  public interface IDemo_Driven
  {
      void ActionConnectProcess();
      void ActionDisconnectProcess();
  }
  ```
- **備註**：`IDemo_Driven` 提供動作的抽象規範，具體實作由 `Demo_Driven` 類別完成。

---

#### 5. 實作事件介面 `IDemo_Event`
- **原因**：事件是觸發狀態轉換的關鍵，需實作出具體類別以供 `IDemo_Context` 接收與處理。
- **步驟**：
  1. 分析系統所需的所有事件類型。
  2. 為每個事件定義對應的類別，實現 `IDemo_Event` 介面。
- **範例**：
  "./DemoModule/Interfaces/IDemo_Event.cs"
  ```csharp
  public interface IDemo_Event { }
  public class Demo_Event_Create : IDemo_Event { }
  public class Demo_Event_Connect : IDemo_Event { }
  public class Demo_Event_Destroy : IDemo_Event { }
  public class Demo_Event_Disconnect : IDemo_Event { }
  ```

---

#### 6. 實作狀態類別 `Demo_State_XXX`
- **原因**：基於 `IDemo_State` 介面，可進一步實作具體的狀態邏輯，並透過抽象基類統一行為規範。
- **步驟**：
  1. 建立抽象基類 `Base_Demo_State`，實現 `IDemo_State` 介面。
  2. 為每個具體狀態創建類別，繼承 `Base_Demo_State`。
  3. 在各狀態中根據事件類型實現具體行為邏輯。
- **範例**：
  "./DemoModule/Interfaces/IDemo_State.cs"
  ```csharp
  public interface IDemo_State
  {
      void HandleEvent(IDemo_Event stateEvent);
  }

  public abstract class Base_Demo_State : IDemo_State
  {
      protected IDemo_Context Context { get; }
      public Base_Demo_State(IDemo_Context context) => Context = context;
      public virtual void HandleEvent(IDemo_Event stateEvent) => throw new NotImplementedException($"HandleEvent was not implemented in '{GetType().Name}'.");
  }

  public class Demo_State_Null : Base_Demo_State
  {
      public Demo_State_Null(IDemo_Context context) : base(context) { }

      public override void HandleEvent(IDemo_Event stateEvent)
      {
          switch (stateEvent)
          {
              case Demo_Event_Create _:
                  Context.SetState(Context.Idle_State);
                  break;
              default:
                  throw new NotImplementedException($"Event '{stateEvent}' was not handled in '{GetType().Name}'.");
          }
      }
  }

  public class Demo_State_Idle : Base_Demo_State
  {
      public Demo_State_Idle(IDemo_Context context) : base(context) { }

      public override void HandleEvent(IDemo_Event stateEvent)
      {
          switch (stateEvent)
          {
              case Demo_Event_Connect _:
                  Context.SetState(Context.Ready_State);
                  Context.Driven.ActionConnectProcess();
                  break;
              case Demo_Event_Destroy _:
                  Context.SetState(Context.Null_State);
                  break;
              default:
                  throw new NotImplementedException($"Event '{stateEvent}' was not handled in '{GetType().Name}'.");
          }
      }
  }

  public class Demo_State_Ready : Base_Demo_State
  {
      public Demo_State_Ready(IDemo_Context context) : base(context) { }

      public override void HandleEvent(IDemo_Event stateEvent)
      {
          switch (stateEvent)
          {
              case Demo_Event_Disconnect _:
                  Context.SetState(Context.Idle_State);
                  Context.Driven.ActionDisconnectProcess();
                  break;
              default:
                  throw new NotImplementedException($"Event '{stateEvent}' was not handled in '{GetType().Name}'.");
          }
      }
  }
  ```
- **備註**：每個狀態需持有 `IDemo_Context` 實例，以便進行狀態切換。

---

#### 7. 更新 `IDemo_Context` 介面以加入狀態屬性
- **原因**：先前已定義 `IDemo_Context`，現需將實作完成的狀態類別整合進介面，以支援狀態管理。
- **步驟**：
  1. 收集所有實現 `IDemo_State` 的狀態類別。
  2. 以唯讀屬性形式將狀態加入 `IDemo_Context`。
- **範例**：
  "./DemoModule/Interfaces/IDemo_Context.cs"
  ```csharp
  public interface IDemo_Context
  {
      // 新增屬性
      IDemo_State CurrentState { get; }
      IDemo_State Null_State { get; }
      IDemo_State Idle_State { get; }
      IDemo_State Ready_State { get; }

      // 既有內容
      IDemo_Driven Driven { get; }
      void SetState(IDemo_State state);
      void HandleEvent(IDemo_Event stateEvent);
  }
  ```

---

#### 8. 實作上下文類別 `Demo_Context`
- **原因**：`Demo_Context` 是狀態機的管理核心，需在狀態與介面設計完成後實作，依賴 `IDemo_State`、`IDemo_Context` 與 `IDemo_Driven`。
- **步驟**：
  1. 實現 `IDemo_Context`，負責管理當前狀態。
  2. 在建構函數中初始化所有狀態實例。
  3. 透過 `CurrentState.HandleEvent` 處理事件。
- **範例**：
  "./DemoModule/Demo_Context.cs"
  ```csharp
  public class Demo_Context : IDemo_Context
  {
      public IDemo_State CurrentState { get; private set; }
      public IDemo_State Null_State { get; }
      public IDemo_State Idle_State { get; }
      public IDemo_State Ready_State { get; }

      public IDemo_Driven Driven { get; }

      public Demo_Context(IDemo_Driven driven)
      {
          Driven = driven;

          Null_State = new Demo_State_Null(this);
          Idle_State = new Demo_State_Idle(this);
          Ready_State = new Demo_State_Ready(this);

          SetState(Null_State); // The initial state is set to 'Null'
      }

      public void SetState(IDemo_State state)
      {
          if (CurrentState == state) return;
          CurrentState = state;
      }

      public void HandleEvent(IDemo_Event stateEvent) => CurrentState.HandleEvent(stateEvent);
  }
  ```
- **備註**：此為基礎實作，後續可根據需求加入狀態堆疊或事件通知功能。

---

#### 9. 實作動作類別 `Demo_Driven`
- **原因**：`Demo_Driven` 是動作的具體執行者，需在 `IDemo_Driven` 與上下文設計完成後實作。
- **步驟**：
  1. 實現 `IDemo_Driven` 中定義的所有動作方法。
  2. 根據需求添加具體的執行邏輯，例如連線或斷線操作。
- **範例**：
  "./DemoModule/Demo_Driven.cs"
  ```csharp
  public class Demo_Driven : IDemo_Driven
  {
      public void ActionConnectProcess() => Console.WriteLine("Execute ConnectProcess Process...");
      public void ActionDisconnectProcess() => Console.WriteLine("Execute DisconnectProcess Process...");}
  }
  ```
- **備註**：範例中以簡單輸出作為示意，實際應用中可涉及硬體或網路操作。

---

### 設計流程總結
1. **定義需求**：明確狀態、事件與動作。
2. **設計 `IDemo_State`**：規範狀態行為。
3. **設計 `IDemo_Context`**：建立上下文管理介面。
4. **設計 `IDemo_Driven`**：定義動作介面。
5. **實作 `IDemo_Event`**：創建事件類別。
6. **實作 `Demo_State_XXX`**：實現抽象基類與具體狀態。
7. **更新 `IDemo_Context`**：整合狀態屬性。
8. **實作 `Demo_Context`**：完成狀態與事件管理。
9. **實作 `Demo_Driven`**：實現具體動作。

---

### 執行流程範例
以下展示系統從初始化到連線再斷線的流程：
1. 創建 `Demo_Driven` 實例。
2. 以 `Demo_Driven` 初始化 `Demo_Context`，預設狀態為 `Null`。
3. 依序觸發事件：`Create`、`Connect`、`Disconnect`、`Destroy`。

程式碼：
```csharp
  static void Main(string[] args)
  {
      IDemo_Driven demoDriven = new Demo_Driven();
      IDemo_Context demoContext = new Demo_Context(demoDriven);
      demoContext.HandleEvent(new Demo_Event_Create());
      demoContext.HandleEvent(new Demo_Event_Connect());
      demoContext.HandleEvent(new Demo_Event_Disconnect());
      demoContext.HandleEvent(new Demo_Event_Destroy());
  }
```

輸出：
```
Execute ConnectProcess Process...
Execute DisconnectProcess Process...
```

---

### 注意事項
- **依賴關係**：`Demo_State_XXX` 依賴 `IDemo_Context`，而 `Demo_Context` 依賴 `IDemo_State` 與 `IDemo_Driven`，因此需優先設計介面。
- **擴展性**：初期可採用簡化實作，後續根據需求新增事件、狀態類型，或加入事件通知與多執行緒支援。
- **測試驗證**：每完成一個階段（介面或實作），應進行測試以確保邏輯正確性。


---

#### 10. 擴充 `IDemo_State` 介面以支援 `Enter` 和 `Exit` 方法
- **原因**：為了增強狀態機的靈活性和可追溯性，新增 `Enter` 和 `Exit` 方法能明確標記狀態的進入與離開時機，方便執行初始化、清理或其他副作用操作（如日誌記錄或資源管理）。
- **步驟**：
  1. 在 `IDemo_State` 介面中新增 `Enter` 和 `Exit` 方法的抽象定義。
  2. 更新現有狀態類別以實現這些方法。
- **範例**：
  "./DemoModule/Interfaces/IDemo_State.cs"
  ```csharp
  public interface IDemo_State
  {
      void HandleEvent(IDemo_Event stateEvent);
      void Enter(); // 新增方法，定義狀態進入行為
      void Exit(); // 新增方法，定義狀態離開行為
  }
  ```

---

#### 11. 更新 `Base_Demo_State` 提供預設的 `Enter` 和 `Exit` 實作
- **原因**：透過在抽象基類中提供預設的 `Enter` 和 `Exit` 實作，可減少子類的重複程式碼，並確保一致的行為模式（如日誌輸出）。具體狀態類別可根據需求覆寫這些方法。
- **步驟**：
  1. 在 `Base_Demo_State` 中實現 `Enter` 和 `Exit` 方法，輸出當前狀態名稱作為日誌。
  2. 確保子類（如 `Demo_State_Null`、`Demo_State_Idle`、`Demo_State_Ready`）繼承預設行為，或視需求進行覆寫。
- **範例**：
  "./DemoModule/Interfaces/IDemo_State.cs"
  ```csharp
  public abstract class Base_Demo_State : IDemo_State
  {
      protected IDemo_Context Context { get; }
      public Base_Demo_State(IDemo_Context context) => Context = context;
      public virtual void HandleEvent(IDemo_Event stateEvent) => throw new NotImplementedException($"HandleEvent was not implemented in '{GetType().Name}'.");
      public virtual void Enter() => Console.WriteLine($"Enter '{GetType().Name}' State.");
      public virtual void Exit() => Console.WriteLine($"Exit '{GetType().Name}' State.");
  }
  ```
- **備註**：子類可選擇保留預設的日誌輸出，或覆寫以實現特定邏輯（如初始化資源或釋放連線）。

---

#### 12. 修改 `Demo_Context` 的 `SetState` 方法以支援 `Enter` 和 `Exit`
- **原因**：狀態切換是狀態機的核心操作，需在切換時正確觸發當前狀態的 `Exit` 和新狀態的 `Enter`，以確保狀態轉換的完整性和副作用的執行。
- **步驟**：
  1. 修改 `SetState` 方法，在設置新狀態前調用當前狀態的 `Exit` 方法。
  2. 在設置新狀態後立即調用新狀態的 `Enter` 方法。
  3. 使用可空引用檢查（`?.`）以避免初始狀態為 `null` 時的異常。
- **範例**：
  "./DemoModule/Demo_Context.cs"
  ```csharp
  public class Demo_Context : IDemo_Context
  {
      public IDemo_State CurrentState { get; private set; }
      public IDemo_Driven Driven { get; }
      public IDemo_State Null_State { get; }
      public IDemo_State Idle_State { get; }
      public IDemo_State Ready_State { get; }

      public Demo_Context(IDemo_Driven driven)
      {
          Driven = driven;
          Null_State = new Demo_State_Null(this);
          Idle_State = new Demo_State_Idle(this);
          Ready_State = new Demo_State_Ready(this);

          SetState(Null_State); // 初始狀態設為 Null
      }

      public void SetState(IDemo_State state)
      {
          if (CurrentState == state) return;
          CurrentState?.Exit();
          CurrentState = state;
          CurrentState?.Enter();
      }

      public void HandleEvent(IDemo_Event stateEvent) => CurrentState.HandleEvent(stateEvent);
  }
  ```
- **備註**：此修改確保每次狀態切換都會正確執行離開和進入動作，提升狀態機的透明度與可維護性。

---

### 擴充後的執行流程範例
以下展示系統在擴充 `Enter` 和 `Exit` 方法後，從初始化到連線再斷線的流程：
1. 創建 `Demo_Driven` 實例。
2. 以 `Demo_Driven` 初始化 `Demo_Context`，預設狀態為 `Null`。
3. 依序觸發事件：`Create`、`Connect`、`Disconnect`、`Destroy`。

程式碼：
```csharp
static void Main(string[] args)
{
    IDemo_Driven Demo_Driven = new Demo_Driven();
    IDemo_Context Demo_Context = new Demo_Context(Demo_Driven);
    Demo_Context.HandleEvent(new Demo_Event_Create());
    Demo_Context.HandleEvent(new Demo_Event_Connect());
    Demo_Context.HandleEvent(new Demo_Event_Disconnect());
    Demo_Context.HandleEvent(new Demo_Event_Destroy());
}
```

輸出：
```
Enter 'Demo_State_Null' State.
Exit 'Demo_State_Null' State.
Enter 'Demo_State_Idle' State.
Exit 'Demo_State_Idle' State.
Enter 'Demo_State_Ready' State.
Execute ConnectProcess Process...
Exit 'Demo_State_Ready' State.
Enter 'Demo_State_Idle' State.
Execute DisconnectProcess Process...
Exit 'Demo_State_Idle' State.
Enter 'Demo_State_Null' State.
```

---

### 擴充設計總結
1. **擴充 `IDemo_State`**：新增 `Enter` 和 `Exit` 方法，規範狀態進入與離開行為。
2. **更新 `Base_Demo_State`**：提供預設的 `Enter` 和 `Exit` 實作，輸出狀態日誌。
3. **修改 `Demo_Context`**：調整 `SetState` 方法，確保狀態切換時正確觸發 `Exit` 和 `Enter`。

---

### 額外注意事項
- **日誌用途**：預設的 `Enter` 和 `Exit` 日誌有利於除錯，實際應用中可替換為具體操作（如資源分配或釋放）。
- **效能考量**：若狀態切換頻繁，應評估 `Enter` 和 `Exit` 的執行成本，避免引入不必要的開銷。
- **覆寫靈活性**：具體狀態類別（如 `Demo_State_Ready`）可覆寫 `Enter` 或 `Exit`，以實現進階邏輯，如連線初始化或斷線清理。
- **測試擴充**：新增 `Enter` 和 `Exit` 後，應針對狀態切換行為進行單元測試，驗證每個狀態的進入與離開邏輯是否符合預期。
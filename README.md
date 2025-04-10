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
│  │  DemoContext.cs
│  │  DemoDriven.cs
│  └─Interfaces
│          IDemoContext.cs
│          IDemoDriven.cs
│          IDemoEvent.cs
│          IDemoState.cs
```

下方是狀態機的狀態遷移表格。
```
| CurrentState | StateEvent           | NextState | Action                  |
|--------------|----------------------|-----------|-------------------------|
| Null         | DemoEvent_Create     | Idle      | -                       |
| Idle         | DemoEvent_Connect    | Ready     | ActionConnectProcess    |
| Idle         | DemoEvent_Destroy    | Null      | -                       |
| Ready        | DemoEvent_Disconnect | Idle      | ActionDisconnectProcess |
```
---

#### 2. 設計狀態介面 `IDemoState`
- **原因**：狀態是狀態機的核心組成部分，定義其行為規範是實作的第一步，為後續邏輯提供基礎。
- **步驟**：
  1. 定義狀態的基本行為，例如處理事件的核心方法。
  2. 評估是否需要加入進入（`Entry`）與離開（`Exit`）狀態的動作。
- **範例**：
  "./DemoModule/Interfaces/IDemoState.cs"
  ```csharp
  public interface IDemoState
  {
      void HandleEvent(IDemoEvent stateEvent); // 抽象方法，用於處理事件
  }
  // 後續將根據需求追加內容
  ```
- **備註**：採用介面設計以確保未來的可擴展性與維護性。

---

#### 3. 設計上下文介面 `IDemoContext`
- **原因**：上下文負責管理狀態並提供外部操作的介面，需在狀態介面定義完成後設計，因其依賴於 `IDemoState`。
- **步驟**：
  1. 定義狀態切換的方法，例如 `SetState(IDemoState)`。
  2. 引入執行介面 `IDemoDriven`，以支援上下文調用具體動作。
  3. 考慮是否需支援狀態堆疊或事件通知功能（如 `Become(IDemoState)` 或 `Restore()`）。
- **範例**：
  "./DemoModule/Interfaces/IDemoContext.cs"
  ```csharp
  public interface IDemoContext
  {
      IDemoDriven Driven { get; }
      void SetState(IDemoState state);
      void HandleEvent(IDemoEvent stateEvent);
      // 後續將根據需求追加內容
  }
  ```
- **備註**：`IDemoContext` 作為狀態的持有者，需與 `IDemoState` 協同運作。

---

#### 4. 設計動作介面 `IDemoDriven`
- **原因**：動作介面負責定義狀態機的執行層行為，需在狀態與上下文介面確立後設計，因其依賴前者的規範。
- **步驟**：
  1. 根據需求列舉所有可能的動作。
  2. 為每個動作定義對應的抽象方法（如 `ActionConnectProcess`）。
- **範例**：
  "./DemoModule/Interfaces/IDemoDriven.cs"
  ```csharp
  public interface IDemoDriven
  {
      void ActionConnectProcess();
      void ActionDisconnectProcess();
  }
  ```
- **備註**：`IDemoDriven` 提供動作的抽象規範，具體實作由 `DemoDriven` 類別完成。

---

#### 5. 實作事件介面 `IDemoEvent`
- **原因**：事件是觸發狀態轉換的關鍵，需實作出具體類別以供 `IDemoContext` 接收與處理。
- **步驟**：
  1. 分析系統所需的所有事件類型。
  2. 為每個事件定義對應的類別，實現 `IDemoEvent` 介面。
- **範例**：
  "./DemoModule/Interfaces/IDemoEvent.cs"
  ```csharp
  public interface IDemoEvent { }
  public class DemoEvent_Create : IDemoEvent { }
  public class DemoEvent_Destroy : IDemoEvent { }
  public class DemoEvent_Connect : IDemoEvent { }
  public class DemoEvent_Disconnect : IDemoEvent { }
  ```

---

#### 6. 實作狀態類別 `DemoState`
- **原因**：基於 `IDemoState` 介面，可進一步實作具體的狀態邏輯，並透過抽象基類統一行為規範。
- **步驟**：
  1. 建立抽象基類 `BaseDemoState`，實現 `IDemoState` 介面。
  2. 為每個具體狀態創建類別，繼承 `BaseDemoState`。
  3. 在各狀態中根據事件類型實現具體行為邏輯。
- **範例**：
  "./DemoModule/Interfaces/IDemoState.cs"
  ```csharp
  public interface IDemoState
  {
      void HandleEvent(IDemoEvent stateEvent);
  }

  public abstract class BaseDemoState : IDemoState
  {
      protected IDemoContext Context { get; }

      public BaseDemoState(IDemoContext context) => Context = context;

      public virtual void HandleEvent(IDemoEvent stateEvent)
      {
          throw new NotImplementedException($"事件 {stateEvent} 未實作。");
      }
  }

  public class DemoNullState : BaseDemoState
  {
      public DemoNullState(IDemoContext context) : base(context) { }

      public override void HandleEvent(IDemoEvent stateEvent)
      {
          switch (stateEvent)
          {
              case DemoEvent_Create _:
                  Context.SetState(Context.IdleState);
                  break;
              default:
                  throw new NotImplementedException($"事件 {stateEvent} 在 {GetType().Name} 中未處理。");
          }
      }
  }

  public class DemoIdleState : BaseDemoState
  {
      public DemoIdleState(IDemoContext context) : base(context) { }

      public override void HandleEvent(IDemoEvent stateEvent)
      {
          switch (stateEvent)
          {
              case DemoEvent_Connect _:
                  Context.SetState(Context.ReadyState);
                  Context.Driven.ActionConnectProcess();
                  break;
              case DemoEvent_Destroy _:
                  Context.SetState(Context.NullState);
                  break;
              default:
                  throw new NotImplementedException($"事件 {stateEvent} 在 {GetType().Name} 中未處理。");
          }
      }
  }

  public class DemoReadyState : BaseDemoState
  {
      public DemoReadyState(IDemoContext context) : base(context) { }

      public override void HandleEvent(IDemoEvent stateEvent)
      {
          switch (stateEvent)
          {
              case DemoEvent_Disconnect _:
                  Context.SetState(Context.IdleState);
                  Context.Driven.ActionDisconnectProcess();
                  break;
              default:
                  throw new NotImplementedException($"事件 {stateEvent} 在 {GetType().Name} 中未處理。");
          }
      }
  }
  ```
- **備註**：每個狀態需持有 `IDemoContext` 實例，以便進行狀態切換。

---

#### 7. 更新 `IDemoContext` 介面以加入狀態屬性
- **原因**：先前已定義 `IDemoContext`，現需將實作完成的狀態類別整合進介面，以支援狀態管理。
- **步驟**：
  1. 收集所有實現 `IDemoState` 的狀態類別。
  2. 以唯讀屬性形式將狀態加入 `IDemoContext`。
- **範例**：
  "./DemoModule/Interfaces/IDemoContext.cs"
  ```csharp
  public interface IDemoContext
  {
      // 新增屬性
      IDemoState CurrentState { get; }
      IDemoState NullState { get; }
      IDemoState IdleState { get; }
      IDemoState ReadyState { get; }

      // 既有內容
      IDemoDriven Driven { get; }
      void SetState(IDemoState state);
      void HandleEvent(IDemoEvent stateEvent);
  }
  ```

---

#### 8. 實作上下文類別 `DemoContext`
- **原因**：`DemoContext` 是狀態機的管理核心，需在狀態與介面設計完成後實作，依賴 `IDemoState`、`IDemoContext` 與 `IDemoDriven`。
- **步驟**：
  1. 實現 `IDemoContext`，負責管理當前狀態。
  2. 在建構函數中初始化所有狀態實例。
  3. 透過 `CurrentState.HandleEvent` 處理事件。
- **範例**：
  "./DemoModule/DemoContext.cs"
  ```csharp
  public class DemoContext : IDemoContext
  {
      public IDemoState CurrentState { get; private set; }
      public IDemoDriven Driven { get; }
      public IDemoState NullState { get; }
      public IDemoState IdleState { get; }
      public IDemoState ReadyState { get; }

      public DemoContext(IDemoDriven driven)
      {
          Driven = driven;

          NullState = new DemoNullState(this);
          IdleState = new DemoIdleState(this);
          ReadyState = new DemoReadyState(this);

          SetState(NullState); // 初始狀態設為 Null
      }

      public void SetState(IDemoState state)
      {
          CurrentState = state;
      }

      public void HandleEvent(IDemoEvent stateEvent) => CurrentState.HandleEvent(stateEvent);
  }
  ```
- **備註**：此為基礎實作，後續可根據需求加入狀態堆疊或事件通知功能。

---

#### 9. 實作動作類別 `DemoDriven`
- **原因**：`DemoDriven` 是動作的具體執行者，需在 `IDemoDriven` 與上下文設計完成後實作。
- **步驟**：
  1. 實現 `IDemoDriven` 中定義的所有動作方法。
  2. 根據需求添加具體的執行邏輯，例如連線或斷線操作。
- **範例**：
  "./DemoModule/DemoDriven.cs"
  ```csharp
  public class DemoDriven : IDemoDriven
  {
      public void ActionConnectProcess()
      {
          Console.WriteLine("執行連線操作...");
      }

      public void ActionDisconnectProcess()
      {
          Console.WriteLine("執行斷線操作...");
      }
  }
  ```
- **備註**：範例中以簡單輸出作為示意，實際應用中可涉及硬體或網路操作。

---

### 設計流程總結
1. **定義需求**：明確狀態、事件與動作。
2. **設計 `IDemoState`**：規範狀態行為。
3. **設計 `IDemoContext`**：建立上下文管理介面。
4. **設計 `IDemoDriven`**：定義動作介面。
5. **實作 `IDemoEvent`**：創建事件類別。
6. **實作 `DemoState`**：實現抽象基類與具體狀態。
7. **更新 `IDemoContext`**：整合狀態屬性。
8. **實作 `DemoContext`**：完成狀態與事件管理。
9. **實作 `DemoDriven`**：實現具體動作。

---

### 執行流程範例
以下展示系統從初始化到連線再斷線的流程：
1. 創建 `DemoDriven` 實例。
2. 以 `DemoDriven` 初始化 `DemoContext`，預設狀態為 `Null`。
3. 依序觸發事件：`Create`、`Connect`、`Disconnect`、`Destroy`。

程式碼：
```csharp
static void Main(string[] args)
{
    IDemoDriven demoDriven = new DemoDriven();
    IDemoContext demoContext = new DemoContext(demoDriven);
    demoContext.HandleEvent(new DemoEvent_Create());
    demoContext.HandleEvent(new DemoEvent_Connect());
    demoContext.HandleEvent(new DemoEvent_Disconnect());
    demoContext.HandleEvent(new DemoEvent_Destroy());
}
```

輸出：
```
執行連線操作...
執行斷線操作...
```

---

### 注意事項
- **依賴關係**：`DemoState` 依賴 `IDemoContext`，而 `DemoContext` 依賴 `IDemoState` 與 `IDemoDriven`，因此需優先設計介面。
- **擴展性**：初期可採用簡化實作，後續根據需求新增事件、狀態類型，或加入事件通知與多執行緒支援。
- **測試驗證**：每完成一個階段（介面或實作），應進行測試以確保邏輯正確性。

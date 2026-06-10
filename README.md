# Shop-system_csmumi

CsmuMi - A shop and inventory management system implemented in [C#]

---

## 1. 命名規範 (Naming Conventions)
為了避免混淆與保持專業度，專案內所有檔案與命名禁止使用中文或空白。

* **專案根目錄/倉庫名稱**：`Shop-system_csmumi`
* **分支 (Branch) 命名**：
  * 主分支（不輕動）：`main`
  * 功能開發分支：`feat-功能名稱`（例如：`feat-login`）
  * Bug 修復分支：`fix-問題名稱`（例如：`fix-db-timeout`）
* **期末打包/報告繳交命名公式**：
  * `學號_姓名_期末專題名稱_版本號`
  * *範例：`14580xx_姓名_ShopSystem_v1.0.0.zip`*

---

## 2. 版本號疊代規範 (Semantic Versioning)
本專案採用 **語意化版本 (SemVer)** 規範，版本號格式為：`主版本號.次版本號.修訂號` ($X.Y.Z$)。

$$Version = X.Y.Z$$

* **X (主版本號 - Major)**：當結構有重大調整、大規模重構，或檔案架構不相容舊版時變更。
* **Y (次版本號 - Minor)**：當新增獨立功能（如：新增購物車模組、新增資料庫連線），且不影響既有功能時變更。
* **Z (修訂號 - Patch)**：當沒有新增功能，純粹修復 Bug、優化效能或修改介面錯字時變更。

> 💡 **專題階段提示**：開發期間統一使用 `v0.x.x`；當所有功能完備、準備上台報告或交付最終成果時，正式定版本為 `v1.0.0`。

---

## 3. 提交紀錄與多使用者更新備註 (Commit & Changelog)
所有成員在更新程式碼（或使用 Git 提交 Commit）時，必須遵守 **約定式提交 (Conventional Commits)** 格式，並在結尾備註更新者。

### 格式模板
```text
<類型>(影響範圍): 簡短說明 [更新者姓名]
### 常用類型 (Type)
＊　＊＊feat: 新增功能 (Feature)

＊　＊＊fix: 修補 Bug (Bug Fix)

＊　＊＊docs: 僅修改文件、註解、報告 (Documentation)

＊　＊＊style: 修改程式碼格式（不影響邏輯，如排版、空格）

＊　＊＊refactor: 重構程式碼（既非功能也非 Bug 的結構優化）

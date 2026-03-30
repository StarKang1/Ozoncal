# 🚀 OZON 利润计算器 - 快速开始指南

## 一分钟快速开始

### 1️⃣ 系统要求
- Windows 10 或更新版本
- .NET 8.0 Runtime 或 SDK

### 2️⃣ 安装 .NET 8.0
```powershell
# Windows 开启 PowerShell，运行以下命令下载安装
winget install Microsoft.DotNet.SDK.8
```

或访问：https://dotnet.microsoft.com/download

### 3️⃣ 构建项目

#### 方式 A：命令行构建
```powershell
cd f:\TraeProjects\Ozoncal
dotnet restore
dotnet build
dotnet run
```

#### 方式 B：Visual Studio 构建
1. 打开 Visual Studio
2. 文件 → 打开文件夹 → 选择 `f:\TraeProjects\Ozoncal`
3. 通过 VS 菜单构建和运行

#### 方式 C：VS Code 构建
1. 打开 VS Code
2. 打开文件夹 `f:\TraeProjects\Ozoncal`
3. 终端 → 新终端
4. 输入：`dotnet run`

### 4️⃣ 发布为单文件 EXE（推荐）
```powershell
cd f:\TraeProjects\Ozoncal

# 生成单文件 EXE
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:SelfContained=true -p:IncludeNativeLibrariesForSelfExtract=true

# 输出文件：bin\Release\net8.0-windows\win-x64\publish\OzonCalc.exe
```

生成的 `OzonCalc.exe` 可以直接分发给用户，无需安装 .NET。

## 📋 使用说明

### 基本使用流程

1. **输入成本** - 输入产品出厂价（如 20 元）
2. **输入重量** - 输入产品克重（如 200g）
3. **查看费率** - 检查或修改：
   - 佣金（默认 20%）
   - 广告费（默认 10%）
   - 货损率（默认 20%）
   - 利润（默认 30%）
4. **点击计算** - 获得建议售价
5. **复制售价** - 一键复制到剪贴板

### 自定义物流公式

默认公式：`3 + 0.045 * weight`

可修改为：
- `5 + 0.05 * weight` （运费更高）
- `10` （固定运费）
- `0.03 * weight` （轻量物品）

### 参数自动保存

所有参数自动保存到：`%APPDATA%\OzonCalc\config.json`

下次打开应用时自动加载。

## 🎯 常见场景

### 场景 1：成本¥20，重量200g，求售价
1. 成本 → 20
2. 重量 → 200
3. 点击"重新计算"
4. 结果：建议售价 ¥59（整数价）
5. 点击"复制售价"

### 场景 2：利润不够，需要提价
1. 修改 **利润率** 为 35%（从 30%）
2. 自动重算
3. 查看新的售价建议

### 场景 3：运费很高的产品
1. 修改物流公式：`5 + 0.08 * weight`
2. 自动重算
3. 查看新的物流成本和售价

## 🔧 配置文件位置

Windows：`%APPDATA%\OzonCalc\config.json`

完整路径示例：
```
C:\Users\YourUsername\AppData\Roaming\OzonCalc\config.json
```

配置内容示例：
```json
{
  "commission": 0.2,
  "ads": 0.1,
  "damage": 0.2,
  "profit": 0.3,
  "ship_formula": "3 + 0.045 * weight"
}
```

## 🐛 常见问题

### Q1：找不到 .NET 8.0
**解决**：运行 `dotnet --version` 检查版本，或从官网下载安装。

### Q2：运行时出错
**解决**：
1. 检查 .csproj 文件配置是否正确
2. 运行 `dotnet restore` 恢复依赖
3. 删除 `bin/obj` 文件夹，重新构建

### Q3：发布的 EXE 很大
**解决**：这是正常的（包含 .NET Runtime），可以压缩分发。

### Q4：需要修改应用标题或图标
**解决**：编辑 `OzonPriceCalculator.csproj` 中的 `<Title>` 和 `<ApplicationIcon>` 字段。

## 📱 项目扩展方向

1. **加入数据库** - 保存历史报价记录
2. **API 集成** - 自动获取 OZON 平台费率
3. **批量导入** - Excel/CSV 文件批量计算
4. **移动版本** - MAUI 跨平台版本
5. **Web 版本** - ASP.NET Core 网页版

## 🎯 下一步

1. ✅ 构建并运行：`dotnet run`
2. ✅ 测试功能：输入数据计算售价
3. ✅ 修改配置：调整费率观察变化
4. ✅ 发布 EXE：生成可分发的执行文件

---

**需要更多帮助？** 查看 [README.md](README.md) 了解更多详情。

# 🎯 OZON 定价计算器 · Glass Edition

一款专为跨境电商卖家设计的 OZON 平台专用利润计算工具，采用 **Mac 风格白玻璃极简设计**。

## ✨ 核心特性

### 🚀 功能亮点
- **秒速计算**：输入成本和重量，立即获得最优售价
- **智能定价**：内置完整的 OZON 定价公式
- **多物流支持**：6 种物流方式自动适配
- **费率配置**：佣金、广告、货损、利润四大费率可自定义
- **自动保存**：配置参数自动持久化存储
- **一键复制**：计算结果快速复制到剪贴板

### 🎨 界面设计
- **白玻璃风格**：Mac 风格半透明效果
- **大圆角设计**：8-24px 柔和圆角
- **响应式布局**：美观的视觉层级
- **平滑交互**：实时计算无延迟

## 🛠️ 技术栈

- **框架**：.NET 8.0 WPF
- **语言**：C# 12.0
- **平台**：Windows (win-x64)
- **UI 技术**：XAML + 样式绑定
- **数据存储**：JSON 配置文件

## 🚀 快速开始

### 方法一：直接运行（推荐）

1. **下载发布版本**：
   - 从 GitHub Releases 下载最新的 `OzonCalc.zip`
   - 解压到任意目录

2. **运行应用**：
   - 双击 `OzonCalc.exe` 即可启动
   - **无需安装 .NET**，自包含运行时

### 方法二：从源码构建

1. **克隆仓库**：
   ```bash
   git clone https://github.com/StarKang1/Ozoncal.git
   cd Ozoncal
   ```

2. **构建项目**：
   ```bash
   dotnet build
   ```

3. **运行应用**：
   ```bash
   dotnet run
   ```

## 📖 使用指南

### 基本操作

1. **输入基础信息**：
   - **成本**：产品采购成本（人民币）
   - **重量**：产品重量（克）
   - **物流方式**：选择合适的物流渠道

2. **设置费率**：
   - **佣金**：OZON 平台佣金比例
   - **广告**：广告投放费用比例
   - **货损**：产品损耗率
   - **利润**：期望利润率

3. **查看结果**：
   - 物流成本：自动计算的物流费用
   - 实际成本：成本 + 货损
   - 推荐售价：最终的 OZON 平台售价

4. **复制售价**：
   - 点击「复制售价」按钮，价格会自动复制到剪贴板

### 物流方式说明

| 物流方式 | 重量范围 | 价格范围（卢布） | 适用场景 |
|---------|---------|----------------|----------|
| Extra Small | 1g - 500g | 最高1,500 | 小配件、轻量级商品 |
| Budget | 501g - 25,000g | 最高1,500 | 大体积但低价值商品 |
| Small | 1g - 2,000g | 1,501 - 7,000 | 中等价值的小物件 |
| Big | 2,001g - 25,000g | 1,501 - 7,000 | 中等价值的大物件 |
| Premium Small | 1g - 5,000g | 7,001 - 250,000 | 高价值的小物件 |
| Premium Big | 5,001g - 25,000g | 7,001 - 250,000 | 高价值的大物件 |

## 🔧 项目结构

```
Ozoncal/
├── App.xaml              # 应用资源定义
├── App.xaml.cs          # 应用启动逻辑
├── MainWindow.xaml      # 主界面（584 行 XAML）
├── MainWindow.xaml.cs   # 事件处理和计算逻辑
├── Models/
│   └── PriceModel.cs    # 数据模型（95 行）
├── Services/
│   └── PriceService.cs  # 计算服务（98 行）
├── Helpers/
│   └── ConfigManager.cs # 配置管理（125 行）
├── OzonPriceCalculator.csproj  # 项目配置
├── OzonPriceCalculator.sln     # Visual Studio Solution
└── README.md            # 项目说明（本文）
```

## 📊 核心计算公式

### 物流成本计算
```
物流成本 = 3 + 0.045 × 重量(克)
```

### 定价公式
```
实际成本 = 成本 × (1 + 货损率)
建议售价 = (物流成本 + 实际成本) ÷ (1 - 佣金 - 广告 - 利润)
最终售价 = CEILING(建议售价)
```

### 测试案例（默认参数）
```
输入：
  成本 = ¥20
  重量 = 200g
  佣金 = 20%
  广告 = 10%
  货损 = 20%
  利润 = 30%

计算过程：
  物流 = 3 + 0.045×200 = ¥12.00
  实际成本 = 20 × 1.20 = ¥24.00
  销售费率 = 1 - 0.60 = 0.40
  售价 = (12 + 24) ÷ 0.40 = ¥90.00
  最终 = CEILING(90.00) = ¥90
```

## ⚙️ 配置管理

### 配置文件位置
```
Windows: C:\Users\YourName\AppData\Roaming\OzonCalc\config.json
```

### 配置格式
```json
{
  "commission": 0.2,      // 佣金比例
  "ads": 0.1,            // 广告比例
  "damage": 0.2,         // 货损比例
  "profit": 0.3,         // 利润比例
  "ship_formula": "3 + 0.045 * weight"  // 物流计算公式
}
```

## 📦 构建和发布

### 开发构建
```bash
dotnet build
```

### 自包含发布（推荐）
```bash
dotnet publish -c Release -r win-x64 \
  -p:PublishSingleFile=true \
  --self-contained \
  -p:IncludeNativeLibrariesForSelfExtract=true
```

### 发布产物
```
bin\Release\net8.0-windows\win-x64\publish\OzonCalc.exe
```

## ❓ 常见问题

### Q: 为什么启动时提示需要安装 .NET？
A: 请使用自包含发布版本，或安装 .NET 8.0 运行时。

### Q: 物流公式可以修改吗？
A: 可以，修改配置文件中的 `ship_formula` 字段即可。

### Q: 配置文件在哪里？
A: `%APPDATA%\OzonCalc\config.json`

### Q: 能在 Mac 上运行吗？
A: 可以通过 .NET MAUI 跨平台改造实现。

## 🤝 贡献指南

1. **Fork 仓库**
2. **创建分支**：`git checkout -b feature/your-feature`
3. **提交更改**：`git commit -m "Add your feature"`
4. **推送到分支**：`git push origin feature/your-feature`
5. **创建 Pull Request**

## 📄 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。

## 🎉 鸣谢

- 感谢 .NET 团队提供的优秀框架
- 感谢 WPF 提供的强大 UI 能力
- 感谢所有贡献者的支持

## 📞 技术支持

如有问题或建议，请在 GitHub Issues 中提交，或联系项目维护者。

---

**版本**：v1.0.0
**更新日期**：2026-03-31
**状态**：✅ 稳定可用

*祝您使用愉快！* 🚀
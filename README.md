# OZON 定价计算器

一个用于计算OZON平台产品定价的工具，支持多种物流方式和费用计算。

## 功能特点

- **多种物流方式**：支持Extra Small、Budget、Small、Big、Premium Small、Premium Big等物流方式
- **详细的费用计算**：包括产品成本、重量、佣金、广告费、货损等因素
- **重量和价格限制检查**：根据不同物流方式的限制条件进行检查和提示
- **自定义标题栏**：支持最小化、固定和关闭功能
- **窗口拖拽**：可以通过鼠标拖动窗口
- **响应式布局**：美观的Mac风格界面

## 物流方式限制

| 物流方式 | 重量范围 | 价格范围（卢布） |
|---------|---------|----------------|
| Extra Small | 1g - 500g | 最高1,500 |
| Budget | 501g - 25,000g | 最高1,500 |
| Small | 1g - 2,000g | 1,501 - 7,000 |
| Big | 2,001g - 25,000g | 1,501 - 7,000 |
| Premium Small | 1g - 5,000g | 7,001 - 250,000 |
| Premium Big | 5,001g - 25,000g | 7,001 - 250,000 |

## 使用说明

1. **基础信息**：输入产品成本、重量和选择物流方式
2. **费率设置**：设置佣金、广告费、货损率和利润率
3. **查看结果**：系统会自动计算物流成本、实际成本和推荐售价
4. **复制售价**：可以一键复制计算出的推荐售价

## 技术栈

- C#
- WPF (Windows Presentation Foundation)
- .NET 8.0

## 构建和运行

1. 克隆仓库：`git clone git@github.com:StarKang1/Ozoncal.git`
2. 打开项目：使用Visual Studio或Visual Studio Code打开项目
3. 构建项目：`dotnet build`
4. 运行项目：`dotnet run`

## 项目结构

- `MainWindow.xaml`：主窗口界面
- `MainWindow.xaml.cs`：主窗口逻辑
- `Models/PriceModel.cs`：数据模型
- `Services/PriceService.cs`：价格计算服务
- `Helpers/ConfigManager.cs`：配置管理

## 注意事项

- 价格转换使用了假设的汇率（1人民币=10卢布），实际使用时请根据当前汇率进行调整
- 物流公式和费率设置会自动保存，下次启动时会加载之前的配置

## 贡献

欢迎提交Issue和Pull Request！